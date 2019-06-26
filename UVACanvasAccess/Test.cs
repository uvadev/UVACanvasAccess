using System;
using System.Threading.Tasks;
using dotenv.net;
using UVACanvasAccess.Util;
using static UVACanvasAccess.Api.DiscussionTopicInclusions;
using static UVACanvasAccess.Api.DiscussionTopicScopes;

namespace UVACanvasAccess {
    internal static class Test {
        private const ulong TestUser1 = 3390,
                            TestUser2Id = 3392,
                            TestCourse = 1028;

        public static async Task Main(string[] args) {
            DotEnv.Config();
            
            var api = new Api(Environment.GetEnvironmentVariable("TEST_TOKEN"), 
                              "https://uview.instructure.com/api/v1/");

            var discussion = await api.ListCourseDiscussionTopics(TestCourse, 
                                                                  scopes: Locked | Unpinned, 
                                                                  includes: Everything);

            Console.WriteLine(discussion.ToPrettyString());
        }
    }
}