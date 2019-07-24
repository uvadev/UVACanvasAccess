using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using dotenv.net;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Debugging;
using UVACanvasAccess.Util;
using static UVACanvasAccess.ApiParts.Api.IndividualLevelCourseIncludes;
using static UVACanvasAccess.ApiParts.Api.Order;
using static UVACanvasAccess.ApiParts.Api.PageSort;

namespace UVACanvasAccess {
    internal static class Test {
        private const ulong TestUser1Id = 3390,
                            TestUser2Id = 3392,
                            TestUser3Id = 3394,
                            TestUser4Id = 3431,
                            TestCourse = 1028,
                            TestDiscussion1 = 375,
                            TestDiscussion2 = 384,
                            TestAssignment1 = 9844,
                            TestAssignment2 = 10486,
                            TestAssignment2Override1 = 70,
                            TestAssignment2Override2 = 71,
                            TestAssignment1Override1 = 72,
                            TestingSubAccount = 154, 
                            TestDomainCourse = 1268;

        public static async Task Main(string[] args) {
            Debug.Listeners.Add(new TraceToStdErr());
            DotEnv.Config();
            
            var api = new Api(Environment.GetEnvironmentVariable("TEST_TOKEN"), 
                              "https://uview.instructure.com/api/v1/");


            for (int i = 0; i < 300; i++) {
                await api.CreateUser(TestingSubAccount.ToString())
                         .WithUniqueId($"TestDomainUser_{i}")
                         .WithName($"Test Domain User #{i}")
                         .WithSkipRegistration()
                         .WithSisUserId($"9888000{i}")
                         .Post()
                         .ThenPeek(async newUser => { 
                                       await api.CreateEnrollment(TestDomainCourse,
                                                                  newUser.Id,
                                                                  "StudentEnrollment",
                                                                  enrollmentState: "active");
                                   })
                         .ThenAccept(newUser => {
                                         Console.WriteLine($"Added and enrolled {newUser.Id}");
                                     });
            }
        }
    }
}