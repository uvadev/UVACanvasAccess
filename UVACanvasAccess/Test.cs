using System;
using System.Linq;
using System.Threading.Tasks;
using dotenv.net;

namespace UVACanvasAccess {
    internal static class Test {
        private const ulong TestUser1Id = 3390,
                            TestUser2Id = 3392,
                            TestUser3Id = 3394,
                            TestCourse = 1028,
                            TestDiscussion1 = 375,
                            TestDiscussion2 = 384;

        public static async Task Main(string[] args) {
            DotEnv.Config();
            
            var api = new Api(Environment.GetEnvironmentVariable("TEST_TOKEN"), 
                              "https://uview.instructure.com/api/v1/");

            api.MasqueradeAs(TestUser2Id);

            var activityStream = await api.GetActivityStream();

            Console.WriteLine(string.Join(",\n", from activity in activityStream 
                                                select $"{{ {activity.Type}: {activity.Title} }}"));
        }
    }
}