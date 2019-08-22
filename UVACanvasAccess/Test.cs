using System;
using System.Linq;
using System.Threading.Tasks;
using dotenv.net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UVACanvasAccess.ApiParts;

namespace UVACanvasAccess {
    internal static class Test {
        private const ulong TestUser1Id = 3390,
                            TestUser2Id = 3392,
                            TestUser3Id = 3394,
                            TestUser4Id = 3431,
                            TestCourse = 1028,
                            TestDiscussion1 = 375,
                            TestDiscussion2 = 384,
                            TestAssignment1 = 9844,
                            TestAssignment2 = 10486,
                            TestAssignment2Override1 = 70,
                            TestAssignment2Override2 = 71,
                            TestAssignment1Override1 = 72,
                            TestingSubAccount = 154, 
                            TestDomainCourse = 1268,
                            TestAppointmentGroup = 65;

        public static async Task Main(string[] args) {
            DotEnv.Config();
            var api = new Api(Environment.GetEnvironmentVariable("TEST_TOKEN") 
                              ?? ".env should have TEST_TOKEN",
                              "https://uview.instructure.com/api/v1/");
            
            var users = new JObject();
            var document = new JObject {
                ["users"] = users
            };

            uint count = 0;
            
            await foreach (var user in api.StreamUsers().Take(15)) {
                try {
                    var o = new JObject {
                        ["sis"] = user.SisUserId
                    };
                    api.MasqueradeAs(user.Id);
                    
                    var (quota, used) = await api.GetPersonalQuotaMiB();
                    o["quotaMiB"] = Math.Round(quota, 5);
                    o["usedMiB"] = Math.Round(used, 5);
                    o["freeMiB"] = Math.Round(quota - used, 5);

                    users[user.Id.ToString()] = o;
                    ++count;
                } catch (Exception e) {
                    Console.WriteLine(e);
                } finally {
                    api.StopMasquerading();
                }
            }

            document["usersInReport"] = count;

            Console.WriteLine(document.ToString(Formatting.Indented));
        }
    }
}