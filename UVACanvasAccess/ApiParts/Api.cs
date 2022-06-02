using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.ApiParts {
    
    /// <summary>
    /// This class is the primary interface for interacting with the Canvas API. <br/>
    /// </summary>
    /// <remarks>
    /// API instances are intended to be be reused between calls, and should only be constructed once per token, per thread. <br/>
    /// The only internal state maintained by this class is <see cref="HttpClient">HttpClient</see>, which is thread safe;
    /// and the masquerade state, which is <b>not</b> thread safe. Do not share the same instance of this class between
    /// threads if any thread will call <see cref="MasqueradeAs"/> or <see cref="StopMasquerading"/>.
    /// </remarks>
    [PublicAPI]
    public partial class Api : IDisposable {
        
        private readonly HttpClient client;
        private ulong? masquerade;

        /// <summary>
        /// Construct a new API instance.
        /// </summary>
        /// <param name="token">The token used to authenticate this API instance.</param>
        /// <param name="baseUrl">The base API url, e.g. <c>https://YOUR_ORGANIZATION.instructure.com/api/v1/</c></param>
        /// <param name="timeout">(Optional) The maximum amount of time one request may take before it is cancelled.</param>
        public Api(string token, string baseUrl, TimeSpan? timeout = null) {
            client = new HttpClient();
            
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (timeout != null) {
                client.Timeout = timeout.Value;
            }
        }

        static Api() {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings {
                FloatParseHandling = FloatParseHandling.Decimal
            };
        }

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose() {
            client?.Dispose();
        }

        #if DEBUG
        internal void TestGet(string url, out bool success, out JToken response, out LinkHeader links, 
                              params ValueTuple<string, string>[] args) {
            var r = client.GetAsync(url + BuildDuplicateKeyQueryString(args)).Result;
            
            response = JToken.Parse(r.Content.ReadAsStringAsync().Result);
            success = r.IsSuccessStatusCode;
            
            r.Headers.TryGetValues("Link", out var l);
            links = l?.First().ConvertIfNotNull(LinkHeader.LinksFromHeader);
        }
        #endif
        
        #if DEBUG
        internal void TestDelete(string url, out bool success, out JToken response, out LinkHeader links, 
                                 params ValueTuple<string, string>[] args) {
            var r = client.DeleteAsync(url + BuildDuplicateKeyQueryString(args)).Result;
            
            response = JToken.Parse(r.Content.ReadAsStringAsync().Result);
            success = r.IsSuccessStatusCode;
            
            r.Headers.TryGetValues("Link", out var l);
            links = l?.First().ConvertIfNotNull(LinkHeader.LinksFromHeader);
        }
        #endif

        /// <summary>
        /// If the current user is an administrator, he can "act as" another user.
        /// When this is set, every API call will be made as if it came from this user's token, even if the user does
        /// not have any tokens generated. Audit logs will indicate if an action was made through masquerade, though.<br/>
        /// 
        /// Masquerading <b>is not thread safe</b>, so do not share <see cref="Api"/> between threads if using masquerading.
        /// </summary>
        /// <param name="id">The user to masquerade as.</param>
        /// <remarks>Certain endpoints, for example those relating to the activity stream and personal files, can only
        /// be called on <c>self</c>. Masquerading makes it possible to bypass this restriction.</remarks>
        public void MasqueradeAs(ulong id) {
            masquerade = id;
        }

        /// <summary>
        /// Stop "acting as" any user.
        /// </summary>
        public void StopMasquerading() {
            masquerade = null;
        }

        /// <summary>
        /// Constructs a query string with the given key-value pairs.
        /// </summary>
        /// <param name="args">The key-value pairs to use in the query string. Null values are ignored.</param>
        /// <returns>The query string.</returns>
        private string BuildQueryString([NotNull] params ValueTuple<string, string>[] args) {
            var query = HttpUtility.ParseQueryString(string.Empty);

            foreach (var (key, val) in args) {
                if (val != null) {
                    query[key] = val;
                }
            }

            if (masquerade != null) {
                query["as_user_id"] = masquerade.ToString();
            }
            
            var s = query.ToString();

            return s == string.Empty ? s
                                     : "?" + s;
        }

        private string BuildDuplicateKeyQueryString([NotNull] params ValueTuple<string, string>[] args) {
            var entries = new List<string>();
            
            foreach (var (key, val) in args) {
                if (val != null) {
                    entries.Add($"{HttpUtility.UrlEncode(key)}={HttpUtility.UrlEncode(val)}");
                }
            }

            if (masquerade != null) {
                entries.Add($"as_user_id={masquerade.ToString()}");
            }

            if (entries.Count == 0) {
                return string.Empty;
            }

            return "?" + string.Join("&", entries);
        }

        /// <summary>
        /// Constructs a <c>x-www-form-url-encoded</c> <c>HttpContent</c> with the given key-value pairs.
        /// </summary>
        /// <param name="args">The set of key-value tuples.</param>
        /// <returns></returns>
        private HttpContent BuildHttpArguments([NotNull] IEnumerable<ValueTuple<string, string>> args) {

            var pairs = args.Where(a => a.Item2 != null)
                            .Select(a => new KeyValuePair<string, string>(a.Item1, a.Item2));

            if (masquerade != null) {
                pairs = pairs.Append(new KeyValuePair<string, string>("as_user_id", masquerade.ToString()));
            }
            
            var content = new FormUrlEncodedContent(pairs);
            
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            return content;
        }

        private MultipartFormDataContent BuildMultipartHttpArguments([NotNull] IEnumerable<ValueTuple<string, string>> args) {
            var content = new MultipartFormDataContent();
            foreach (var (k, v) in args) {
                content.Add(new StringContent(v), k);
            }

            if (masquerade != null) {
                content.Add(new StringContent(masquerade.ToString()), "as_user_id");
            }

            return content;
        }

        private HttpContent BuildHttpJsonBody(JObject json) {
            if (masquerade != null) {
                json.Add("as_user_id", masquerade.ToString());
            }
            var content = new StringContent(json.ToString(), Encoding.Default, "application/json");
            return content;
        }

        /// <summary>
        /// Accumulates all the elements in a paginated response.
        /// </summary>
        /// <param name="response">The first response received after initiating the request.</param>
        /// <returns>The list of JSON elements.</returns>
        private async Task<List<JToken>> AccumulatePages(HttpResponseMessage response) {
            response.AssertSuccess();

            var pages = new List<HttpContent> { response.Content };
            
            while (response.Headers.TryGetValues("Link", out var linkValues)) {
                var links = LinkHeader.LinksFromHeader(linkValues.First());
                if (links?.NextLink == null)
                    break;
                
                response = await client.GetAsync(links.NextLink);
                response.AssertSuccess();
                pages.Add(response.Content);
            }

            var accumulated = new List<JToken>();

            foreach (var content in pages) {
                var responseStr = await content.ReadAsStringAsync();
                accumulated.AddRange(JToken.Parse(responseStr));
            }

            return accumulated;
        }
        
        /// <summary>
        /// Accumulates all the elements in a paginated response, then deserializes each element as <c>TElement</c>.
        /// </summary>
        /// <param name="response">The first response received after initiating the request.</param>
        /// <typeparam name="TElement">The model type of the elements returned in the response.</typeparam>
        /// <returns>The list of deserialized elements.</returns>
        private async Task<List<TElement>> AccumulateDeserializePages<TElement>(HttpResponseMessage response) {
            response.AssertSuccess();

            var pages = new List<HttpContent> { response.Content };
            
            while (response.Headers.TryGetValues("Link", out IEnumerable<string> linkValues)) {
                var links = LinkHeader.LinksFromHeader(linkValues.First());
                if (links?.NextLink == null)
                    break;
                
                response = await client.GetAsync(links.NextLink);
                response.AssertSuccess();
                pages.Add(response.Content);
            }

            var accumulated = new List<TElement>();

            foreach (var content in pages) {
                var responseStr = await content.ReadAsStringAsync();
                accumulated.AddRange(JsonConvert.DeserializeObject<List<TElement>>(responseStr));
            }

            return accumulated;
        }

        private async IAsyncEnumerable<JToken> StreamPages(HttpResponseMessage response) {

            var firstPage = JToken.Parse(await response.AssertSuccess()
                                                       .Content
                                                       .ReadAsStringAsync());

            yield return firstPage;

            while (response.Headers.TryGetValues("Link", out IEnumerable<string> linkValues)) {
                var links = LinkHeader.LinksFromHeader(linkValues.First());
                if (links?.NextLink == null)
                    break;

                response = await client.GetAsync(links.NextLink);
                
                var content = response.AssertSuccess().Content;

                yield return JToken.Parse(await content.ReadAsStringAsync());
            }
        }

        private async IAsyncEnumerable<TElement> StreamDeserializePages<TElement>(HttpResponseMessage response) {

            var firstPage = JsonConvert.DeserializeObject<List<TElement>>(await response.AssertSuccess()
                                                                                        .Content
                                                                                        .ReadAsStringAsync());

            foreach (var e in firstPage) {
                yield return e;
            }

            while (response.Headers.TryGetValues("Link", out IEnumerable<string> linkValues)) {
                var links = LinkHeader.LinksFromHeader(linkValues.First());
                if (links?.NextLink == null)
                    break;

                response = await client.GetAsync(links.NextLink);
                
                var content = response.AssertSuccess().Content;

                var list = JsonConvert.DeserializeObject<List<TElement>>(await content.ReadAsStringAsync());
                foreach (var e in list) {
                    yield return e;
                }
            }
        }
        
        private async IAsyncEnumerable<TObject> StreamDeserializeObjectPages<TObject>(HttpResponseMessage response) {

            var firstPage = JsonConvert.DeserializeObject<TObject>(await response.AssertSuccess()
                                                                                 .Content
                                                                                 .ReadAsStringAsync());

            yield return firstPage;

            while (response.Headers.TryGetValues("Link", out IEnumerable<string> linkValues)) {
                var links = LinkHeader.LinksFromHeader(linkValues.First());
                if (links?.NextLink == null)
                    break;

                response = await client.GetAsync(links.NextLink);
                
                var content = response.AssertSuccess().Content;

                var page = JsonConvert.DeserializeObject<TObject>(await content.ReadAsStringAsync());
                yield return page;
            }
        }

        /// <summary>
        /// Direction to sort by.
        /// </summary>
        [PublicAPI]
        public enum Order : byte {
            /// <summary>
            /// Sort in ascending order.
            /// </summary>
            [ApiRepresentation("asc")]
            Ascending,
            /// <summary>
            /// Sort in descending order.
            /// </summary>
            [ApiRepresentation("desc")]
            Descending
        }

        /// <summary>
        /// Behavior when encountering a duplicate item.
        /// </summary>
        [PublicAPI]
        public enum OnDuplicate : byte  {
            /// <summary>
            /// Overwrite the previous item with the new item.
            /// </summary>
            [ApiRepresentation("overwrite")]
            Overwrite,
            /// <summary>
            /// Rename the new item.
            /// </summary>
            [ApiRepresentation("rename")]
            Rename
        }
    }
}