using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dotenv.net;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Debugging;
using UVACanvasAccess.Util;

namespace UVACanvasAccess {
    internal static class Test {
        private const ulong TestUser1Id = 3390,
                            TestUser2Id = 3392,
                            TestUser3Id = 3394,
                            TestUser4Id = 3431,
                            TestCourse = 1028,
                            TestDiscussion1 = 375,
                            TestDiscussion2 = 384,
                            TestAssignment1 = 9844,
                            TestAssignment2 = 10486,
                            TestAssignment2Override1 = 70,
                            TestAssignment2Override2 = 71,
                            TestAssignment1Override1 = 72,
                            TestingSubAccount = 154, 
                            TestDomainCourse = 1268;

        public static async Task Main(string[] args) {
            Debug.Listeners.Add(new TraceToStdErr());
            DotEnv.Config();
            
            var fileMapDir = Environment.GetEnvironmentVariable("TEST_MAP_DIR") 
                             ?? throw new ArgumentException(".env should contain TEST_MAP_DIR");
            if (!fileMapDir.EndsWith(Path.DirectorySeparatorChar.ToString())) {
                fileMapDir += Path.PathSeparator;
            }

            var list = File.ReadAllLines(fileMapDir + "map.csv").ToList();
            
            List<string>[] taskLists = list.Chunk(list.Count / Environment.ProcessorCount)
                                           .ToArray();

            var nThreads = taskLists.Length;
            
            var apis = new Api[nThreads];

            for (int i = 0; i < nThreads; i++) {
                apis[i] = new Api(Environment.GetEnvironmentVariable("TEST_TOKEN") 
                                  ?? throw new ArgumentException(".env should contain TEST_TOKEN"), 
                                  "https://uview.instructure.com/api/v1/");
            }

            Console.WriteLine($"Using {nThreads} threads.");
            
            var completed = new ConcurrentBag<ulong>();

            using (var countdown = new CountdownEvent(nThreads)) {
                for (int i = 0; i < nThreads; i++) {
                    ThreadPool.QueueUserWorkItem(o => {
                        try {
                            var n = (int) o;
                            foreach (var line in taskLists[n]) {
                                string[] halves = line.Split(',');
                                Debug.Assert(halves.Length == 2);

                                var (userKey, userFile) = (halves[0], halves[1]);

                                var api = apis[n];

                                try {
                                    var user = api.StreamUsers(userKey)
                                                  .FirstAsync()
                                                  .Result;

                                    var bytes = File.ReadAllBytes(fileMapDir + userFile);

                                    Console.WriteLine($"Preparing to upload filename {userFile} to user " +
                                                      $"{userKey}, Id {user.Id}, SIS {user.SisUserId}");

                                    api.MasqueradeAs(user.Id);

                                    var file = api.UploadPersonalFile(bytes,
                                                                      userFile,
                                                                      "test_csv_file_upload_p2")
                                                  .Result;

                                    Console.WriteLine($"Uploaded as {file.Id}!");
                                    completed.Add(file.Id);
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
        }
    }
}