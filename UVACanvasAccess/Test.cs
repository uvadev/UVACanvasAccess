using System;
using System.Threading.Tasks;
using dotenv.net;

namespace UVACanvasAccess {
    internal static class Test {
        public static async Task Main(string[] args) {
            DotEnv.Config();
            
            var api = new Api(Environment.GetEnvironmentVariable("TEST_TOKEN"), 
                              "https://uview.instructure.com/api/v1/");
            
            var natalies = await api.GetListUsers("Natalie", "username", "desc");
            Console.WriteLine("List of Natalies ({0} natalies):", natalies.Count);
            foreach (var natalie in natalies) {
                Console.WriteLine(natalie + "\n========\n");
            }
        }
    }
}