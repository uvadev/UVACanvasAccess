using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UVACanvasAccess.Model.Authentications;
using UVACanvasAccess.Structures.Authentications;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {

        [PaginatedResponse]
        private Task<HttpResponseMessage> RawListAuthenticationEvents(string endpoint, 
                                                                      string id,
                                                                      DateTime? start,
                                                                      DateTime? end) {
            var url = $"audit/authentication/{endpoint}/{id}";
            return _client.GetAsync(url+ BuildQueryString(("start_time", start?.ToIso8601Date()), 
                                                                   ("end_time", end?.ToIso8601Date())));
        }

        public async IAsyncEnumerable<AuthenticationEvent> StreamUserAuthenticationEvents(ulong userId,
                                                                                          DateTime? start = null,
                                                                                          DateTime? end = null) {
            var response = await RawListAuthenticationEvents("users", userId.ToString(), start, end);

            var events = StreamDeserializeObjectPages<AuthenticationEventsResponseModel>(response)
                        .SelectMany(r => r.Events.ToAsyncEnumerable(),
                                    (_, m) => new AuthenticationEvent(this, m));

            await foreach (var e in events) {
                yield return e;
            }
        }
        
        public async IAsyncEnumerable<AuthenticationEvent> StreamAccountAuthenticationEvents(ulong? accountId = null,
                                                                                             DateTime? start = null,
                                                                                             DateTime? end = null) {
            var response = await RawListAuthenticationEvents("accounts", accountId?.ToString() ?? "self", start, end);

            var events = StreamDeserializeObjectPages<AuthenticationEventsResponseModel>(response)
                        .SelectMany(r => r.Events.ToAsyncEnumerable(),
                                    (_, m) => new AuthenticationEvent(this, m));

            await foreach (var e in events) {
                yield return e;
            }
        }
    }
}