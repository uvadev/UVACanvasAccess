using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Tomlyn;
using Tomlyn.Model;
using Tomlyn.Syntax;
using static System.Environment;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Util;
using static Newtonsoft.Json.Formatting;

namespace SuperReport {
    internal static class Program {
    
        public static async Task Main(string[] args) {
            var shareDir = GetFolderPath(SpecialFolder.LocalApplicationData, 
                                          SpecialFolderOption.Create);
            var reportDir = Path.Combine(shareDir, "uva_super_report");

            if (!Directory.Exists(reportDir)) {
                Directory.CreateDirectory(reportDir);
            }

            var configPath = Path.Combine(reportDir, "config.toml");

            Console.WriteLine($"Using config path: {configPath}");

            if (!File.Exists(configPath)) {
                Console.WriteLine($"No config file found at path {configPath}. Trying to create it.");

                var doc = new DocumentSyntax {
                    Tables = {
                        new TableSyntax("tokens") {
                            Items = {
                                {"token", "PUT_TOKEN_HERE"}
                            }
                        },
                        new TableSyntax("limits") {
                            Items = {
                                {"sample_skip", 0},
                                {"sample_take", 10}
                            }
                        }
                    }
                };
                
                File.WriteAllText(configPath, doc.ToString());

                Console.WriteLine("Wrote bare config file. Please go put in your token.");
                return;
            }

            Console.WriteLine("Found config file.");

            var configDoc = Toml.Parse(File.ReadAllText(configPath));
            if (configDoc.HasErrors) {
                Console.WriteLine("The config file had errors in it.");

                foreach (var diag in configDoc.Diagnostics) {
                    Console.WriteLine(diag.ToString());
                }
                
                return;
            }

            var config = configDoc.ToModel();
            var token = (string) ((TomlTable) config["tokens"])["token"];
            var sampleTake = (int)(long) ((TomlTable) config["limits"])["sample_take"];
            var sampleSkip = (int)(long) ((TomlTable) config["limits"])["sample_skip"];

            Console.WriteLine($"SKIPPING {sampleSkip} users.");
            Console.WriteLine($"TAKING {sampleTake} users.");
            
            // --------------------------------------------------------------------

            var api = new Api(token, "https://uview.instructure.com/api/v1/");
            
            var studentsObj = new JObject();
            var teachersObj = new JObject();
            var coursesObj = new JObject();
            var assignmentsOverallObj = new JObject();
            var assignmentsIndividualObj = new JObject();

            var started = DateTime.Now;
            
            var document = new JObject {
                ["teachers"] = teachersObj,
                ["students"] = studentsObj,
                ["courses"] = coursesObj,
                ["assignmentsOverall"] = assignmentsOverallObj,
                ["assignmentsIndividual"] = assignmentsIndividualObj,
                ["dateStarted"] = started.ToIso8601Date()
            };

            var sample = api.StreamUsers()
                            .Where(u => !u.Name.ToLowerInvariant().Contains("test"))
                            .Where(u => !u.SisUserId?.StartsWith("pG") ?? false)
                            .Skip(sampleSkip)
                            .Take(sampleTake);

            await foreach (var user in sample) {
                if (await user.IsTeacher()) {
                    if (!teachersObj.ContainsKey(user.Id.ToString())) {
                        teachersObj[user.Id.ToString()] = new JObject {
                            ["sisId"] = user.SisUserId,
                            ["fullName"] = user.Name
                        };
                    } 
                } else {
                    if (!studentsObj.ContainsKey(user.Id.ToString())) {
                        studentsObj[user.Id.ToString()] = new JObject {
                            ["sisId"] = user.SisUserId,
                            ["fullName"] = user.Name
                        };
                    }
                }
            }

            document["dateCompleted"] = DateTime.Now.ToIso8601Date();
            document["usersInReport"] = studentsObj.Count + teachersObj.Count;

            var outPath = Path.Combine(reportDir, $"SuperReport_{started.Ticks}.json");
            File.WriteAllText(outPath, document.ToString(Indented));
            Console.WriteLine($"Wrote report to {outPath}");
        }
    }
}
