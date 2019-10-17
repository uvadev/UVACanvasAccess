using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AppUtils;
using Newtonsoft.Json.Linq;
using Tomlyn.Syntax;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Util;
using static Newtonsoft.Json.Formatting;
using static UVACanvasAccess.ApiParts.Api.CourseEnrollmentType;

namespace SuperReport {
    internal static class Program {
    
        public static async Task Main(string[] args) {
            var home = new AppHome("uva_super_report");

            Console.WriteLine($"Using config path: {home.ConfigPath}");

            if (!home.ConfigPresent()) {
                Console.WriteLine("Need to generate a config file.");
                
                home.CreateConfig( new DocumentSyntax {
                    Tables = {
                        new TableSyntax("tokens") {
                            Items = {
                                {"token", "PUT_TOKEN_HERE"}
                            }
                        },
                        new TableSyntax("limits") {
                            Items = {
                                {"sample_skip", 0},
                                {"sample_take", 10},
                                {"courses_per_teacher", 5},
                                {"assignments_per_course", 2},
                                {"submissions_per_assignment", 10},
                                {"teachers_only", false}
                            }
                        }
                    }
                });

                Console.WriteLine("Created a new config file. Please go put in your token.");
                return;
            }

            Console.WriteLine("Found config file.");

            var config = home.GetConfig();
            Debug.Assert(config != null, nameof(config) + " != null");
            
            var token = config.GetTable("tokens").Get<string>("token");

            var limits = config.GetTable("limits");
            var sampleTake = (int) limits.GetOr<long>("sample_take");
            var sampleSkip = (int) limits.GetOr<long>("sample_skip");
            var coursesPerTeacher = (int) limits.GetOr<long>("courses_per_teacher");
            var assignmentsPerCourse = (int) limits.GetOr<long>("assignments_per_course");
            var submissionsPerAssignment = (int) limits.GetOr<long>("submissions_per_assignment");
            var teachersOnly = limits.GetOr<bool>("teachers_only");

            Console.WriteLine($"SKIPPING {sampleSkip} users.");
            Console.WriteLine($"TAKING {(sampleTake == default ? "ALL" : sampleTake.ToString())} users.");
            Console.WriteLine($"TAKING {(coursesPerTeacher == default ? "ALL" : coursesPerTeacher.ToString())} courses per teacher.");
            Console.WriteLine($"TAKING {(assignmentsPerCourse == default ? "ALL" : assignmentsPerCourse.ToString())} assignments per course.");
            Console.WriteLine($"TAKING {(submissionsPerAssignment == default ? "ALL" : submissionsPerAssignment.ToString())} submissions per assignment.");
            
            // --------------------------------------------------------------------

            var api = new Api(token, "https://uview.instructure.com/api/v1/");
            
            var studentsObj = new JObject();
            var teachersObj = new JObject();
            var coursesObj = new JObject();
            var assignmentsOverallObj = new JObject();
            var assignmentsIndividualObj = new JObject();
            var individualCoursePerformanceObj = new JObject();
            var overallCoursePerformanceObj = new JObject();

            var started = DateTime.Now;
            
            var document = new JObject {
                ["teachers"] = teachersObj,
                ["students"] = studentsObj,
                ["courses"] = coursesObj,
                ["assignmentsOverall"] = assignmentsOverallObj,
                ["assignmentsIndividual"] = assignmentsIndividualObj,
                ["individualCoursePerformance"] = individualCoursePerformanceObj,
                ["overallCoursePerformance"] = overallCoursePerformanceObj,
                ["limits"] = new JObject {
                    ["sampleTake"] = sampleTake,
                    ["sampleSkip"] = sampleSkip,
                    ["coursesPerTeacher"] = coursesPerTeacher,
                    ["assignmentsPerCourse"] = assignmentsPerCourse,
                    ["submissionsPerAssignment"] = submissionsPerAssignment,
                    ["teachers_only"] = teachersOnly
                },
                ["dateStarted"] = started.ToIso8601Date()
            };

            var sample = api.StreamUsers()
                            .Where(u => !u.Name.ToLowerInvariant().Contains("test"))
                            .Where(u => !u.SisUserId?.StartsWith("pG") ?? false);

            if (teachersOnly) {
                Console.WriteLine("Taking TEACHERS ONLY.");
                sample = sample.WhereAwait(async u => await u.IsTeacher());
            }
            
            sample = sample.Skip(sampleSkip);

            if (sampleTake != default) {
                sample = sample.Take(sampleTake);
            }

            await foreach (var user in sample) {
                if (await user.IsTeacher()) {
                    #region CurrentUserIsTeacher

                    if (!teachersObj.ContainsKey(user.Id.ToString())) {
                        teachersObj[user.Id.ToString()] = new JObject {
                            ["sisId"] = user.SisUserId,
                            ["fullName"] = user.Name
                        };
                    }

                    var enrollmentsStream = api.StreamUserEnrollments(user.Id, TeacherEnrollment.Yield());
                    if (coursesPerTeacher != default) {
                        enrollmentsStream = enrollmentsStream.Take(coursesPerTeacher);
                    }
                    
                    await foreach (var enrollment in enrollmentsStream) {
                        var course = await api.GetCourse(enrollment.CourseId);
                        
                        if (!coursesObj.ContainsKey(course.Id.ToString())) {
                            coursesObj[course.Id.ToString()] = new JObject {
                                ["sisId"] = course.SisCourseId,
                                ["name"] = course.Name
                            };
                        }

                        if (!overallCoursePerformanceObj.ContainsKey(course.Id.ToString())) {
                            overallCoursePerformanceObj[course.Id.ToString()] = new JObject {
                                
                            };
                        }

                        var assignmentsStream = api.StreamCourseAssignments(course.Id)
                                                   .Where(a => a.Published);

                        if (assignmentsPerCourse != default) {
                            assignmentsStream = assignmentsStream.Take(assignmentsPerCourse);
                        }
                        
                        var assignments = await assignmentsStream.ToListAsync();

                        foreach (var assignment in assignments) {

                            var submissionsStream = api.StreamSubmissionVersions(course.Id, assignment.Id)
                                                       .Where(s => s.Score != null)
                                                       .GroupBy(s => s.UserId)
                                                       .SelectAwait(sg => sg.FirstAsync());

                            if (submissionsPerAssignment != default) {
                                submissionsStream = submissionsStream.Take(submissionsPerAssignment);
                            }
                            
                            var submissions = await submissionsStream.ToListAsync();
                            
                            if (!submissions.Any()) {
                                continue;
                            }
                            
                            var scores = submissions.Select(s => s.Score)
                                                    .Cast<decimal>()
                                                    .ToList();

                            var stats = new Stats(scores);
                            
                            assignmentsOverallObj[assignment.Id.ToString()] = new JObject {
                                ["assignmentName"] = assignment.Name,
                                ["courseId"] = course.Id,
                                ["teacherId"] = user.Id,
                                ["countedInFinalGrade"] = !(assignment.OmitFromFinalGrade ?? false),
                                ["pointsPossible"] = assignment.PointsPossible,
                                ["createdDate"] = assignment.CreatedAt.ToIso8601Date(),
                                ["dueDate"] = assignment.DueAt?.ToIso8601Date(),
                                ["gradesInSample"] = submissions.Count,
                                ["meanScore"] = stats.Mean,
                                ["modeScore"] = stats.Mode,
                                ["25thPercentileScore"] = stats.Q1,
                                ["medianScore"] = stats.Median,
                                ["75thPercentileScore"] = stats.Q3,
                                ["scoreStandardDeviation"] = stats.Sigma
                            };
                        }
                    }

                    #endregion
                } else {
                    #region CurrentUserIsStudent

                    if (!studentsObj.ContainsKey(user.Id.ToString())) {
                        studentsObj[user.Id.ToString()] = new JObject {
                            ["sisId"] = user.SisUserId,
                            ["fullName"] = user.Name
                        };
                    }

                    await foreach (var enrollment in api.StreamUserEnrollments(user.Id)) {

                        if (!coursesObj.ContainsKey(enrollment.CourseId.ToString())) {
                            var course = await api.GetCourse(enrollment.CourseId);
                            coursesObj[course.Id.ToString()] = new JObject {
                                ["sisId"] = course.SisCourseId,
                                ["name"] = course.Name
                            };
                        }
                        
                        Debug.Assert(!individualCoursePerformanceObj.ContainsKey(enrollment.Id.ToString()));

                        var grades = enrollment.Grades;
                        individualCoursePerformanceObj[enrollment.Id.ToString()] = new JObject {
                            ["studentId"] = user.Id,
                            ["courseId"] = enrollment.CourseId,
                            ["currentLetterGrade"] = grades.CurrentGrade,
                            ["finalLetterGrade"] = grades.FinalGrade,
                            ["currentScore"] = grades.CurrentScore,
                            ["finalScore"] = grades.FinalScore
                        };
                    }

                    #endregion
                }
            }

            document["dateCompleted"] = DateTime.Now.ToIso8601Date();
            document["usersInReport"] = studentsObj.Count + teachersObj.Count;

            var outPath = Path.Combine(home.NsDir, $"SuperReport_{started.Ticks}.json");
            File.WriteAllText(outPath, document.ToString(Indented) + "\n");
            Console.WriteLine($"Wrote report to {outPath}");
        }
    }
}
