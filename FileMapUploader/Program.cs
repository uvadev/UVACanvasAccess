using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppUtils;
using Tomlyn.Syntax;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Structures;
using UVACanvasAccess.Util;

namespace FileMapUploader {
    
    internal static class Program {
        
        public static async Task Main(string[] args) {
            var home = new AppHome("file_map_uploader");
            
            if (!home.ConfigPresent()) {
                Console.WriteLine("Need to generate a config file.");
                
                home.CreateConfig( new DocumentSyntax {
                    Tables = {
                        new TableSyntax("tokens") {
                            Items = {
                                {"token", "PUT_TOKEN_HERE"}
                            }
                        },
                        new TableSyntax("data") {
                            Items = {
                                {"map_file", "RELATIVE_MAP_FILE_PATH_HERE"},
                                {"target_canvas_dir", "uploaded"},
                                {"send_message", true},
                                {"message", "An important file was pushed to your account."}
                            }
                        }
                    }
                });

                Console.WriteLine("Created a new config file. Please go put in your token and map info.");
                return;
            }
            
            Console.WriteLine("Found config file.");

            var config = home.GetConfig();
            Debug.Assert(config != null, nameof(config) + " != null");
            
            var token = config.GetTable("tokens").Get<string>("token");

            var data = config.GetTable("data");
            var mapFileName = data.Get<string>("map_file");
            var targetFolder = data.Get<string>("target_canvas_dir");
            var sendMessage = data.Get<bool>("send_message");
            var message = data.Get<string>("message");

            var mapFilePath = Path.Combine(home.NsDir, mapFileName);

            Console.WriteLine($"Sourcing map from {mapFilePath}");
            
            // ------------------------------------------------------------------------

            var list = File.ReadAllLines(mapFilePath).ToList();
            
            List<string>[] taskLists = list.Chunk(Math.Min(Math.Max(list.Count / 7, 2), list.Count - 1))
                                           .ToArray();
            
            var nThreads = taskLists.Length;
            
            var apis = new Api[nThreads];
            
            for (int i = 0; i < nThreads; i++) {
                apis[i] = new Api(token, "https://uview.instructure.com/api/v1/");
            }

            Console.WriteLine($"Using {nThreads} threads.");
            
            var completed = new ConcurrentBag<string>();
            var keys = new ConcurrentBag<string>();
            
            using (var countdown = new CountdownEvent(nThreads)) {
                for (int i = 0; i < nThreads; i++) {
                    ThreadPool.QueueUserWorkItem(o => {
                        try {
                            var n = (int) o;
                            foreach (var line in taskLists[n]) {
                                string[] halves = line.Split(',');
                                Debug.Assert(halves.Length == 2);

                                var (userKey, userFile) = (halves[0], halves[1]);
                                keys.Add(userKey);

                                var api = apis[n];

                                try {
                                    var user = api.StreamUsers(userKey)
                                                  .FirstOrDefaultAsync(u => u.SisUserId == userKey)
                                                  .Result;

                                    if (user == null) {
                                        Console.WriteLine($"WARN: Couldn't find the user for sis {userKey} !!");
                                        continue;
                                    }

                                    var bytes = File.ReadAllBytes(Path.Combine(Path.GetDirectoryName(mapFilePath),
                                                                               userFile));

                                    Console.WriteLine($"Preparing to upload filename {userFile} to user " +
                                                      $"{userKey}, Id {user.Id}, SIS {user.SisUserId}");

                                    api.MasqueradeAs(user.Id);

                                    var file = api.UploadPersonalFile(bytes,
                                                                      userFile,
                                                                      targetFolder)
                                                  .Result;

                                    Console.WriteLine($"Uploaded as {file.Id}!");
                                    completed.Add(userKey);

                                    if (sendMessage) {
                                        api.StopMasquerading();
                                        api.CreateConversation(new QualifiedId(user.Id).Yield(), 
                                                               message,
                                                               forceNew: true);
                                    }

                                } catch (Exception e) {
                                    Console.WriteLine($"Caught an exception during upload for {userKey}: {e}");
                                } finally {
                                    api.StopMasquerading();
                                }
                            }
                        } finally {
                            countdown.Signal();
                        }
                    }, i);
                }
                countdown.Wait();
            }

            Console.WriteLine($"{completed.Distinct().Count()} out of {list.Count} operations were completed.");

            var exc = keys.Except(completed).ToList();
            if (exc.Any()) {
                Console.WriteLine($"Operation failed for the following SIS IDs: {exc.ToPrettyString()}");
            }
        }
    }
}
