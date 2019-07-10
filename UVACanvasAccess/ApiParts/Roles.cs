using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UVACanvasAccess.Model.Roles;
using UVACanvasAccess.Structures.Roles;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {

        private Task<HttpResponseMessage> RawCreateRole(string accountId, HttpContent content) {
            return _client.PostAsync($"accounts/{accountId}/roles", content);
        }
        
        private async Task<Role> CreateRole(string label,
                                            IEnumerable<(string, string)> permissions,
                                            string baseRoleType,
                                            ulong? accountId) {
            
            var response = await RawCreateRole(accountId?.ToString() ?? "self",
                                               BuildHttpArguments(new [] {
                                                                             ("label", label),
                                                                             ("base_role_type", baseRoleType)
                                                                         }.Concat(permissions))).AssertSuccess();
            
            var model = JsonConvert.DeserializeObject<RoleModel>(await response.Content.ReadAsStringAsync());
            return new Role(this, model);
        }

        public Task<Role> CreateRole(string label,
                                     in RolePermissionsSet? permissions = null,
                                     string baseRoleType = null,
                                     ulong? accountId = null) {
            return CreateRole(label, permissions?.GetAsParams() ?? Enumerable.Empty<(string, string)>(), baseRoleType, accountId);
        }
    }
}