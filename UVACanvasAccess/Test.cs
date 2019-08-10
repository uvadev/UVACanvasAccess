using System;
using System.Threading.Tasks;
using dotenv.net;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Structures.Calendar;
using static UVACanvasAccess.Structures.Calendar.EventContextType;

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
            DotEnv.Config();
            var api = new Api(Environment.GetEnvironmentVariable("TEST_TOKEN") 
                              ?? ".env should have TEST_TOKEN",
                              "https://uview.instructure.com/api/v1/");

            var now = DateTime.Now;

            var newGroup = await api.CreateAppointmentGroup("Test Generated Appointment Group", 
                                                            new EventContext(Course, TestCourse))
                                    .WithDescription("Generated programatically.")
                                    .WithLocationName("UVA")
                                    .AddTimeSlot(now, now.AddHours(2))
                                    .AddTimeSlot(now.AddHours(2), now.AddHours(4))
                                    .AddTimeSlot(now.AddHours(4), now.AddHours(8))
                                    .WithProtectedVisibility()
                                    .Post();
            Console.WriteLine(newGroup.ToPrettyString());
        }
    }
}