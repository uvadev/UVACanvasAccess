using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UVACanvasAccess.Model.Users;
using UVACanvasAccess.Structures.Users;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {

        public async IAsyncEnumerable<User> StreamObservees(ulong observerId) {
            var result = await _client.GetAsync($"users/{observerId}/observees?include[]=avatar_url");

            await foreach (var model in StreamDeserializePages<UserModel>(result)) {
                yield return new User(this, model);
            }
        }

        public async Task<User> AddObservee(ulong observerId, ulong observeeId, ulong? rootAccountId = null) {
            var args = BuildHttpArguments(new[] {("root_account_id", rootAccountId?.ToString())});
            var response = await _client.PutAsync($"users/{observerId}/observees/{observeeId}", args);

            var model = JsonConvert.DeserializeObject<UserModel>(await response.Content.ReadAsStringAsync());
            return new User(this, model);
        }

        public async Task<User> RemoveObservee(ulong observerId, ulong observeeId, ulong? rootAccountId = null) {
            var args = BuildQueryString(("root_account_id", rootAccountId?.ToString()));
            var response = await _client.DeleteAsync($"users/{observerId}/observees/{observeeId}" + args);

            var model = JsonConvert.DeserializeObject<UserModel>(await response.Content.ReadAsStringAsync());
            return new User(this, model);
        }
    }
}
