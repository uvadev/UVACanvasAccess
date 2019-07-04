using System;
using System.Threading.Tasks;
using dotenv.net;
using UVACanvasAccess.ApiParts;

namespace UVACanvasAccess {
    internal static class Test {
        private const ulong TestUser1Id = 3390,
                            TestUser2Id = 3392,
                            TestUser3Id = 3394,
                            TestCourse = 1028,
                            TestDiscussion1 = 375,
                            TestDiscussion2 = 384,
                            TestAssignment1 = 9844,
                            TestAssignment2 = 10486,
                            TestAssignment2Override1 = 70;

        public static async Task Main(string[] args) {
            DotEnv.Config();
            
            var api = new Api(Environment.GetEnvironmentVariable("TEST_TOKEN"), 
                              "https://uview.instructure.com/api/v1/");

            var o = await api.CreateAssignmentOverride(TestCourse, TestAssignment2)
                             .WithStudents(TestUser3Id)
                             .WithTitle("Cool TestUser3 Override")
                             .WithDueDate(DateTime.Now)
                             .Post();

            Console.WriteLine(o.ToPrettyString());
        }
    }
}