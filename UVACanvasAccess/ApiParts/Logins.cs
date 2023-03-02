using System.Collections.Generic;
using UVACanvasAccess.Model.Logins;
using UVACanvasAccess.Structures.Logins;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {

        private async IAsyncEnumerable<UserLogin> StreamLogins(string url) {
            var response = await client.GetAsync($"{url}" + BuildQueryString());
            
            await foreach (var model in StreamDeserializePages<UserLoginModel>(response)) {
                yield return new UserLogin(this, model);
            }
        }

        public IAsyncEnumerable<UserLogin> StreamUserLogins(ulong userId) {
            return StreamLogins($"users/{userId}/logins");
        }
        
        public IAsyncEnumerable<UserLogin> StreamAccountLogins(ulong accountId) {
            return StreamLogins($"accounts/{accountId}/logins");
        }
    }
}
