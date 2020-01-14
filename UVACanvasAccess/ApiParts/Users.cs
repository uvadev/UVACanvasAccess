using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UVACanvasAccess.Builders;
using UVACanvasAccess.Model.Assignments;
using UVACanvasAccess.Model.ToDos;
using UVACanvasAccess.Model.Users;
using UVACanvasAccess.Structures.Assignments;
using UVACanvasAccess.Structures.ToDos;
using UVACanvasAccess.Structures.Users;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.ApiParts {
    
    public partial class Api {
        private Task<HttpResponseMessage> RawCreateUser(string accountId, HttpContent content) {
            return _client.PostAsync($"accounts/{accountId}/users", content);
        }
        
        internal async Task<User> PostCreateUser(CreateUserBuilder builder) {
            var content = BuildHttpArguments(from kv in builder.Fields select (kv.Key, kv.Value));
            var response = await RawCreateUser(builder.AccountId, content);
            
            response.AssertSuccess();

            var responseStr = await response.Content.ReadAsStringAsync();
            var userModel = JsonConvert.DeserializeObject<UserModel>(responseStr);

            return new User(this, userModel);
        }

        /// <summary>
        /// Returns a <c>CreateUserBuilder</c> for creating a new user.
        /// </summary>
        /// <param name="accountId">The account to create the user under. <c>self</c> by default.</param>
        /// <returns>The builder.</returns>
        public CreateUserBuilder CreateUser(string accountId = "self") {
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
            
            response.AssertSuccess();

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
            
            response.AssertSuccess();

            var responseStr = await response.Content.ReadAsStringAsync();
            var profileModel = JsonConvert.DeserializeObject<ProfileModel>(responseStr);

            return new Profile(this, profileModel);
        }

        private Task<HttpResponseMessage> RawGetUserDetails(string userId) {
            return _client.GetAsync("users/" + userId + BuildQueryString());
        }
        
        /// <summary>
        /// Get details for a user, including a non-comprehensive list of permissions.
        /// </summary>
        /// <param name="id">The id of the user. A null id is interpreted as <c>self</c>.</param>
        /// <returns>The User object.</returns>
        /// <exception cref="Exception">Thrown if the API returns a failing response code.</exception>
        public async Task<User> GetUser(ulong? id = null) {
            var response = await RawGetUserDetails(id?.ToString() ?? "self");
            
            response.AssertSuccess();

            var responseStr = await response.Content.ReadAsStringAsync();
            var userModel = JsonConvert.DeserializeObject<UserModel>(responseStr);

            return new User(this, userModel);
        }

        [PaginatedResponse]
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
        /// Get the list of users associated with the account.
        /// Corresponds to the API endpoint <c>GET /api/v1/accounts/:account_id/users</c>. 
        /// </summary>
        /// <param name="searchTerm">The partial name or full ID of the users to match.</param>
        /// <param name="accountId">The account to search. <c>self</c> by default.</param>
        /// <param name="sort">The column to sort results by. Allowed values are <c>username, email, sis_id, last_login</c>.</param>
        /// <param name="order">The order to sort the given column by. Allowed values are <c>asc, desc</c>.</param>
        /// <returns>The list of users found in the search.</returns>
        /// <exception cref="Exception">Thrown if the API returns a failing response code.</exception>
        public async Task<IEnumerable<User>> ListUsers(string searchTerm = null,
                                                       string sort = null,
                                                       string order = null,
                                                       string accountId = "self") {
            var response = await RawGetListUsers(searchTerm, accountId, sort, order);

            var userModels = await AccumulateDeserializePages<UserModel>(response);

            return from userModel in userModels
                   select new User(this, userModel);
        }

        /// <summary>
        /// Stream the list of users associated with the account.
        /// Corresponds to the API endpoint <c>GET /api/v1/accounts/:account_id/users</c>. 
        /// </summary>
        /// <param name="searchTerm">The partial name or full ID of the users to match.</param>
        /// <param name="accountId">The account to search. <c>self</c> by default.</param>
        /// <param name="sort">The column to sort results by. Allowed values are <c>username, email, sis_id, last_login</c>.</param>
        /// <param name="order">The order to sort the given column by. Allowed values are <c>asc, desc</c>.</param>
        /// <returns>The stream of users found in the search.</returns>
        /// <exception cref="Exception">Thrown if the API returns a failing response code.</exception>
        public async IAsyncEnumerable<User> StreamUsers(string searchTerm = null,
                                                        string sort = null,
                                                        string order = null,
                                                        string accountId = "self") {
            var response = await RawGetListUsers(searchTerm, accountId, sort, order);

            var users = StreamDeserializePages<UserModel>(response)
                       .Select(m => new User(this, m));

            await foreach (var user in users) {
                yield return user;
            }
        }

        /// <summary>
        /// Delete a user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="accountId">The account the user is under. <c>self</c> by default.</param>
        /// <returns>The deleted user.</returns>
        public async Task<User> DeleteUser(ulong userId, ulong? accountId = null) {
            var response = await _client.DeleteAsync($"accounts/{accountId?.ToString() ?? "self"}/users/{userId}");
            return new User(this, JsonConvert.DeserializeObject<UserModel>(await response.Content.ReadAsStringAsync()));
        }

        /// <summary>
        /// Stream the current user's to-do items.
        /// </summary>
        /// <param name="includeUngradedQuizzes">(Optional) If true, include ungraded quizzes.</param>
        /// <returns>The stream of to-do items.</returns>
        public async IAsyncEnumerable<ToDoItem> StreamToDoItems(bool includeUngradedQuizzes = false) {
            (string, string)[] args = includeUngradedQuizzes switch {
                true => new[] {("include[]", "ungraded_quizzes")},
                _    => new (string, string)[] { }
            };

            var response = await _client.GetAsync("users/self/todo" + BuildQueryString(args));
            await foreach (var model in StreamDeserializePages<ToDoItemModel>(response)) {
                yield return ToDoItem.NewToDoItem(this, model);
            }
        }

        /// <summary>
        /// Stream a user's past-due assignments which have no submissions.
        /// </summary>
        /// <param name="userId">(Optional) The user id. <c>Self</c> by default.</param>
        /// <param name="submittableOnly">(Optional) Only return assignments which still accept submissions.</param>
        /// <param name="includePlannerOverrides">(Optional) Include planner overrides in the assignments.</param>
        /// <param name="includeCourse">(Optional) Include courses in the assignments.</param>
        /// <returns>The stream of assignments.</returns>
        /// <remarks>If not calling this method on <c>self</c>, the caller must be an admin or observer.</remarks>
        public async IAsyncEnumerable<Assignment> StreamMissingAssignments(ulong? userId = null, 
                                                                           bool submittableOnly = false,
                                                                           bool includePlannerOverrides = false,
                                                                           bool includeCourse = false) {
            (string, string)[] args = {
                ("filter[]", submittableOnly ? "submittable" : null),
                ("include[]", includePlannerOverrides ? "planner_overrides" : null),
                ("include[]", includeCourse ? "course" : null)
            };

            var u = userId != null ? userId.ToString() : "self";
            var response = await _client.GetAsync($"users/{u}/missing_submissions" + BuildDuplicateKeyQueryString(args));
            await foreach (var model in StreamDeserializePages<AssignmentModel>(response)) {
                yield return new Assignment(this, model);
            }
        }

        internal async Task IgnoreToDoItem(ToDoItem item, bool permanent) {
            if (permanent) {
                await _client.GetAsync(item.PermanentIgnoreUrl);
            } else {
                await _client.GetAsync(item.IgnoreUrl);
            }
        }
    }
}