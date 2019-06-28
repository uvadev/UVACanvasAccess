using System;
using System.Linq;
using System.Threading.Tasks;
using dotenv.net;
using UVACanvasAccess.Util;
using static UVACanvasAccess.Api.DiscussionTopicInclusions;

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

            var topic = await api.GetCourseDiscussionTopic(TestCourse, TestDiscussion1, Everything);

            var oldestEntry = (await topic.GetEntries()).Last();

            var replies = await oldestEntry.GetReplies();

            Console.WriteLine(replies.ToPrettyString());
        }
    }
}