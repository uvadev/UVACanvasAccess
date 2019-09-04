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
    public class UserCourseActivityReport {
        private readonly ITestOutputHelper _testOutputHelper;

        private readonly Api _api;
        
        public UserCourseActivityReport(ITestOutputHelper testOutputHelper) {
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
                var userCoursesObj = new JObject {
                    ["userSisId"] = user.SisUserId
                };

                await foreach (var userEnrollment in _api.StreamUserEnrollments(user.Id, new[] {StudentEnrollment})) {
                    if (userEnrollment.LastActivityAt == null)
                        continue;

                    userCoursesObj[userEnrollment.CourseId.ToString()] = JToken.FromObject(new {
                        courseSisId = userEnrollment.SisCourseId,
                        courseName = await _api.GetCourse(userEnrollment.CourseId).ThenApply(c => c.Name),
                        secondsSpent = userEnrollment.TotalActivityTime,
                        lastActivity = userEnrollment.LastActivityAt?.ToIso8601Date()
                    });
                }

                usersObj[user.Id.ToString()] = userCoursesObj;
                ++usersInReport;
            }

            document["usersInReport"] = usersInReport;

            _testOutputHelper.WriteLine(document.ToString(Formatting.Indented));
        }
    }
}
