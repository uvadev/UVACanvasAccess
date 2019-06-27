using System;
using System.Threading.Tasks;
using dotenv.net;
using UVACanvasAccess.Util;
using static UVACanvasAccess.Api.DiscussionTopicInclusions;

namespace UVACanvasAccess {
    internal static class Test {
        private const ulong TestUser1 = 3390,
                            TestUser2Id = 3392,
                            TestCourse = 1028,
                            TestDiscussion = 375;

        public static async Task Main(string[] args) {
            DotEnv.Config();
            
            var api = new Api(Environment.GetEnvironmentVariable("TEST_TOKEN"), 
                              "https://uview.instructure.com/api/v1/");

            var myTopic = await api.GetCourseDiscussionTopic(TestCourse, TestDiscussion, Everything);
            Console.WriteLine("My topic: " + myTopic.ToPrettyString());

            var entries = await myTopic.GetEntries();
            Console.WriteLine("Entries therein: " + entries.ToPrettyString());
        }
    }
}