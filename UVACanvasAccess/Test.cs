using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using dotenv.net;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Debugging;

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
            
            var api = new Api(Environment.GetEnvironmentVariable("TEST_TOKEN") 
                              ?? throw new ArgumentException(".env should contain TEST_TOKEN"), 
                              "https://uview.instructure.com/api/v1/");

            foreach (var line in File.ReadLines(fileMapDir + "map.csv")) {
                try {
                    string[] halves = line.Split(',');
                    Debug.Assert(halves.Length == 2);
                    
                    var (userKey, userFile) = (halves[0], halves[1]);

                    var user = await api.StreamUsers(userKey)
                                        .FirstAsync();
                    
                    var bytes = File.ReadAllBytes(fileMapDir + userFile);

                    Console.WriteLine($"Preparing to upload filename {userFile} to user {userKey}, Id {user.Id}, SIS {user.SisUserId}");
                    
                    api.MasqueradeAs(user.Id);

                    var file = await api.UploadPersonalFile(bytes, userFile, "test_csv_file_upload");

                    Console.WriteLine($"Uploaded as {file.Id}!");
                } finally {
                    api.StopMasquerading();
                }
            }
        }
    }
}