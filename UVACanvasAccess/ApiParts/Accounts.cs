using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UVACanvasAccess.Model.Accounts;
using UVACanvasAccess.Structures.Accounts;
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
    }
}