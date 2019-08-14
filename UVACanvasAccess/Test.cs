using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using dotenv.net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Util;
using static UVACanvasAccess.ApiParts.Api.CourseSearchBy;
using static UVACanvasAccess.ApiParts.Api.SectionIncludes;

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

            var noSisCourses = new JArray();
            var document = new JObject {
                ["noSis"] = noSisCourses
            };

            await foreach (var master in api.StreamCourses(searchTerm: "Master", searchBy: Course)) {
                var noSidSections = new JArray();
                var masterObj = new JObject {
                    ["name"] = master.Name,
                    ["canvasId"] = master.Id,
                    ["isBlueprint"] = master.Blueprint ?? false,
                    ["noSid"] = noSidSections
                };


                await foreach (var section in api.StreamCourseSections(master.Id, Everything)) {
                    var sectionObj = new JObject {
                        ["name"] = section.Name ?? "",
                        ["canvasId"] = section.Id
                    };

                    if (section.SisSectionId != null) {
                        masterObj[section.SisSectionId] = sectionObj;
                    } else {
                        noSidSections.Add(sectionObj);
                    }
                }

                if (master.SisCourseId != null) {
                    document[master.SisCourseId] = masterObj;
                } else {
                    noSisCourses.Add(masterObj);
                }
                
            }

            File.WriteAllText("SectionsAndTerms.json", document.ToString(Formatting.Indented));
        }
    }
}