using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AppUtils;
using C5;
using Newtonsoft.Json.Linq;
using Tomlyn.Syntax;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Structures.Assignments;
using UVACanvasAccess.Util;
using static Newtonsoft.Json.Formatting;
using static UVACanvasAccess.ApiParts.Api.CourseEnrollmentType;
using MoreLinq;

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
            var teacherPerformanceObj = new JObject();

            var started = DateTime.Now;
            
            var document = new JObject {
                ["teachers"] = teachersObj,
                ["students"] = studentsObj,
                ["courses"] = coursesObj,
                ["assignmentsOverall"] = assignmentsOverallObj,
                ["assignmentsIndividual"] = assignmentsIndividualObj,
                ["individualCoursePerformance"] = individualCoursePerformanceObj,
                ["overallCoursePerformance"] = overallCoursePerformanceObj,
                ["teacherPerformance"] = teacherPerformanceObj,
                ["limits"] = new JObject {
                    ["sampleTake"] = sampleTake,
                    ["sampleSkip"] = sampleSkip,
                    ["coursesPerTeacher"] = coursesPerTeacher,
                    ["assignmentsPerCourse"] = assignmentsPerCourse,
                    ["submissionsPerAssignment"] = submissionsPerAssignment,
                    ["teachersOnly"] = teachersOnly
                },
                ["dateStarted"] = started.ToIso8601Date()
            };

            var sample = api.StreamUsers()
                            .Where(u => !u.Name.ToLowerInvariant().Contains("test"))
                            .Where(u => !u.SisUserId?.StartsWith("pG") ?? false);

            if (teachersOnly) {
                Console.WriteLine("TAKING TEACHERS ONLY.");
                sample = sample.WhereAwait(async u => await u.IsTeacher());
            }
            
            sample = sample.Skip(sampleSkip);

            if (sampleTake != default) {
                sample = sample.Take(sampleTake);
            }

            await foreach (var user in sample) {
                try {
                    if (await user.IsTeacher()) {
                        #region CurrentUserIsTeacher

                        if (!teachersObj.ContainsKey(user.Id.ToString())) {
                            teachersObj[user.Id.ToString()] = new JObject {
                                ["sisId"] = user.SisUserId,
                                ["fullName"] = user.Name
                            };
                        }

                        var enrollmentsStream = api.StreamUserEnrollments(user.Id, TeacherEnrollment.Yield())
                                                   .SelectAwait(async e => (e, await api.GetCourse(e.CourseId)))
                                                   .Where(ec => !ec.Item2.Name.ToLowerInvariant().Contains("advisory"));

                        if (coursesPerTeacher != default) {
                            enrollmentsStream = enrollmentsStream.Take(coursesPerTeacher);
                        }

                        var ungradedAssignments = new IntervalHeap<Assignment>(Comparer<Assignment>.Create((a1, a2) => {
                            Debug.Assert(a1.DueAt != null && a2.DueAt != null);
                            return a1.DueAt.Value.CompareTo(a2.DueAt.Value);
                        }));

                        await foreach (var (enrollment, course) in enrollmentsStream) {

                            if (!coursesObj.ContainsKey(course.Id.ToString())) {
                                coursesObj[course.Id.ToString()] = new JObject {
                                    ["sisId"] = course.SisCourseId,
                                    ["name"] = course.Name
                                };
                            }

                            if (!overallCoursePerformanceObj.ContainsKey(course.Id.ToString())) {

                                var studentEnrollments =
                                    api.StreamCourseEnrollments(course.Id, StudentEnrollment.Yield());

                                var courseScores = new List<decimal>();
                                await foreach (var studentEnrollment in studentEnrollments) {
                                    var grades = studentEnrollment.Grades;

                                    if (!individualCoursePerformanceObj.ContainsKey(enrollment.Id.ToString())) {
                                        individualCoursePerformanceObj[enrollment.Id.ToString()] = new JObject {
                                            ["studentId"] = user.Id,
                                            ["courseId"] = enrollment.CourseId,
                                            ["currentLetterGrade"] = grades.CurrentGrade,
                                            ["finalLetterGrade"] = grades.FinalGrade,
                                            ["currentScore"] = grades.CurrentScore,
                                            ["finalScore"] = grades.FinalScore
                                        };
                                    }

                                    var currentScore = grades.CurrentScore;
                                    courseScores.Add(string.IsNullOrEmpty(currentScore) ? 0
                                                                                        : Convert.ToDecimal(grades.CurrentScore));
                                }

                                if (!courseScores.Any()) {
                                    continue;
                                }

                                var courseScoreStats = new Stats(courseScores);
                                var pass = (decimal) courseScores.Count(s => s > 66.5m);

                                overallCoursePerformanceObj[course.Id.ToString()] = new JObject {
                                    ["gradesInSample"] = courseScores.Count,
                                    ["meanCourseScore"] = courseScoreStats.Mean,
                                    ["modeCourseScore"] = courseScoreStats.Mode,
                                    ["25thPercentileCourseScore"] = courseScoreStats.Q1,
                                    ["medianCourseScore"] = courseScoreStats.Median,
                                    ["75thPercentileCourseScore"] = courseScoreStats.Q3,
                                    ["courseScoreStandardDeviation"] = courseScoreStats.Sigma,
                                    ["coursePassFailRatio"] = pass == courseScores.Count ? -1
                                                                                         : pass / (courseScores.Count - pass)
                                };
                            }

                            var assignments = (await api.StreamCourseAssignments(course.Id)
                                                        .Where(a => a.Published)
                                                        .ToListAsync())
                                                        .DistinctBy(a => a.Id);

                            if (assignmentsPerCourse != default) {
                                assignments = assignments.Take(assignmentsPerCourse);
                            }

                            foreach (var assignment in assignments) {

                                var allSubmissionsStream = api.StreamSubmissionVersions(course.Id, assignment.Id);

                                if (assignment.DueAt != null) {
                                    var ungraded = await allSubmissionsStream.GroupBy(s => s.UserId)
                                                                             .AnyAwaitAsync(async g =>
                                                                                                !await g.AnyAsync(s => s.Score != null));

                                    if (ungraded) {
                                        ungradedAssignments.Add(assignment);
                                    }
                                }

                                var submissionsStream = allSubmissionsStream.Where(s => s.Score != null)
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

                                if (assignmentsOverallObj.ContainsKey($"c_{course.Id}|a_{assignment.Id}")) {
                                    Console.WriteLine($"WARN: Duplicated assignment?\nc={course.Id}\na={assignment.Id}\n\n{assignment}\nSkipping!------\n");
                                    continue;
                                }

                                assignmentsOverallObj[$"c_{course.Id}|a_{assignment.Id}"] = new JObject {
                                    ["assignmentName"] = assignment.Name,
                                    ["assignmentId"] = assignment.Id,
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

                                foreach (var submission in submissions) {
                                    var submitter = await api.GetUser(submission.UserId);
                                    Debug.Assert(!assignmentsIndividualObj.ContainsKey($"c_{course.Id}|a_{assignment.Id}|s_{submitter.Id}"));

                                    var score = submission.Score.Value;
                                    var z = Convert.ToDouble(score - stats.Mean) / stats.Sigma;
                                    var iqr = stats.Q3 - stats.Q1;

                                    assignmentsIndividualObj[$"c_{course.Id}|a_{assignment.Id}|s_{submitter.Id}"] = new JObject {
                                        ["assignmentId"] = assignment.Id,
                                        ["courseId"] = course.Id,
                                        ["userId"] = submitter.Id,
                                        ["submissionDate"] = submission.SubmittedAt,
                                        ["pointsEarned"] = score,
                                        ["z"] = z,
                                        ["isUnusual"] = Math.Abs(z) > 1.96,
                                        ["isMinorOutlier"] = score < stats.Q1 - iqr * 1.5m
                                                          || score > stats.Q3 + iqr * 1.5m,
                                        ["isMajorOutlier"] = score < stats.Q1 - iqr * 3m
                                                          || score > stats.Q3 + iqr * 3m,
                                    };
                                }
                            }
                        }

                        var ungradedAssignmentsList = new List<Assignment>(ungradedAssignments.Count);
                        while (ungradedAssignments.Count > 0) {
                            ungradedAssignmentsList.Add(ungradedAssignments.DeleteMin());
                        }

                        teacherPerformanceObj[user.Id.ToString()] = new JObject {
                            ["ungradedAssignments"] = new JArray(ungradedAssignmentsList.Select(a => a.Id).Distinct())
                        };

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
                        }

                        #endregion
                    }
                } catch (Exception e) {
                    Console.WriteLine($"Caught an exception while processing user id {user.Id}\n{e}\n-------\n");
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
