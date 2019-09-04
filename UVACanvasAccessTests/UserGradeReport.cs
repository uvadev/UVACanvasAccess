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

namespace UVACanvasAccessTests {
    
    public class UserGradeReport {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly Api _api;
        
        public UserGradeReport(ITestOutputHelper testOutputHelper) {
            _testOutputHelper = testOutputHelper;
            DotEnv.Config();
            _api = new Api(Environment.GetEnvironmentVariable("TEST_TOKEN") 
                           ?? throw new ArgumentException(".env should contain TEST_TOKEN"), 
                           "https://uview.instructure.com/api/v1/");
        }

        [Fact]
        public async Task Run() {
            IAsyncEnumerable<User> sample = _api.StreamUsers()
                                                .Where(u => !u.Name.ToLowerInvariant().Contains("test"))
                                                .Where(u => !u.SisUserId?.StartsWith("pG") ?? false)
                                                .Take(10);

            uint usersInReport = 0;
            var usersObj = new JObject();
            
            var document = new JObject {
                ["dateGenerated"] = DateTime.Now.ToIso8601Date(),
                ["users"] = usersObj
            };

            await foreach (var user in sample) {
                var userObj = new JObject {
                    ["userSis"] = user.SisUserId, 
                    ["userFullName"] = user.Name
                };

                await foreach (var enrollment in _api.StreamUserEnrollments(user.Id)) {
                    var grades = enrollment.Grades;

                    userObj[enrollment.CourseId.ToString()] = new JObject {
                        ["courseSis"] = enrollment.SisCourseId,
                        ["courseName"] = await _api.GetCourse(enrollment.CourseId).ThenApply(c => c.Name),
                        ["currentLetterGrade"] = grades.CurrentGrade,
                        ["finalLetterGrade"] = grades.FinalGrade,
                        ["currentScore"] = grades.CurrentScore,
                        ["finalScore"] = grades.FinalScore
                    };
                }

                usersObj[user.Id.ToString()] = userObj;
                ++usersInReport;
            }

            document["usersInReport"] = usersInReport;

            _testOutputHelper.WriteLine(document.ToString(Formatting.Indented));
        }
    }
}
