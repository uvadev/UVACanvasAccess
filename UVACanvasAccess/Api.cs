using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UVACanvasAccess.Builders;
using UVACanvasAccess.Model.Users;
using UVACanvasAccess.Structures.Users;

namespace UVACanvasAccess {
    public class Api : IDisposable {

        private readonly HttpClient _client;
        
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

        public void Dispose() {
            _client?.Dispose();
        }

        /// <summary>
        /// Constructs a query string with the given key-value pairs.
        /// </summary>
        /// <param name="args">The key-value pairs to use in the query string. Null values are ignored.</param>
        /// <returns>The query string.</returns>
        private static string BuildQueryString(params ValueTuple<string, string>[] args) {
            var query = HttpUtility.ParseQueryString(string.Empty);

            foreach (var (key, val) in args) {
                if (val != null) {
                    query[key] = val;
                }
            }

            var s = query.ToString();

            return s == string.Empty ? s
                                     : "?" + s;
        }

        /// <summary>
        /// Constructs a <c>x-www-form-url-encoded</c> <c>HttpContent</c> with the given key-value pairs.
        /// </summary>
        /// <param name="args">The set of key-value tuples.</param>
        /// <returns></returns>
        private static HttpContent BuildHttpArguments(IEnumerable<ValueTuple<string, string>> args) {
            var content =
                new FormUrlEncodedContent(args.Select(a => new KeyValuePair<string, string>(a.Item1, a.Item2)));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            return content;
        }

        private static HttpContent BuildHttpJsonBody(JObject json) {
            var content = new StringContent(json.ToString(), Encoding.Default, "application/json");
            return content;
        }

        private Task<HttpResponseMessage> RawLoadCustomJson(string userId, string scopes, string ns) {
            return _client.GetAsync($"users/{userId}/custom_data/{scopes}" + BuildQueryString(("ns", ns)));
        }

        /// <summary>
        /// Retrieve arbitrary user data as JSON.
        /// Corresponds to the API endpoint <c>GET /api/v1/users/:user_id/custom_data(/*scope)</c>.
        /// </summary>
        /// <param name="ns">The namespace under which the data is stored.</param>
        /// <param name="scopes">The scope, and optionally subscopes, under which the data is stored.</param>
        /// <param name="userId">The id of the user.</param>
        /// <returns>The JSON data.</returns>
        /// <exception cref="Exception"></exception>
        /// <seealso cref="StoreCustomJson"/>
        public async Task<JObject> LoadCustomJson(string ns, string scopes, ulong? userId = null) {
            var response = await RawLoadCustomJson(userId?.ToString() ?? "self", scopes, ns);
            
            if (!response.IsSuccessStatusCode) {
                throw new Exception($"http failure response: {response.StatusCode} {response.ReasonPhrase}");
            }

            var responseStr = await response.Content.ReadAsStringAsync();
            return JObject.Parse(responseStr);
        }

        private Task<HttpResponseMessage> RawStoreCustomJson(string userId, string scopes, HttpContent content) {
            return _client.PutAsync($"users/{userId}/custom_data/{scopes}", content);
        }

        /// <summary>
        /// Store arbitrary user data as JSON. 
        /// Corresponds to the API endpoint <c>PUT /api/v1/users/:user_id/custom_data(/*scope)</c>.
        /// </summary>
        /// <param name="ns">The namespace under which to store the data.</param>
        /// <param name="scopes">The scope, and optionally subscopes, under which to store the data.</param>
        /// <param name="data">The JSON data.</param>
        /// <param name="userId">The id of the user.</param>
        /// <returns>A copy of the stored json.</returns>
        /// <exception cref="Exception"></exception>
        /// <seealso cref="LoadCustomJson"/>
        public async Task<JObject> StoreCustomJson(string ns, string scopes, JObject data, ulong? userId = null) {

            var json = new JObject {
                                       ["ns"] = ns,
                                       ["data"] = data
                                   };


            var content = BuildHttpJsonBody(json);
            var response = await RawStoreCustomJson(userId?.ToString() ?? "self", scopes, content);
            
            if (!response.IsSuccessStatusCode) {
                throw new Exception($"http failure response: {response.StatusCode} {response.ReasonPhrase}");
            }

            var responseStr = await response.Content.ReadAsStringAsync();
            return JObject.Parse(responseStr);
        }

        private Task<HttpResponseMessage> RawCreateUser(string accountId, HttpContent content) {
            return _client.PostAsync($"accounts/{accountId}/users", content);
        }

        /// <summary>
        /// Creates a new user with the <c>/api/v1/accounts/:account_id/users</c> endpoint.
        /// It is preferred to instead call <see cref="BuildNewUser"/> and <see cref="CreateUserBuilder.Post"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The new user.</returns>
        /// <exception cref="Exception">Thrown if the API returns a failing response code.</exception>
        /// <seealso cref="BuildNewUser"/>
        /// <seealso cref="CreateUserBuilder"/>
        public async Task<User> CreateUser(CreateUserBuilder builder) {
            var content = BuildHttpArguments(from kv in builder.Fields select (kv.Key, kv.Value));
            var response = await RawCreateUser(builder.AccountId, content);
            
            if (!response.IsSuccessStatusCode) {
                throw new Exception($"http failure response: {response.StatusCode} {response.ReasonPhrase}");
            }

            var responseStr = await response.Content.ReadAsStringAsync();
            var userModel = JsonConvert.DeserializeObject<UserModel>(responseStr);

            return new User(this, userModel);
        }

