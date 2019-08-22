using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using dotenv.net;
using UVACanvasAccess.ApiParts;
using Xunit;
using Xunit.Abstractions;

namespace UVACanvasAccessTests {

    public class SingleThreadedFileMapUpload {
        private readonly ITestOutputHelper _testOutputHelper;

        private readonly Api _api;
        
        public SingleThreadedFileMapUpload(ITestOutputHelper testOutputHelper) {
            _testOutputHelper = testOutputHelper;
            DotEnv.Config();
            _api = new Api(Environment.GetEnvironmentVariable("TEST_TOKEN") 
                           ?? throw new ArgumentException(".env should contain TEST_TOKEN"), 
                           "https://uview.instructure.com/api/v1/");
        }

        [Fact]
        public async Task Run() {
            var fileMapDir = Environment.GetEnvironmentVariable("TEST_MAP_DIR")
                             ?? throw new ArgumentException(".env should contain TEST_MAP_DIR");
            if (!fileMapDir.EndsWith(Path.DirectorySeparatorChar.ToString())) {
                fileMapDir += Path.PathSeparator;
            }

            foreach (var line in File.ReadLines(fileMapDir + "map.csv")) {
                try {
                    string[] halves = line.Split(',');
                    Debug.Assert(halves.Length == 2);

                    var (userKey, userFile) = (halves[0], halves[1]);

                    var user = _api.StreamUsers(userKey)
                                  .FirstOrDefaultAsync(u => u.SisUserId == userKey)
                                  .Result;
                                    
                    if (user == null) {
                        _testOutputHelper.WriteLine($"WARN: Couldn't find the user for sis {userKey} !!");
                        continue;
                    }

                    byte[] bytes = File.ReadAllBytes(fileMapDir + userFile);

                    _testOutputHelper.WriteLine($"Preparing to upload filename {userFile} to user {userKey}, Id {user.Id}, SIS {user.SisUserId}");

                    _api.MasqueradeAs(user.Id);

                    var file = await _api.UploadPersonalFile(bytes, userFile, "test_csv_file_upload");

                    _testOutputHelper.WriteLine($"Uploaded as {file.Id}!");
                } finally {
                    _api.StopMasquerading();
                }
            }
        }
    }
}