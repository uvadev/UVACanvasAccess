using System;
using System.Linq;
using System.Threading.Tasks;
using dotenv.net;

namespace UVACanvasAccess {
    internal static class Test {
        private const ulong Test1Id = 3390;
        private const ulong Test2Id = 3392;

        public static async Task Main(string[] args) {
            DotEnv.Config();
            
            var api = new Api(Environment.GetEnvironmentVariable("TEST_TOKEN"), 
                              "https://uview.instructure.com/api/v1/");

            var natalies = (await api.GetListUsers("Natalie")).ToList();
            Console.WriteLine("# of Natalies: " + natalies.ToList().Count);

            foreach (var natalie in natalies) {
                Console.WriteLine(natalie.Id);
            }
        }
    }
}