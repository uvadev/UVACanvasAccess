using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Tomlyn;
using Tomlyn.Model;
using Tomlyn.Syntax;
using static System.Environment;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Util;
using static Newtonsoft.Json.Formatting;
using static UVACanvasAccess.ApiParts.Api.CourseEnrollmentType;

namespace SuperReport {
    internal static class Program {
    
        public static async Task Main(string[] args) {
            var shareDir = GetFolderPath(SpecialFolder.LocalApplicationData, 
                                          SpecialFolderOption.Create);
            var reportDir = Path.Combine(shareDir, "uva_super_report");

            if (!Directory.Exists(reportDir)) {
                Directory.CreateDirectory(reportDir);
            }

            var configPath = Path.Combine(reportDir, "config.toml");

            Console.WriteLine($"Using config path: {configPath}");

            if (!File.Exists(configPath)) {
                Console.WriteLine($"No config file found at path {configPath}. Trying to create it.");

                var doc = new DocumentSyntax {
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
                                {"assignments_per_course", 2},
                                {"submissions_per_assignment", 10},
                                {"teachers_only", false}
                            }
                        }
                    }
                };
                
                File.WriteAllText(configPath, doc.ToString());

                Console.WriteLine("Wrote bare config file. Please go put in your token.");
                return;
            }

            Console.WriteLine("Found config file.");

            var configDoc = Toml.Parse(File.ReadAllText(configPath));
            if (configDoc.HasErrors) {
                Console.WriteLine("The config file had errors in it.");

                foreach (var diag in configDoc.Diagnostics) {
                    Console.WriteLine(diag.ToString());
                }
                
                return;
            }

            var config = configDoc.ToModel();
            var token = (string) ((TomlTable) config["tokens"])["token"];
            var sampleTake = (int)(long) ((TomlTable) config["limits"])["sample_take"];
            var sampleSkip = (int)(long) ((TomlTable) config["limits"])["sample_skip"];
            var assignmentsPerCourse = (int)(long) ((TomlTable) config["limits"])["assignments_per_course"];
            var submissionsPerAssignment = (int)(long) ((TomlTable) config["limits"])["submissions_per_assignment"];
            var teachersOnly = ((TomlTable) config["limits"])["teachers_only"] as bool? ?? false;

            Console.WriteLine($"SKIPPING {sampleSkip} users.");
            Console.WriteLine($"TAKING {sampleTake} users.");
            
            // --------------------------------------------------------------------

            var api = new Api(token, "https://uview.instructure.com/api/v1/");
            
            var studentsObj = new JObject();
            var teachersObj = new JObject();
            var coursesObj = new JObject();
            var assignmentsOverallObj = new JObject();
            var assignmentsIndividualObj = new JObject();
            var coursePerformanceObj = new JObject();

            var started = DateTime.Now;
            
            var document = new JObject {
                ["teachers"] = teachersObj,
                ["students"] = studentsObj,
                ["courses"] = coursesObj,
                ["assignmentsOverall"] = assignmentsOverallObj,
                ["assignmentsIndividual"] = assignmentsIndividualObj,
                ["coursePerformance"] = coursePerformanceObj,
                ["limits"] = new JObject {
                    ["sampleTake"] = sampleTake,
                    ["sampleSkip"] = sampleSkip,
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
            
            sample = sample.Skip(sampleSkip)
                           .Take(sampleTake);

            await foreach (var user in sample) {
                if (await user.IsTeacher()) {
                    if (!teachersObj.ContainsKey(user.Id.ToString())) {
                        teachersObj[user.Id.ToString()] = new JObject {
                            ["sisId"] = user.SisUserId,
                            ["fullName"] = user.Name
                        };
                    }

                    await foreach (var enrollment in api.StreamUserEnrollments(user.Id, TeacherEnrollment.Yield())) {
                        var course = await api.GetCourse(enrollment.CourseId);
                        
                        if (!coursesObj.ContainsKey(course.Id.ToString())) {
                            coursesObj[course.Id.ToString()] = new JObject {
                                ["sisId"] = course.SisCourseId,
                                ["name"] = course.Name
                            };
                        }
                        
                        var assignments = await api.StreamCourseAssignments(course.Id)
                                                   .Where(a => a.Published)
                                                   .Take(assignmentsPerCourse)
                                                   .ToListAsync();

                        foreach (var assignment in assignments) {
                            var submissions = await api.StreamSubmissionVersions(course.Id, assignment.Id)
                                                       .Where(s => s.Score != null)
                                                       .GroupBy(s => s.UserId)
                                                       .SelectAwait(sg => sg.FirstAsync())
                                                       .Take(submissionsPerAssignment)
                                                       .ToListAsync();
                            
                            if (!submissions.Any()) {
                                continue;
                            }
                            
                            var scores = submissions.Select(s => s.Score)
                                                    .Cast<decimal>()
                                                    .OrderBy(d => d)
                                                    .ToList();

                            var scoresMean = scores.Average();
                        
                            var scoresQ1Point = scores.Count / 4;
                            var scoresQ2Point = scores.Count / 2;
                            var scoresQ3Point = scores.Count / 4 * 3;
                        
                            var scoresQ1 = scores.Count % 2 != 0 ? scores[scoresQ1Point]
                                                                 : (scores[scoresQ1Point] + scores[scoresQ1Point - 1]) / 2;
                        
                            var scoresMedian = scores.Count % 2 != 0 ? scores[scoresQ2Point]
                                                                     : (scores[scoresQ2Point] + scores[scoresQ2Point - 1]) / 2;
                        
                            var scoresQ3 = scores.Count % 2 != 0 ? scores[scoresQ3Point]
                                                                 : (scores[scoresQ3Point] + scores[scoresQ3Point - 1]) / 2;
                        
                            var scoresMode = scores.GroupBy(s => s)
                                                   .OrderByDescending(g => g.Count())
                                                   .First()
                                                   .Key;

                            var sigma = Math.Sqrt(scores.Select(Convert.ToDouble)
                                                        .Aggregate(0.0, (acc, s) => acc + Math.Pow(s - Convert.ToDouble(scoresMean), 2)) / scores.Count);
                            
                            assignmentsOverallObj[assignment.Id.ToString()] = new JObject {
                                ["assignmentName"] = assignment.Name,
                                ["courseId"] = course.Id,
                                ["teacherId"] = user.Id,
                                ["countedInFinalGrade"] = !(assignment.OmitFromFinalGrade ?? false),
                                ["pointsPossible"] = assignment.PointsPossible,
                                ["createdDate"] = assignment.CreatedAt.ToIso8601Date(),
                                ["dueDate"] = assignment.DueAt?.ToIso8601Date(),
                                ["gradesInSample"] = submissions.Count,
                                ["meanScore"] = scoresMean,
                                ["modeScore"] = scoresMode,
                                ["25thPercentileScore"] = scoresQ1,
                                ["medianScore"] = scoresMedian,
                                ["75thPercentileScore"] = scoresQ3,
                                ["scoreStandardDeviation"] = sigma
                            };
                        }
                    }
                } else {
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
                        
                        Debug.Assert(!coursePerformanceObj.ContainsKey(enrollment.Id.ToString()));

                        var grades = enrollment.Grades;
                        coursePerformanceObj[enrollment.Id.ToString()] = new JObject {
                            ["studentId"] = user.Id,
                            ["courseId"] = enrollment.CourseId,
                            ["currentLetterGrade"] = grades.CurrentGrade,
                            ["finalLetterGrade"] = grades.FinalGrade,
                            ["currentScore"] = grades.CurrentScore,
                            ["finalScore"] = grades.FinalScore
                        };
                    }
                }
            }

            document["dateCompleted"] = DateTime.Now.ToIso8601Date();
            document["usersInReport"] = studentsObj.Count + teachersObj.Count;

            var outPath = Path.Combine(reportDir, $"SuperReport_{started.Ticks}.json");
            File.WriteAllText(outPath, document.ToString(Indented) + "\n");
            Console.WriteLine($"Wrote report to {outPath}");
        }
    }
}
