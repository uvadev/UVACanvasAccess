using System;
using System.Threading.Tasks;
using dotenv.net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Util;
using Xunit;
using Xunit.Abstractions;
using static UVACanvasAccess.ApiParts.Api.CourseEnrollmentType;
using static UVACanvasAccess.ApiParts.Api.IndividualLevelCourseIncludes;

namespace UVACanvasAccessTests {
    
    public class GradeReporting {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly Api _api;

        public GradeReporting(ITestOutputHelper testOutputHelper) {
            _testOutputHelper = testOutputHelper;
            DotEnv.Config();
            _api = new Api(Environment.GetEnvironmentVariable("TEST_TOKEN") 
                           ?? throw new ArgumentException(".env should contain TEST_TOKEN"), 
                           "https://uview.instructure.com/api/v1/");
        }

        [Fact]
        public async Task Run() {
            var advisory = await _api.GetCourse(1028, includes: Everything);

            var enrollments = _api.StreamCourseEnrollments(advisory.Id, new[] {StudentEnrollment});

            var students = new JArray();

            await foreach (var enrollment in enrollments) {
                var student = await _api.GetUser(enrollment.UserId);
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

            _testOutputHelper.WriteLine(document.ToString(Formatting.Indented));
        }
    }
}
