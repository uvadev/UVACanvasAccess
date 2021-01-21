using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using AppUtils;
using Newtonsoft.Json.Linq;
using Tomlyn.Syntax;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Util;
using static Newtonsoft.Json.Formatting;
using static UVACanvasAccess.Structures.SisImports.SisImportState;

namespace SisImporter {
    
    internal static class Program {

        public static async Task Main(string[] args) {
            var home = new AppHome("sis_importer");
            
            Console.WriteLine($"Using config path: {home.ConfigPath}");
            
            if (!home.ConfigPresent()) {
                Console.WriteLine("Need to generate a config file.");
                
                home.CreateConfig( new DocumentSyntax {
                    Tables = {
                        new TableSyntax("tokens") {
                            Items = {
                                {"token", "PUT_TOKEN_HERE"}
                            }
                        },
                        new TableSyntax("input") {
                            Items = {
                                {"import_file", "RELATIVE_IMPORT_FILE_PATH_HERE"}
                            }
                        }
                    }
                });

                Console.WriteLine("Created a new config file. Please go put in your token and input path.");
                return;
            }
            
            Console.WriteLine("Found config file.");

            var config = home.GetConfig();
            Debug.Assert(config != null, nameof(config) + " != null");
            
            var token = config.GetTable("tokens")
                              .Get<string>("token");
            
            var inputFilePath = config.GetTable("input")
                                      .Get<string>("import_file");

            if (!File.Exists(inputFilePath)) {
                if (File.Exists(Path.Combine(home.NsDir, inputFilePath))) {
                    inputFilePath = Path.Combine(home.NsDir, inputFilePath);
                } else {
                    Console.WriteLine($"The import file path {inputFilePath} was not found.");
                    return;
                }
            }

            byte[] file = File.ReadAllBytes(inputFilePath);

            Console.WriteLine($"Successfully read import file at {inputFilePath}.");
            
            var api = new Api(token, "https://uview.instructure.com/api/v1/");

            var started = DateTime.Now;
            
            try {
                var imp = await api.ImportSisData(file, Path.GetFileName(inputFilePath));
                Console.WriteLine($"Import with ID {imp.Id} was submitted. Waiting until completion...");

                do {
                    await Task.Delay(500); // check the import status every N
                    imp = await api.GetSisImport(imp.Id);
                } while (!imp.WorkflowState.IsHaltedState());

                Console.WriteLine($"Import finished with status: {imp.WorkflowState}");
                var completed = DateTime.Now;
                
                switch (imp.WorkflowState) {
                    case Imported:
                    case ImportedWithMessages: 
                    case FailedWithMessages:
                    case Failed: {
                        var document = new JObject {
                            ["wasSuccessful"] = imp.WorkflowState == Imported || 
                                                imp.WorkflowState == ImportedWithMessages,
                            ["status"] = imp.WorkflowState.ToString(),
                            ["importStarted"] = started.ToIso8601Date(),
                            ["importCompleted"] = completed.ToIso8601Date(),
                            ["counts"] = JToken.FromObject(imp.Data.Counts ?? new object()),
                            ["errors"] = JToken.FromObject(imp.ProcessingErrors ?? new List<IEnumerable<string>>()),
                            ["warnings"] = JToken.FromObject(imp.ProcessingWarnings ?? new List<IEnumerable<string>>())
                        };

                        var outPath = Path.Combine(home.NsDir, $"SisImport_{started.Year}-{started.Month}-{started.Day}.json");
                        File.WriteAllText(outPath, document.ToString(Indented) + "\n");
                        Console.WriteLine($"Wrote log to {outPath}");
                        
                        return;
                    }
                    case PartiallyRestored:
                    case Restored:
                    case Aborted:
                        return;
                    case Initializing:
                    case Created:
                    case Importing:
                    case CleanupBatch:
                    case Restoring:
                    case Invalid:
                        Debug.Assert(false);
                        return;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            } catch (Exception e) {
                Console.WriteLine($"ImportSisData call threw up:\n{e}");
            }
        }
    }
}
