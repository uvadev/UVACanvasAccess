using System;
using System.Threading.Tasks;
using dotenv.net;

namespace UVACanvasAccess {
    internal static class Test {
        private const ulong Test1Id = 3390;

        public static async Task Main(string[] args) {
            DotEnv.Config();
            
            var api = new Api(Environment.GetEnvironmentVariable("TEST_TOKEN"), 
                              "https://uview.instructure.com/api/v1/");

            var currentProfile = await api.GetUserProfile(Test1Id);

            currentProfile.Name = "UVACanvasAccess Test Account 1";
            currentProfile.ShortName = "CanvasAccess Test 1";
            currentProfile.SortableName = "Test1, CanvasAccess";

            Console.WriteLine(currentProfile);
        }
    }
}