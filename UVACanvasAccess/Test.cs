using System;
using System.Globalization;
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

            var test2PageViews = await api.GetUserPageViews(Test2Id, 
                                                            new DateTime(2019, 6, 21, new GregorianCalendar()),
                                                            new DateTime(2019, 6, 22, new GregorianCalendar()));
            
            foreach (var view in test2PageViews) {
                Console.WriteLine(view.ToPrettyString());
            }
        }
    }
}