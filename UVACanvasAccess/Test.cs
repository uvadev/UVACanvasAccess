using System;
using System.IO;
using System.Threading.Tasks;
using dotenv.net;

namespace UVACanvasAccess {
    internal static class Test {
        private const ulong TestUser1Id = 3390,
                            TestUser2Id = 3392,
                            TestUser3Id = 3394,
                            TestCourse = 1028,
                            TestDiscussion1 = 375,
                            TestDiscussion2 = 384;

        public static async Task Main(string[] args) {
            DotEnv.Config();
            
            var api = new Api(Environment.GetEnvironmentVariable("TEST_TOKEN"), 
                              "https://uview.instructure.com/api/v1/");

            api.MasqueradeAs(TestUser2Id);

            var path = Environment.GetEnvironmentVariable("TEST_FILE") ?? throw new Exception();
            var bytes = File.ReadAllBytes(path);

            var up = await api.UploadPersonalFile(bytes, path, parentFolderName: "scary");

            Console.WriteLine(up);
        }
    }
}