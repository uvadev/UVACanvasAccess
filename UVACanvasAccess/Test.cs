using System;
using System.Globalization;
using System.Threading.Tasks;
using dotenv.net;
using UVACanvasAccess.Util;

namespace UVACanvasAccess {
    internal static class Test {
        private const ulong Test1Id = 3390;
        private const ulong Test2Id = 3392;

        public static async Task Main(string[] args) {
            DotEnv.Config();
            
            var api = new Api(Environment.GetEnvironmentVariable("TEST_TOKEN"), 
                              "https://uview.instructure.com/api/v1/");

            var user = await api.GetUserDetails(Test2Id);
            var profile = await user.GetProfile();
            var pageViews = await user.GetPageViews(new DateTime(2019, 6, 21, new GregorianCalendar()),
                                                    new DateTime(2019, 6, 22, new GregorianCalendar()));

            Console.WriteLine(profile.ToPrettyString());
            
            Console.WriteLine(pageViews.ToPrettyString());
            
        }
    }
}