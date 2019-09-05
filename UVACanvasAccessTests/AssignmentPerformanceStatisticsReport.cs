using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotenv.net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Structures.Users;
using UVACanvasAccess.Util;
using Xunit;
using Xunit.Abstractions;
using static UVACanvasAccess.ApiParts.Api.CourseEnrollmentType;

namespace UVACanvasAccessTests {
    public class AssignmentPerformanceStatisticsReport {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly Api _api;
        
        public AssignmentPerformanceStatisticsReport(ITestOutputHelper testOutputHelper) {
            _testOutputHelper = testOutputHelper;
            DotEnv.Config();
            _api = new Api(Environment.GetEnvironmentVariable("TEST_TOKEN") 
                           ?? throw new ArgumentException(".env should contain TEST_TOKEN"), 
                           "https://uview.instructure.com/api/v1/");
        }

        [Fact]
        // Note: make sure to comment out the LIMIT lines below to get a full report
        // The full report for this one takes 5ever
        public async Task Run() {
            IAsyncEnumerable<User> sample = _api.StreamUsers()
                                                .Where(u => !u.Name.ToLowerInvariant().Contains("test"))
                                                .Where(u => !u.SisUserId?.StartsWith("pG") ?? false)
                                                .Where(u => u.IsTeacher().Result)
                                                .Skip(1)  //
                                                .Take(1); // LIMITS number of users reported, comment this for full report

            uint usersInReport = 0;
            var usersObj = new JObject();
            
            var document = new JObject {
                ["dateGenerated"] = DateTime.Now.ToIso8601Date(),
                ["users"] = usersObj
            };

            await foreach (var user in sample) {
                
                var userObj = new JObject {
                    ["teacherSis"] = user.SisUserId, 
                    ["teacherFullName"] = user.Name
                };
                
                await foreach (var enrollment in _api.StreamUserEnrollments(user.Id, new[] {TeacherEnrollment})) {
                    var course = await _api.GetCourse(enrollment.CourseId);
                    var assignments = await _api.StreamCourseAssignments(course.Id)
                                               .Where(a => a.Published)
                                               .Take(10) // LIMITS number of assignments reported, comment this for full report
                                               .ToListAsync();
                    
                    var courseObj = new JObject {
                        ["courseSis"] = enrollment.SisCourseId,
                        ["courseName"] = course.Name,
                        ["assignmentsCreated"] = assignments.Count
                    };

                    foreach (var assignment in assignments) {

                        var submissions = await _api.StreamSubmissionVersions(course.Id, assignment.Id)
                                                   .Where(s => s.Score != null)
                                                   .GroupBy(s => s.UserId)
                                                   .SelectAwait(sg => sg.FirstAsync())
                                                   //.Take(10) // LIMITS number of submissions reported, comment this for full report
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

                        var assignmentObj = new JObject {
                            ["assignmentName"] = assignment.Name,
                            ["countedInFinalGrade"] = !(assignment.OmitFromFinalGrade ?? false),
                            ["pointsPossible"] = assignment.PointsPossible,
                            ["createdDate"] = assignment.CreatedAt.ToIso8601Date(),
                            ["dueDate"] = assignment.DueAt?.ToIso8601Date(),
                            ["gradesInSample"] = submissions.Count,
                            ["stats"] = new JObject {
                                ["scores"] = new JObject {
                                    ["mean"] = scoresMean,
                                    ["mode"] = scoresMode,
                                    ["q1"] = scoresQ1,
                                    ["median"] = scoresMedian,
                                    ["q3"] = scoresQ3,
                                    ["sigma"] = sigma
                                }
                            }
                        };

                        courseObj[assignment.Id.ToString()] = assignmentObj;
                    }

                    userObj[course.Id.ToString()] = courseObj;
                }

                usersObj[user.Id.ToString()] = userObj;
                ++usersInReport;
            }

            document["usersInReport"] = usersInReport;

            _testOutputHelper.WriteLine(document.ToString(Formatting.Indented));
        }
    }
}
