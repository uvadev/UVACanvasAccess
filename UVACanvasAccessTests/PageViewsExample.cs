using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using dotenv.net;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Structures.Users;
using Xunit;

namespace UVACanvasAccessTests {
    public class PageViewsExample {

        private const ulong TestUser = 3392;
        private readonly Api _api;

        public PageViewsExample() {
            DotEnv.Config();
            _api =  new Api(Environment.GetEnvironmentVariable("TEST_TOKEN"), 
                            "https://uview.instructure.com/api/v1/");
        }
        
        [Fact]
        public async Task OutputPageViewsToCsv() {
            
            // get the test user by his id
            var testUser = await _api.GetUserDetails(TestUser);

            // accumulate all the user's page views
            IEnumerable<PageView> views = await testUser.GetPageViews();
            
            // format each view as a csv row
            IEnumerable<string> rows = views.Select(view => $@"""{testUser.Name}"",{testUser.Id},""{view.CreatedAt}""," + 
                                                            $@"""{view.Url}"",""{view.RemoteIp}""");

            // join the rows
            var csv = "Name,UserId,Date,Url,Ip\n" + string.Join("\n", rows);
            
            File.WriteAllText("out.csv", csv);
        }
        
    }
}