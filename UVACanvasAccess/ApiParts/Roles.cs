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

        [PaginatedResponse]
        private Task<HttpResponseMessage> RawListRoles(string accountId,
                                                       IEnumerable<string> states, 
                                                       bool? showInherited) {
            var url = $"accounts/{accountId}/roles";

            var args = states.Select(s => ("state[]", s))
                             .Append(("show_inherited", showInherited?.ToShortString()));
            
            return _client.GetAsync(url + BuildQueryString(args.ToArray()));
        }

        public async Task<IEnumerable<Role>> ListRoles(RoleState states = 0, bool? showInherited = null, ulong? accountId = null) {
            var response = await RawListRoles(accountId?.ToString() ?? "self", 
                                              states.GetFlags().Select(f => f.GetApiRepresentation()),
                                              showInherited);

            var models = await AccumulateDeserializePages<RoleModel>(response);

            return from model in models
                   select new Role(this, model);
        }

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