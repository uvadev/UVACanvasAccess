using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dotenv.net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Debugging;
using UVACanvasAccess.Structures.Courses;
using UVACanvasAccess.Util;
using static UVACanvasAccess.ApiParts.Api.CourseEnrollmentType;
using static UVACanvasAccess.ApiParts.Api.IndividualLevelCourseIncludes;

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

            var advisory = await api.GetCourse(TestCourse, includes: Everything);

            var enrollments = api.StreamCourseEnrollments(advisory.Id, new[] {StudentEnrollment});

            var students = new JArray();

            await foreach (var enrollment in enrollments) {
                var student = await api.GetUserDetails(enrollment.UserId);
                students.Add(JObject.FromObject(new {
                    student = new {
                        id = student.Id,
                        sis = student.SisUserId,
                        name = student.Name
                    },
                    score = new {
                        current = enrollment.Grades.CurrentScore,
                        final = enrollment.Grades.FinalScore
                    },
                    grade = new {
                        current = enrollment.Grades.CurrentGrade,
                        final = enrollment.Grades.FinalGrade
                    }
                }));
            }

            var document = new JObject {
                ["generatedOn"] = DateTime.Now.ToUniversalTime().ToIso8601Date(), 
                ["students"] = students
            };

            Console.WriteLine(document.ToString(Formatting.Indented));
        }
    }
}