        /// <summary>
        /// Returns a <c>CreateUserBuilder</c> for creating a new user.
        /// </summary>
        /// <param name="accountId">The account to create the user under. <c>self</c> by default.</param>
        /// <returns>The builder.</returns>
        public CreateUserBuilder BuildNewUser(string accountId = "self") {
            return new CreateUserBuilder(this, accountId);
        }

        private Task<HttpResponseMessage> RawEditUser(string userId, HttpContent content) {
            return _client.PutAsync("users/" + userId, content);
        }

        /// <summary>
        /// Modify an existing user.
        /// The fields that can be modified are: <c>{name, short_name, sortable_name, time_zone, email, locale, title, bio}</c>.
        /// Corresponds to the API endpoint <c>PUT /api/v1/users/:id</c>
        /// </summary>
        /// <param name="fields">The set of field-value tuples to modify.</param>
        /// <param name="id">The id of the user. A null id is interpreted as <c>self</c>.</param>
        /// <returns>The edited user.</returns>
        /// <exception cref="Exception">Thrown if the API returns a failing response code.</exception>
        public async Task<User> EditUser(IEnumerable<ValueTuple<string, string>> fields, ulong? id = null) {
            var content = BuildHttpArguments(from kv in fields select ($"user[{kv.Item1}]", kv.Item2));
            var response = await RawEditUser(id?.ToString() ?? "self", content);
            
            if (!response.IsSuccessStatusCode) {
                throw new Exception($"http failure response: {response.StatusCode} {response.ReasonPhrase}");
            }

            var responseStr = await response.Content.ReadAsStringAsync();
            var userModel = JsonConvert.DeserializeObject<UserModel>(responseStr);

            return new User(this, userModel);
        }
        
        private Task<HttpResponseMessage> RawGetUserProfile(string userId) {
            return _client.GetAsync("users/" + userId + "/profile");
        }

        /// <summary>
        /// Get the profile of a user.
        /// </summary>
        /// <param name="id">The id of the user. A null id is interpreted as <c>self</c>.</param>
        /// <returns>The user profile.</returns>
        /// <exception cref="Exception">Thrown if the API returns a failing response code.</exception>
        public async Task<Profile> GetUserProfile(ulong? id = null) {
            var response = await RawGetUserProfile(id?.ToString() ?? "self");
            
            if (!response.IsSuccessStatusCode) {
                throw new Exception($"http failure response: {response.StatusCode} {response.ReasonPhrase}");
            }

            var responseStr = await response.Content.ReadAsStringAsync();
            var profileModel = JsonConvert.DeserializeObject<ProfileModel>(responseStr);

            return new Profile(this, profileModel);
        }

        private Task<HttpResponseMessage> RawGetUserDetails(string userId) {
            return _client.GetAsync("users/" + userId);
        }
        
        /// <summary>
        /// Get details for a user, including a non-comprehensive list of permissions.
        /// </summary>
        /// <param name="id">The id of the user. A null id is interpreted as <c>self</c>.</param>
        /// <returns>The User object.</returns>
        /// <exception cref="Exception">Thrown if the API returns a failing response code.</exception>
        public async Task<User> GetUserDetails(ulong? id = null) {
            var response = await RawGetUserDetails(id?.ToString() ?? "self");
            
            if (!response.IsSuccessStatusCode) {
                throw new Exception($"http failure response: {response.StatusCode} {response.ReasonPhrase}");
            }

            var responseStr = await response.Content.ReadAsStringAsync();
            var userModel = JsonConvert.DeserializeObject<UserModel>(responseStr);

            return new User(this, userModel);
        }

        private Task<HttpResponseMessage> RawGetListUsers(string searchTerm, 
                                                          string accountId,
                                                          string sort,
                                                          string order) {
            return _client.GetAsync("accounts/" + accountId + "/users" + BuildQueryString(
                                                                                        ("search_term", searchTerm),
                                                                                        ("sort", sort),
                                                                                        ("order", order)
                                                                                      ));
        }
        
        /// <summary>
        /// Get the list of users associated with the account, according to a search term.
        /// Corresponds to the API endpoint <c>GET /api/v1/accounts/:account_id/users</c>. 
        /// </summary>
        /// <param name="searchTerm">The partial name or full ID of the users to match.</param>
        /// <param name="accountId">The account to search. <c>self</c> by default.</param>
        /// <param name="sort">The column to sort results by. Allowed values are <c>username, email, sis_id, last_login</c>.</param>
        /// <param name="order">The order to sort the given column by. Allowed values are <c>asc, desc</c>.</param>
        /// <returns>The list of users found in the search.</returns>
        /// <exception cref="Exception">Thrown if the API returns a failing response code.</exception>
        public async Task<IEnumerable<User>> GetListUsers(string searchTerm,
                                                          string sort = null,
                                                          string order = null,
                                                          string accountId = "self") {
            var response = await RawGetListUsers(searchTerm, accountId, sort, order);
            if (!response.IsSuccessStatusCode) {
                throw new Exception($"http failure response: {response.StatusCode} {response.ReasonPhrase}");
            }

            var responseStr = await response.Content.ReadAsStringAsync();
            var userModels = JsonConvert.DeserializeObject<List<UserModel>>(responseStr);
            
            return from userModel in userModels
                   select new User(this, userModel);
        }
        
        
    }
}