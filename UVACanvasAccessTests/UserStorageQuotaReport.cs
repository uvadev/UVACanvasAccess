using System;
using System.Linq;
using System.Threading.Tasks;
using dotenv.net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UVACanvasAccess.ApiParts;
using Xunit;
using Xunit.Abstractions;

namespace UVACanvasAccessTests {
    public class UserStorageQuotaReport {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly Api _api;
        
        public UserStorageQuotaReport(ITestOutputHelper testOutputHelper) {
            _testOutputHelper = testOutputHelper;
            DotEnv.Config();
            _api =  new Api(Environment.GetEnvironmentVariable("TEST_TOKEN"), 
                            "https://uview.instructure.com/api/v1/");
        }

        [Fact]
        public async Task Run() {
            var users = new JObject();
            var document = new JObject {
                ["users"] = users
            };

            uint count = 0;
            
            await foreach (var user in _api.StreamUsers().Take(15)) {
                try {
                    var o = new JObject {
                        ["sis"] = user.SisUserId
                    };
                    _api.MasqueradeAs(user.Id);
                    
                    var (quota, used) = await _api.GetPersonalQuotaMiB();
                    o["quotaMiB"] = Math.Round(quota, 5);
                    o["usedMiB"] = Math.Round(used, 5);
                    o["freeMiB"] = Math.Round(quota - used, 5);

                    users[user.Id.ToString()] = o;
                    ++count;
                } catch (Exception e) {
                    _testOutputHelper.WriteLine(e.ToString());
                } finally {
                    _api.StopMasquerading();
                }
            }

            document["usersInReport"] = count;

            _testOutputHelper.WriteLine(document.ToString(Formatting.Indented));
        }
    }
}
