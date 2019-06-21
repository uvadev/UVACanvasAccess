using System;
using System.Threading.Tasks;
using dotenv.net;
using Newtonsoft.Json.Linq;

namespace UVACanvasAccess {
    internal static class Test {
        private const ulong Test1Id = 3390;
        private const ulong Test2Id = 3392;

        public static async Task Main(string[] args) {
            DotEnv.Config();
            
            var api = new Api(Environment.GetEnvironmentVariable("TEST_TOKEN"), 
                              "https://uview.instructure.com/api/v1/");

            var _ = await api.StoreCustomJson("academy.uview", 
                                              "test",
                                              new JObject {["field1"] = 17, ["field2"] = "foo bar baz"},
                                              Test2Id);

            var storedJson = await api.LoadCustomJson("academy.uview",
                                                      "test",
                                                      Test2Id);

            Console.WriteLine(storedJson);
        }
    }
}