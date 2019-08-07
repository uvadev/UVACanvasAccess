using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UVACanvasAccess.Model.Users;
using UVACanvasAccess.Structures.Users;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {

        /// <summary>
        /// Streams the list of observees associated with an observer.
        /// </summary>
        /// <param name="observerId">The observer.</param>
        /// <returns>The stream of observees.</returns>
        public async IAsyncEnumerable<User> StreamObservees(ulong observerId) {
            var result = await _client.GetAsync($"users/{observerId}/observees?include[]=avatar_url");

            await foreach (var model in StreamDeserializePages<UserModel>(result)) {
                yield return new User(this, model);
            }
        }

        /// <summary>
        /// Set a user to be observed by another user.
        /// </summary>
        /// <param name="observerId">The user to become the observer.</param>
        /// <param name="observeeId">The user to become the observee.</param>
        /// <param name="rootAccountId">(Optional) The id of the root account with which to associate the link.</param>
        /// <returns>The observee.</returns>
        public async Task<User> AddObservee(ulong observerId, ulong observeeId, ulong? rootAccountId = null) {
            var args = BuildHttpArguments(new[] {("root_account_id", rootAccountId?.ToString())});
            var response = await _client.PutAsync($"users/{observerId}/observees/{observeeId}", args);

            var model = JsonConvert.DeserializeObject<UserModel>(await response.Content.ReadAsStringAsync());
            return new User(this, model);
        }

        /// <summary>
        /// Set a user to stop being observed by another user.
        /// </summary>
        /// <param name="observerId">The observer.</param>
        /// <param name="observeeId">The observee.</param>
        /// <param name="rootAccountId">(Optional) The id of the root account from which to remove the link.</param>
        /// <returns>The former observee.</returns>
        public async Task<User> RemoveObservee(ulong observerId, ulong observeeId, ulong? rootAccountId = null) {
            var args = BuildQueryString(("root_account_id", rootAccountId?.ToString()));
            var response = await _client.DeleteAsync($"users/{observerId}/observees/{observeeId}" + args);

            var model = JsonConvert.DeserializeObject<UserModel>(await response.Content.ReadAsStringAsync());
            return new User(this, model);
        }
    }
}
