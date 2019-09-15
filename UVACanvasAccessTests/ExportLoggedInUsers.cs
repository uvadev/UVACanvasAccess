using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using dotenv.net;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Structures.Authentications;
using UVACanvasAccess.Util;
using Xunit;
using Xunit.Abstractions;

namespace UVACanvasAccessTests {
    public class ExportLoggedInUsers {
        private readonly ITestOutputHelper _testOutputHelper;

        private readonly Api _api;
        private const string OutPath = "users_loggedin.csv";

        public ExportLoggedInUsers(ITestOutputHelper testOutputHelper) {
            _testOutputHelper = testOutputHelper;
            DotEnv.Config();
            _api =  new Api(Environment.GetEnvironmentVariable("TEST_TOKEN"), 
                            "https://uview.instructure.com/api/v1/");
        }

        /// <summary>
        /// Exports a .csv of users that have logged in in the past year, according to the Canvas authentication log.
        /// </summary>
        [Fact]
        public async Task Run() {
            var queue = new ConcurrentQueue<string>();

            var events = _api.StreamAccountAuthenticationEvents()
                             .Where(e => e.Event == EventType.Login)
                             .GroupBy(e => e.UserId) 
                             .SelectAwait(async g => (g.Key, await g.Select(e => e.CreatedAt)
                                                                    .AggregateAsync((acc, n) => n.CompareTo(acc) > 0 ? n : acc)));

            await foreach (var (userId, dateTime) in events) {
                try {
                    var name = await _api.GetUser(userId)
                                         .ThenApply(u => u.Name);

                    queue.Enqueue($"{userId},\"{name}\",\"{dateTime: yyyy-MM-dd HH:mm 'UTC+'K}\"");
                } catch (Exception) {
                    _testOutputHelper.WriteLine($"WARNING: This token is unauthorized to view the name of the user with id {userId}.");
                    queue.Enqueue($"{userId},\"(unavailable)\",\"{dateTime: yyyy-MM-dd HH:mm 'UTC+'K}\"");
                }
            }
            
            var content = string.Join("\n", queue);
            
            File.WriteAllText(OutPath, "id,shortName,lastLogin\n" + content);

            _testOutputHelper.WriteLine($"done. {Path.GetFullPath(OutPath)}");
        }
    }
}