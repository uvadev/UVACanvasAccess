using System;
using System.IO;
using System.Threading.Tasks;
using dotenv.net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UVACanvasAccess.ApiParts;
using Xunit;
using static UVACanvasAccess.ApiParts.Api.AccountLevelCourseIncludes;
using static UVACanvasAccess.ApiParts.Api.CourseSearchBy;

namespace UVACanvasAccessTests {
    public class MasterCourseSectionsReport {
        private readonly Api _api;
        
        public MasterCourseSectionsReport() {
            DotEnv.Config();
            _api = new Api(Environment.GetEnvironmentVariable("TEST_TOKEN") 
                           ?? throw new ArgumentException(".env should contain TEST_TOKEN"), 
                           "https://uview.instructure.com/api/v1/");
        }

        [Fact]
        public async Task Run() {
            var document = new JObject();

            await foreach (var master in _api.StreamCourses(searchTerm: "Master", searchBy: Course, includes: Term)) {
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
                
                await foreach (var section in _api.StreamCourseSections(master.Id)) {
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
