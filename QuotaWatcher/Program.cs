using System;
using System.Threading.Tasks;
using dotenv.net;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Structures;
using UVACanvasAccess.Util;

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
            DotEnv.Config();
            var api = new Api(Environment.GetEnvironmentVariable("DO_NOT_REPLY_TOKEN")
                              ?? ".env should have DO_NOT_REPLY_TOKEN",
                              "https://uview.instructure.com/api/v1/");

            if (await api.GetUser().ThenApply(u => u.Id) != 9140) {
                Console.WriteLine("Token for wrong account.");
                return;
            }

            uint nagged = 0;
            await foreach (var user in api.StreamUsers()) {
                try {
                    api.MasqueradeAs(user.Id);
                    var (quota, used) = await api.GetPersonalQuotaMiB();
                    api.StopMasquerading();

                    if (used / quota < .85m) {
                        continue;
                    }

                    Console.WriteLine($"Nagging {user.Id} ({used}/{quota})");
                    
                    var message = string.Format(WarningMessageTemplate, 
                                                user.Name,
                                                Math.Round(used / quota * 100, 2), 
                                                Math.Round(used, 3));

                    await foreach (var c in api.CreateConversation(new QualifiedId[] {user.Id},
                                                                   message,
                                                                   "File Storage Alert",
                                                                   true)) {
                        Console.WriteLine($"sent the message to {user.Id}.\n{c.ToPrettyString()}\n------\n");
                        ++nagged;
                    }
                } catch (Exception e) {
                    Console.WriteLine(e);
                } finally {
                    api.StopMasquerading();
                }
            }

            Console.WriteLine($"nagged {nagged} users.");
        }
    }
}
