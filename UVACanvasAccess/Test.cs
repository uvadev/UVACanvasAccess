using System;
using System.IO;
using System.Threading.Tasks;
using dotenv.net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UVACanvasAccess.ApiParts;
using static UVACanvasAccess.ApiParts.Api.AccountLevelCourseIncludes;
using static UVACanvasAccess.ApiParts.Api.CourseSearchBy;

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
                            TestDomainCourse = 1268,
                            TestAppointmentGroup = 65;

        public static async Task Main(string[] args) {
            DotEnv.Config();
            var api = new Api(Environment.GetEnvironmentVariable("TEST_TOKEN") 
                              ?? ".env should have TEST_TOKEN",
                              "https://uview.instructure.com/api/v1/");

            var document = new JObject();

            await foreach (var master in api.StreamCourses(searchTerm: "Master", searchBy: Course, includes: Term)) {
                var noSidSections = new JArray();
                var masterSections = new JObject {
                    ["noSid"] = noSidSections
                };
                
                var masterObj = new JObject {
                    ["name"] = master.Name,
                    ["canvasId"] = master.Id,
                    ["isBlueprint"] = master.Blueprint ?? false,
                    ["sections"] = masterSections,
                    ["term"] = new JObject {
                        ["name"] = master.Term?.Name,
                        ["id"] = master.Term?.Id
                    }
                };
                
                await foreach (var section in api.StreamCourseSections(master.Id)) {
                    var sectionObj = new JObject {
                        ["name"] = section.Name,
                        ["canvasId"] = section.Id
                    };

                    if (section.SisSectionId != null) {
                        masterSections[section.SisSectionId] = sectionObj;
                    } else {
                        noSidSections.Add(sectionObj);
                    }
                }
                document[master.Id.ToString()] = masterObj;
            }

            File.WriteAllText("SectionsAndTerms.json", document.ToString(Formatting.Indented));
        }
    }
}