using System;
using System.Threading.Tasks;
using dotenv.net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UVACanvasAccess.ApiParts;
using Xunit;
using Xunit.Abstractions;

namespace UVACanvasAccessTests {
    public class ObserversToObservees {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly Api _api;

        public ObserversToObservees(ITestOutputHelper testOutputHelper) {
            _testOutputHelper = testOutputHelper;
            DotEnv.Config();
            _api = new Api(Environment.GetEnvironmentVariable("TEST_TOKEN") 
                           ?? throw new ArgumentException(".env should contain TEST_TOKEN"), 
                           "https://uview.instructure.com/api/v1/");
        }

        [Fact]
        public async Task Run() {
            var observers = new ulong[] {3390, 3392};

            var entries = new JArray();
            
            foreach (var observer in observers) {
                var observees = new JArray();
                await foreach (var observee in _api.StreamObservees(observer)) {
                    observees.Add(observee.Id);
                }
                entries.Add(JObject.FromObject(new {
                    observer,
                    observees
                }));
            }

            _testOutputHelper.WriteLine(entries.ToString(Formatting.Indented));
        }
    }
}
