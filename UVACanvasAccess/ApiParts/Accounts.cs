using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UVACanvasAccess.Model.Accounts;
using UVACanvasAccess.Structures.Accounts;
using UVACanvasAccess.Structures.Roles;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {

        [Flags]
        public enum AccountIncludes : byte {
            Default = 0,
            [ApiRepresentation("lti_guid")]
            LtiGuid = 1 << 0,
            [ApiRepresentation("registration_settings")]
            RegistrationSettings = 1 << 1,
            [ApiRepresentation("services")]
            Services = 1 << 2
        }

        [PaginatedResponse]
        private Task<HttpResponseMessage> RawListAccounts([NotNull] IEnumerable<string> includes) {
            return _client.GetAsync("accounts" + BuildQueryString(includes.Select(i => ("include[]", i)).ToArray()));
        }

        public async Task<IEnumerable<Account>> ListAccounts(AccountIncludes includes = AccountIncludes.Default) {
            var response = await RawListAccounts(includes.GetFlags().Select(f => f.GetApiRepresentation()));

            var models = await AccumulateDeserializePages<AccountModel>(response);

            return from model in models
                   select new Account(this, model);
        }

        private Task<HttpResponseMessage> RawGetAccount(string id) {
            return _client.GetAsync($"accounts/{id}");
        }

        public async Task<Account> GetAccount(ulong accountId) {
            var response = await RawGetAccount(accountId.ToString());

            var model = JsonConvert.DeserializeObject<AccountModel>(await response.Content.ReadAsStringAsync());
            return new Account(this, model);
        }

        private Task<HttpResponseMessage> RawGetAccountPermissions(string id, [NotNull] IEnumerable<string> permissions) {
            var url = $"accounts/{id}/permissions";
            return _client.GetAsync(url + BuildDuplicateKeyQueryString(permissions.Select(p => ("permissions[]", p)).ToArray()));
        }

        public async Task<BasicAccountPermissionsSet> GetAccountPermissions(AccountRolePermissions checkedPermissions, 
                                                                            ulong? accountId = null) {
            var response = await RawGetAccountPermissions(accountId?.ToString() ?? "self",
                                                          checkedPermissions.GetFlags()
                                                                            .Select(p => p.GetApiRepresentation()));
            
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, bool>>(await response.Content.ReadAsStringAsync());

            AccountRolePermissions allowed = default, 
                                   denied = default;

            foreach (var (k, v) in dictionary) {
                AccountRolePermissions? permission = k.ToApiRepresentedEnum<AccountRolePermissions>();
                if (permission == null) {
                    Console.Error.WriteLine("WARNING: encountered unknown permission type: " + k);
                    continue;
                }
                
                if (v) {
                    allowed |= permission.Value;
                } else {
                    denied |= permission.Value;
                }
            }
            
            return new BasicAccountPermissionsSet(allowed, denied);
        }

        private Task<HttpResponseMessage> RawGetTermsOfService(string id) {
            return _client.GetAsync($"accounts/{id}/terms_of_service");
        }

        public async Task<TermsOfService> GetTermsOfService(ulong? accountId = null) {
            var response = await RawGetTermsOfService(accountId?.ToString() ?? "self");

            var model = JsonConvert.DeserializeObject<TermsOfServiceModel>(await response.Content.ReadAsStringAsync());
            return new TermsOfService(this, model);
        }
    }
}