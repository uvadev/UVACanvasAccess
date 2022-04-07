using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AppUtils;
using Newtonsoft.Json.Linq;
using Tomlyn.Syntax;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Util;
using static Newtonsoft.Json.Formatting;
using static UVACanvasAccess.ApiParts.Api.FileSort;
using static UVACanvasAccess.ApiParts.Api.Order;

namespace QuotaPurger {
    internal static class Program {

        private const string DeletionMessageTemplate = 
            "";

        public static async Task Main(string[] args) {
            
            var home = new AppHome("uva_quota_purger");

            Console.WriteLine($"Using config path: {home.ConfigPath}");

            if (!home.ConfigPresent()) {
                Console.WriteLine("Need to generate a config file.");
                
                home.CreateConfig(new DocumentSyntax {
                    Tables = {
                        new TableSyntax("tokens") {
                            Items = {
                                {"token", "PUT_TOKEN_HERE"}
                            }
                        },
                        new TableSyntax("options") {
                            Items = {
                                {"destroy", false},
                                {"send_message", false}
                            }
                        },
                        new TableSyntax("input") {
                            Items = {
                                {"input_file", "PUT_INPUT_FILE_HERE"}
                            }
                        }
                    }
                });

                Console.WriteLine("Created a new config file. Please go put in your token and input file.");
                return;
            }

            Console.WriteLine("Found config file.");

            var config = home.GetConfig();
            Debug.Assert(config != null, nameof(config) + " != null");
            
            var token = config.GetTable("tokens").Get<string>("token");
            
            var options = config.GetTable("options");
            var destroy = options.GetOr<bool>("destroy");
            var sendMessage = destroy && options.GetOr<bool>("send_message");
            var inputFilePath = config.GetTable("input").Get<string>("input_file");

            if (!File.Exists(inputFilePath)) {
                if (File.Exists(Path.Combine(home.NsDir, inputFilePath))) {
                    inputFilePath = Path.Combine(home.NsDir, inputFilePath);
                } else {
                    Console.WriteLine($"The input file path {inputFilePath} was not found.");
                    return;
                }
            }

            var input = JObject.Parse(File.ReadAllText(inputFilePath));
            var detectedUsers = input["detectedUsers"].ToObject<Dictionary<string, JObject>>()
                                                      .KeySelect(ulong.Parse)
                                                      .ValSelect(v => new {
                                                           userSis = v["userSis"],
                                                           userFullName = v["userFullName"]
                                                      });

            Console.WriteLine($"Loaded input file {inputFilePath} with {input["usersInLog"]} entries.");

            Console.WriteLine($"Being destructive? {(destroy ? "YES" : "NO")}");
            Console.WriteLine($"Sending message? {(sendMessage ? "YES" : "NO")}");

            // --------------------------------------------------------------------

            var api = new Api(token, "https://uview.instructure.com/api/v1/");

            var started = DateTime.Now;

            var resolvedUsers = new JObject();
            var actionTakenUsers = new JObject();
            var actionFailedUsers = new JObject();
            var inspectionFailedUsers = new JObject();
            var actionLimitedUsers = new JObject();
            
            var document = new JObject {
                ["resolvedUsers"] = resolvedUsers,
                ["actionTakenUsers"] = actionTakenUsers,
                ["actionLimitedUsers"] = actionLimitedUsers,
                ["actionFailedUsers"] = actionFailedUsers,
                ["inspectionFailedUsers"] = inspectionFailedUsers,
                ["dateStarted"] = started.ToIso8601Date(),
                ["sentMessages"] = sendMessage,
                ["deletedFiles"] = destroy
            };

            foreach (var (userId, userData) in detectedUsers) {
                try {
                    var isTeacher = await api.GetUser(userId)
                                             .ThenApplyAwait(async u => await u.IsTeacher());
                    
                    api.MasqueradeAs(userId);
                    var (quota, used) = await api.GetPersonalQuotaMiB();
                    api.StopMasquerading();

                    var overLimit = used / quota >= .85m;

                    if (!overLimit) {
                        resolvedUsers[userId.ToString()] = JToken.FromObject(new {
                            userData.userSis,
                            userData.userFullName,
                            usedMiB = used
                        });

                        Console.WriteLine($"User {userId} is now under the limit. No action taken.");
                        continue;
                    }

                    if (isTeacher) {
                        actionLimitedUsers[userId.ToString()] = JToken.FromObject(new {
                            userData.userSis,
                            userData.userFullName,
                            usedMiB = used
                        });

                        Console.WriteLine($"User {userId} is still over the limit, but is a teacher. No action taken.");
                        continue;
                    }
                    
                    if (!destroy) {
                        var mockDeletedFiles = new JObject();
                        
                        api.MasqueradeAs(userId);
                        
                        var conversationAttachments = await api.StreamPersonalFolders()
                                                               .FirstAsync(pf => pf.Name.ToLowerInvariant().Equals("conversation attachments"));

                        await foreach (var file in api.StreamPersonalFiles(sortBy: Size, order: Descending)
                                                      .Where(f => f.FolderId == conversationAttachments.Id)) {
                            Console.WriteLine($"[MOCK] Would delete file {file.Id} for user {userId}.");
                            mockDeletedFiles[file.Id.ToString()] = JToken.FromObject(new {
                                fileName = file.Filename,
                                displayName = file.DisplayName,
                                fileSize = file.Size,
                                url = file.Url,
                                contentType = file.ContentType,
                                createdAt = file.CreatedAt,
                                modifiedAt = file.ModifiedAt,
                                updatedAt = file.UpdatedAt
                            });
                        }
                        
                        api.StopMasquerading();
                        
                        actionTakenUsers[userId.ToString()] = JToken.FromObject(new {
                            userData.userSis,
                            userData.userFullName,
                            filesWereDeleted = false,
                            mockDeletedFiles
                        });
                        Console.WriteLine($"User {userId} was over the limit, and his files would have been purged, but destructive mode is off.");
                        continue;
                    }

                    try {
                        var deletedFiles = new JObject();
                        
                        api.MasqueradeAs(userId);
                        
                        var conversationAttachments = await api.StreamPersonalFolders()
                                                               .FirstAsync(pf => pf.Name.ToLowerInvariant().Equals("conversation attachments"));


                        await foreach (var file in api.StreamPersonalFiles(sortBy: Size, order: Descending)
                                                      .Where(f => f.FolderId == conversationAttachments.Id)) {
                            await api.DeleteFile(file.Id);
                            Console.WriteLine($"Deleted file {file.Id} for user {userId}.");
                            deletedFiles[file.Id.ToString()] = JToken.FromObject(new {
                                fileName = file.Filename,
                                displayName = file.DisplayName,
                                fileSize = file.Size,
                                url = file.Url,
                                contentType = file.ContentType,
                                createdAt = file.CreatedAt,
                                modifiedAt = file.ModifiedAt,
                                updatedAt = file.UpdatedAt
                            });
                        }
                        
                        api.StopMasquerading();
                        
                        actionTakenUsers[userId.ToString()] = JToken.FromObject(new {
                            userData.userSis,
                            userData.userFullName,
                            filesWereDeleted = true,
                            deletedFiles
                        });

                        Console.WriteLine($"User {userId} was over the limit, and his files were purged.");

                        if (sendMessage) {
                            // todo
                        }
                    } catch (Exception e) {
                        Console.WriteLine($"Exception when taking action for user {userId}.\n{e}\nContinuing as normal ------");
                        actionFailedUsers[userId.ToString()] = JToken.FromObject(new {
                            userData.userSis,
                            userData.userFullName,
                            usedMiB = used
                        });
                    } finally {
                        api.StopMasquerading();
                    }
                } catch (Exception e) {
                    Console.WriteLine($"Exception when inspecting user {userId}.\n{e}\nContinuing as normal ------");
                    inspectionFailedUsers[userId.ToString()] = JToken.FromObject(userData);
                } finally {
                    api.StopMasquerading();
                }
            }
            
            document["dateCompleted"] = DateTime.Now.ToIso8601Date();
            document["usersInLog"] = detectedUsers.Count;
                
            var outPath = Path.Combine(home.NsDir, $"QuotaPurger_Log_{started.Ticks}.json");
            File.WriteAllText(outPath, document.ToString(Indented) + "\n");
            Console.WriteLine($"Wrote log to {outPath}");
        }
    }
}
