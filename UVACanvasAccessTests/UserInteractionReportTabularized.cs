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
    
    public class UserInteractionReportTabularized {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly Api _api;
        
        public UserInteractionReportTabularized(ITestOutputHelper testOutputHelper) {
            _testOutputHelper = testOutputHelper;
            DotEnv.Config();
            _api = new Api(Environment.GetEnvironmentVariable("TEST_TOKEN") 
                           ?? throw new ArgumentException(".env should contain TEST_TOKEN"), 
                           "https://uview.instructure.com/api/v1/");
        }

        [Fact]
        // Note: make sure to comment out the LIMIT lines below to get a full report
        // The full report for this one takes for-ev-er
        public async Task Run() {
            IAsyncEnumerable<User> sample = _api.StreamUsers()
                                                .Where(u => !u.Name.ToLowerInvariant().Contains("test"))
                                                .Where(u => !u.SisUserId?.StartsWith("pG") ?? false)
                                                .Take(1); // LIMITS number of users reported, comment this for full report

            uint usersInReport = 0;
            var usersObj = new JObject();
            var pageViewsObj = new JObject();
            
            var document = new JObject {
                ["dateGenerated"] = DateTime.Now.ToIso8601Date(),
                ["users"] = usersObj,
                ["pageViews"] = pageViewsObj
            };

            await foreach (var user in sample) {
                var pageViewsArr = new JArray();
                var pageViews = await user.StreamPageViews()
                                          .Take(50) // LIMITS number of page views reported per user, comment this for full report
                                          .OrderBy(pv => pv.CreatedAt)
                                          .ToListAsync();
                
                var userObj = new JObject {
                    ["userSis"] = user.SisUserId, 
                    ["userFullName"] = user.Name,
                    ["isTeacher"] = await user.IsTeacher(),
                    ["pageViewsInReport"] = pageViews.Count
                };

                for (var i = 0; i < pageViews.Count; i++) {
                    var current = pageViews[i];
                    var next = i < pageViews.Count - 1 ? pageViews[i + 1] 
                                                       : null;
                    
                    pageViewsArr.Add(JObject.FromObject(new {
                        beginningOfInteraction = current.CreatedAt.ToIso8601Date(),
                        interactionSeconds = current.InteractionSeconds,
                        secondsUntilNextView = next?.CreatedAt.Subtract(current.CreatedAt).Seconds,
                        pageAction = current.Action, // the action being performed with/on the controller
                        pageController = current.Controller, // the thing being interacted with
                        pageContext = current.ContextType, // (optional) the kind of thing the controller belongs to
                        pageUrl = current.Url,
                        assetType = current.AssetType,
                        remoteIp = current.RemoteIp,
                        userParticipated = current.Participated
                    }));
                }

                pageViewsObj[user.Id.ToString()] = pageViewsArr;
                usersObj[user.Id.ToString()] = userObj;
                ++usersInReport;
            }

            document["usersInReport"] = usersInReport;

            _testOutputHelper.WriteLine(document.ToString(Formatting.Indented));
        }
    }
}
