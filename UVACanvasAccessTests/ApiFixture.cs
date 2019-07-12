using System;
using dotenv.net;
using UVACanvasAccess.ApiParts;

namespace UVACanvasAccessTests {
    
    // ReSharper disable MemberCanBePrivate.Global
    public class ApiFixture : IDisposable {
        public Api Api { get; }

        public ApiFixture() {
            DotEnv.Config();
            
            Api = new Api(Environment.GetEnvironmentVariable("TEST_TOKEN"), 
                          "https://uview.instructure.com/api/v1/");
        }

        public void Dispose() {
            Api.Dispose();
        }
    }
}