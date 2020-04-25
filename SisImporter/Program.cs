using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using AppUtils;
using Tomlyn.Syntax;
using UVACanvasAccess.ApiParts;

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

            try {
                var imp = await api.ImportSisData(file, Path.GetFileName(inputFilePath));
                Console.WriteLine($"Import with ID {imp.Id} was successfully created.");
            } catch (Exception e) {
                Console.WriteLine($"ImportSisData call threw up:\n{e}");
            }
        }
    }
}
