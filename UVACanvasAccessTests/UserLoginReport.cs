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
using static UVACanvasAccess.Structures.Authentications.EventType;

namespace UVACanvasAccessTests {
    public class UserLoginReport {
        private readonly ITestOutputHelper _testOutputHelper;

        private readonly Api _api;
        
        public UserLoginReport(ITestOutputHelper testOutputHelper) {
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
                var authEvents = await _api.StreamUserAuthenticationEvents(user.Id).Where(e => e.Event == Login)
                                           .ToListAsync();

                if (authEvents.Count == 0) {
                    continue;
                }
                
                var userObj = new JObject {
                    ["userSisId"] = user.SisUserId,
                    ["userIsTeacher"] = await user.IsTeacher(),
                    ["mostRecentLogin"] = authEvents.Select(e => e.CreatedAt).Max().ToIso8601Date()
                };

                var loginsArr = new JArray();

                foreach (var authEvent in authEvents) {
                    loginsArr.Add(authEvent.CreatedAt.ToIso8601Date());
                }

                userObj["logins"] = loginsArr;
                usersObj[user.Id.ToString()] = userObj;
                ++usersInReport;
            }

            document["usersInReport"] = usersInReport;

            _testOutputHelper.WriteLine(document.ToString(Formatting.Indented));
        }
    }
}
