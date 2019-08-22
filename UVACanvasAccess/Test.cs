using System;
using System.Threading.Tasks;
using dotenv.net;
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
            
            var (quota, used) = await api.GetPersonalQuotaMiB();
            Console.WriteLine($"Used about {Math.Round(used, 2)}/{Math.Round(quota, 2)} MiB ({Math.Round(quota - used, 2)} MiB free)");
        }
    }
}