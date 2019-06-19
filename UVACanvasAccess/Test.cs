using System;
using System.Threading.Tasks;
using dotenv.net;

namespace UVACanvasAccess {
    internal static class Test {
        public static async Task Main(string[] args) {
            DotEnv.Config();
            
            var api = new Api(Environment.GetEnvironmentVariable("TEST_TOKEN"), 
                              "https://uview.instructure.com/api/v1/");

            var currentProfile = await api.GetUserProfile();

            currentProfile.Bio = "This is a test bio.";                   // These setters call the API and update
            currentProfile.Title = "Professional UVACanvasAccess Tester"; // the user.

            Console.WriteLine(currentProfile);
        }
    }
}