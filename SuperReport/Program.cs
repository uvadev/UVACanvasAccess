using System;
using System.IO;
using Tomlyn;
using Tomlyn.Model;
using Tomlyn.Syntax;
using static System.Environment;
using UVACanvasAccess.ApiParts;

namespace SuperReport {
    internal static class Program {
    
        public static void Main(string[] args) {
            var shareDir = GetFolderPath(SpecialFolder.LocalApplicationData, 
                                          SpecialFolderOption.Create);
            var configPath = Path.Combine(shareDir, "uva_super_report");

            if (!Directory.Exists(configPath)) {
                Directory.CreateDirectory(configPath);
            }

            configPath = Path.Combine(configPath, "config.toml");

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
            var sampleTake = (long) ((TomlTable) config["limits"])["sample_take"];
            var sampleSkip = (long) ((TomlTable) config["limits"])["sample_skip"];
            
            // --------------------------------------------------------------------

            var api = new Api(token, "https://uview.instructure.com/api/v1/");
            
            
        }
    }
}
