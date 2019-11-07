using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using AppUtils;
using Newtonsoft.Json.Linq;
using Tomlyn.Syntax;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Structures;
using UVACanvasAccess.Util;
using static Newtonsoft.Json.Formatting;

namespace QuotaWatcher {
    internal static class Program {

        private const string WarningMessageTemplate = 
            "Hi {0},\n\n" +
            "We noticed that your personal file storage folder is {1}% full ({2} mb used).\n" +
            "This might cause problems in the future if important files ever need to be automatically uploaded to your account.\n\n" +
            "Please make some more room in your personal folder by deleting some files you don't need. You can do that here: https://uview.instructure.com/files. Make sure you check every subfolder under \"My Files\".\n\n" +
            "If you don't do this, some of your files may get deleted automatically in the future. If that happens, you won't be able to recover those files.\n\n" +
            "Thanks.\n\n" +
            "[This message was generated automatically.]";
        
        public static async Task Main(string[] args) {

            var home = new AppHome("uva_quota_watcher");

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
                                {"send_message", false}
                            }
                        }
                    }
                });

                Console.WriteLine("Created a new config file. Please go put in your token.");
                return;
            }

            Console.WriteLine("Found config file.");

            var config = home.GetConfig();
            Debug.Assert(config != null, nameof(config) + " != null");
            
            var token = config.GetTable("tokens").Get<string>("token");

            var options = config.GetTable("options");
            var sendMessage = options.GetOr<bool>("send_message");

            Console.WriteLine($"Sending message? {(sendMessage ? "YES" : "NO")}");

            // --------------------------------------------------------------------

            var api = new Api(token, "https://uview.instructure.com/api/v1/");
            
            var started = DateTime.Now;

            var detectedUsers = new JObject();
            var document = new JObject {
                ["detectedUsers"] = detectedUsers,
                ["dateStarted"] = started.ToIso8601Date(),
                ["sentMessages"] = sendMessage
            };

            await foreach (var user in api.StreamUsers()) {
                try {
                    api.MasqueradeAs(user.Id);
                    var (quota, used) = await api.GetPersonalQuotaMiB();
                    api.StopMasquerading();

                    if (used / quota < .85m) {
                        continue;
                    }

                    Console.WriteLine($"Noticed {user.Id, -4} - {Math.Round(used / quota * 100, 3), 8:####.000}%");
                    
                    detectedUsers[user.Id.ToString()] = new JObject {
                        ["userSis"] = user.SisUserId,
                        ["userFullName"] = user.Name,
                        ["quotaUsedMiB"] = used,
                        ["quotaUsedPercent"] = used / quota * 100
                    };

                    if (sendMessage) {
                        var message = string.Format(WarningMessageTemplate, 
                                                    user.Name,
                                                    Math.Round(used / quota * 100, 2), 
                                                    Math.Round(used, 3));

                        await foreach (var c in api.CreateConversation(new QualifiedId[] {user.Id},
                                                                       message,
                                                                       "File Storage Alert",
                                                                       true)) {
                            Console.WriteLine($"Sent the message to {user.Id}.\n{c.ToPrettyString()}\n------\n");
                        }
                    }
                } catch (Exception e) {
                    Console.WriteLine($"Caught exception, skipping check for user {user.Id}.\n{e}\n");
                } finally {
                    api.StopMasquerading();
                }
            }
            
            document["dateCompleted"] = DateTime.Now.ToIso8601Date();
            document["usersInLog"] = detectedUsers.Count;
                
            var outPath = Path.Combine(home.NsDir, $"QuotaWatcher_Log_{started.Ticks}.json");
            File.WriteAllText(outPath, document.ToString(Indented) + "\n");
            Console.WriteLine($"Wrote log to {outPath}");
        }
    }
}
