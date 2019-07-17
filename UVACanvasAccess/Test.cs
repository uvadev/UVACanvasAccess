using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using dotenv.net;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Debugging;
using UVACanvasAccess.Util;

namespace UVACanvasAccess {
    internal static class Test {
        private const ulong TestUser1Id = 3390,
                            TestUser2Id = 3392,
                            TestUser3Id = 3394,
                            TestCourse = 1028,
                            TestDiscussion1 = 375,
                            TestDiscussion2 = 384,
                            TestAssignment1 = 9844,
                            TestAssignment2 = 10486,
                            TestAssignment2Override1 = 70,
                            TestAssignment2Override2 = 71,
                            TestAssignment1Override1 = 72;

        public static async Task Main(string[] args) {
            Debug.Listeners.Add(new DontIgnoreAssertsTraceListener());
            DotEnv.Config();
            
            var api = new Api(Environment.GetEnvironmentVariable("TEST_TOKEN"), 
                              "https://uview.instructure.com/api/v1/");

            var names = await api.ListUsers()
                                 .ThenAccept(l => l.Select(u => u.Name));

            foreach (var name in names) {
                Console.WriteLine(name);
            }
            
            var streamedNames = api.StreamUsers()
                                   .Select(c => c.Name);

            await foreach (var name in streamedNames) {
                Console.WriteLine(name);
            }
        }
    }
}