using System;
using System.IO;
using System.Threading.Tasks;
using dotenv.net;

namespace UVACanvasAccess {
    internal static class Test {
        private const ulong Test1Id = 3390;
        private const ulong Test2Id = 3392;

        public static async Task Main(string[] args) {
            DotEnv.Config();
            
            var api = new Api(Environment.GetEnvironmentVariable("TEST_TOKEN"), 
                              "https://uview.instructure.com/api/v1/");

            var testFilePath = Environment.GetEnvironmentVariable("TEST_FILE");

            var testFile = File.ReadAllBytes(testFilePath ?? throw new Exception());

            var uploaded = await api.UploadPersonalFile(testFile, testFilePath);

            Console.WriteLine($"Successfully uploaded {uploaded.Filename} (id {uploaded.Id}), " +
                              $"which is a {uploaded.MimeClass}, to URL: {uploaded.Url}");
        }
    }
}