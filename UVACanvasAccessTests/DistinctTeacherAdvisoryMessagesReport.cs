using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotenv.net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Structures;
using UVACanvasAccess.Structures.Users;
using UVACanvasAccess.Util;
using Xunit;
using Xunit.Abstractions;
using static UVACanvasAccess.ApiParts.Api.CourseEnrollmentType;
using static UVACanvasAccess.Structures.ContextType;

namespace UVACanvasAccessTests {
    public class DistinctTeacherAdvisoryMessagesReport {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly Api _api;
        
        public DistinctTeacherAdvisoryMessagesReport(ITestOutputHelper testOutputHelper) {
            _testOutputHelper = testOutputHelper;
            DotEnv.Config();
            _api = new Api(Environment.GetEnvironmentVariable("TEST_TOKEN") 
                           ?? throw new ArgumentException(".env should contain TEST_TOKEN"), 
                           "https://uview.instructure.com/api/v1/");
        }

        [Fact]
        // Note: make sure to comment out the LIMIT lines below to get a full report
        public async Task Run() {
            IAsyncEnumerable<User> sample = _api.StreamUsers()
                                                .WhereAwait(u => u.IsTeacher())
                                                .Where(u => !u.Name.ToLowerInvariant().Contains("test"))
                                                .Where(u => !u.SisUserId?.StartsWith("pG") ?? false)
                                                .Take(15); // LIMITS number of teachers reported, comment this for full report
            
            uint teachersInReport = 0;
            var teachersObj = new JObject();
            
            var document = new JObject {
                ["dateGenerated"] = DateTime.Now.ToIso8601Date(),
                ["teachers"] = teachersObj
            };

            await foreach (var teacher in sample) {
                var advisoryCourse = await _api.StreamUserEnrollments(teacher.Id, TeacherEnrollment.Yield())
                                               .Select(e => _api.GetCourse(e.CourseId).Result)
                                               .Where(c => c.WorkflowState == "available")
                                               .Where(c => c.Name.ToLowerInvariant().Contains("advisory"))
                                               .FirstOrDefaultAsync();
                
                if (advisoryCourse == default) {
                    continue;
                }

                var teacherObj = new JObject {
                    ["teacherSisId"] = teacher.SisUserId,
                    ["teacherName"] = teacher.Name,
                    ["advisoryCourseId"] = advisoryCourse.Id
                };
                
                try {
                    _api.MasqueradeAs(teacher.Id);

                    var conversations = await _api.StreamConversations(filter: new QualifiedId(advisoryCourse.Id, Course).Yield())
                                                  .Where(c => c.ContextName == advisoryCourse.Name)
                                                  .Distinct(c => c.Audience)
                                                  .CountAsync();
                    
                    teacherObj["distinctAdvisoryConversations"] = conversations;
                } catch (Exception e) {
                    _testOutputHelper.WriteLine(e.Message);
                    continue;
                }
                finally {
                    _api.StopMasquerading();
                }

                teachersObj[teacher.Id.ToString()] = teacherObj;
                ++teachersInReport;
            }

            document["teachersInReport"] = teachersInReport;
            _testOutputHelper.WriteLine(document.ToString(Formatting.Indented));
        }
    }
}
