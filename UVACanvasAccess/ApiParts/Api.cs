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
    
    [PublicAPI]
    public partial class Api : IDisposable {

        private readonly HttpClient _client;
        private ulong? _masquerade;
        
        /// <summary>
        /// Construct a new API instance.
        /// </summary>
        /// <param name="token">The token used to authenticate this API instance.</param>
        /// <param name="baseUrl">The base url of the API server, ending with the version number.
        /// Ex: <c>https://uview.instructure.com/api/v1/</c>
        /// </param>
        public Api(string token, string baseUrl) {
            _client = new HttpClient();
            
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _client.BaseAddress = new Uri(baseUrl);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        static Api() {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings {
                FloatParseHandling = FloatParseHandling.Decimal
            };
        }

        public void Dispose() {
            _client?.Dispose();
        }

        #if DEBUG
        internal void TestGet(string url, out bool success, out JToken response, params ValueTuple<string, string>[] args) {
            var r = _client.GetAsync(url + BuildQueryString(args)).Result;
            response = JToken.Parse(r.Content.ReadAsStringAsync().Result);
            success = r.IsSuccessStatusCode;
        }
        #endif

        /// <summary>
        /// If the current user is an administrator, he can "act as" another user.
        /// When this is set, every API call will be made as if it came from this user's token.
        /// </summary>
        /// <param name="id">The user to masquerade as.</param>
        /// <remarks>Certain endpoints, like those relating to the activity stream, can only be called on
        /// <c>self</c>. Masquerading makes it possible to bypass this restriction.</remarks>
        public void MasqueradeAs(ulong id) {
            _masquerade = id;
        }

        /// <summary>
        /// Stop "acting as" any user.
        /// </summary>
        public void StopMasquerading() {
            _masquerade = null;
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

            if (_masquerade != null) {
                query["as_user_id"] = _masquerade.ToString();
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

            if (_masquerade != null) {
                entries.Add($"as_user_id={_masquerade.ToString()}");
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

            var pairs = from a in args
                        where a.Item2 != null
                        select new KeyValuePair<string, string>(a.Item1, a.Item2);

            if (_masquerade != null) {
                pairs = pairs.Append(new KeyValuePair<string, string>("as_user_id", _masquerade.ToString()));
            }
            
            var content =
                new FormUrlEncodedContent(pairs);
            
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            return content;
        }

        private MultipartFormDataContent BuildMultipartHttpArguments([NotNull] IEnumerable<ValueTuple<string, string>> args) {
            var content = new MultipartFormDataContent();
            foreach (var (k, v) in args) {
                content.Add(new StringContent(v), k);
            }

            if (_masquerade != null) {
                content.Add(new StringContent(_masquerade.ToString()), "as_user_id");
            }

            return content;
        }

        private static HttpContent BuildHttpJsonBody(JObject json) {
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
                
                response = await _client.GetAsync(links.NextLink);
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
                
                response = await _client.GetAsync(links.NextLink);
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
            response.AssertSuccess();

            while (response.Headers.TryGetValues("Link", out IEnumerable<string> linkValues)) {
                var links = LinkHeader.LinksFromHeader(linkValues.First());
                if (links?.NextLink == null)
                    break;

                response = await _client.GetAsync(links.NextLink);
                
                var content = response.AssertSuccess().Content;

                yield return JToken.Parse(await content.ReadAsStringAsync());
            }
        }

        private async IAsyncEnumerable<TElement> StreamDeserializePages<TElement>(HttpResponseMessage response) {
            response.AssertSuccess();

            while (response.Headers.TryGetValues("Link", out IEnumerable<string> linkValues)) {
                var links = LinkHeader.LinksFromHeader(linkValues.First());
                if (links?.NextLink == null)
                    break;

                response = await _client.GetAsync(links.NextLink);
                
                var content = response.AssertSuccess().Content;

                var list = JsonConvert.DeserializeObject<List<TElement>>(await content.ReadAsStringAsync());
                foreach (var e in list) {
                    yield return e;
                }
            }
        }

        [PublicAPI]
        public enum Order {
            [ApiRepresentation("asc")]
            Ascending,
            [ApiRepresentation("desc")]
            Descending
        }
    }
}