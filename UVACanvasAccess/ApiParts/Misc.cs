using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UVACanvasAccess.Model.Users;
using UVACanvasAccess.Structures.Users;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.ApiParts {
    
    public partial class Api {
        [PaginatedResponse]
        private Task<HttpResponseMessage> RawGetActivityStream(bool? onlyActiveCourses) {
            return _client.GetAsync("users/activity_stream" +
                                    BuildQueryString(("only_active_courses", onlyActiveCourses?.ToString())));
        }
        
        /// <summary>
        /// Gets the activity stream for the current user.
        /// </summary>
        /// <param name="onlyActiveCourses">If true, only entries for active courses will be returned.</param>
        /// <returns>The list of activity stream entries.</returns>
        public async Task<IEnumerable<ActivityStreamObject>> GetActivityStream(bool? onlyActiveCourses = null) {
            var response = await RawGetActivityStream(onlyActiveCourses);
            response.AssertSuccess();

            var models = await AccumulateDeserializePages<ActivityStreamObjectModel>(response);

            return from model in models
                   select ActivityStreamObject.FromModel(this, model);
        }
        
        public async IAsyncEnumerable<ActivityStreamObject> StreamActivityStream(bool? onlyActiveCourses = null) {
            var response = await RawGetActivityStream(onlyActiveCourses);
            response.AssertSuccess();

            await foreach (var model in StreamDeserializePages<ActivityStreamObjectModel>(response)) {
                yield return ActivityStreamObject.FromModel(this, model);
            }
        }

        private Task<HttpResponseMessage> RawGetActivityStreamSummary() {
            return _client.GetAsync("users/self/activity_stream/summary");
        }

        /// <summary>
        /// Returns the summary of activity stream entries for the current user.
        /// </summary>
        /// <returns>The map of activity stream types to their counts.</returns>
        public async Task<Dictionary<string, ActivityStreamSummaryEntry>> GetActivityStreamSummary() {
            var response = await RawGetActivityStreamSummary();
            response.AssertSuccess();

            var models = await AccumulateDeserializePages<ActivityStreamSummaryEntryModel>(response);
            
            var dic = new Dictionary<string, ActivityStreamSummaryEntry>();
            foreach (var model in models) {
                dic[model.Type] = new ActivityStreamSummaryEntry(model);
            }

            return dic;
        }

        private Task<HttpResponseMessage> RawGetUserPageViews(string userId, string startTime, string endTime) {
            return _client.GetAsync($"users/{userId}/page_views" +
                                    BuildQueryString(("start_time", startTime), ("end_time", endTime)));
        }

        /// <summary>
        /// Streams the user's page view history. Page views are returned in descending order; newest to oldest.
        /// </summary>
        /// <param name="userId">The id of the user. Defaults to <c>self</c>.</param>
        /// <param name="startTime">The beginning of the date-time range to retrieve page views from. Defaults to unbounded.</param>
        /// <param name="endTime">The end of the date-time range to retrieve page views from. Defaults to unbounded.</param>
        /// <returns>The stream of page views.</returns>
        public async IAsyncEnumerable<PageView> StreamUserPageViews(ulong? userId = null, 
                                                                    DateTime? startTime = null,
                                                                    DateTime? endTime = null) {
            var response = await RawGetUserPageViews(userId?.ToString() ?? "self", 
                                                     startTime == null ? null : JsonConvert.SerializeObject(startTime), 
                                                     endTime == null ? null : JsonConvert.SerializeObject(endTime));
            response.AssertSuccess();

            await foreach (var model in StreamDeserializePages<PageViewModel>(response)) {
                yield return new PageView(this, model);
            }
        }

        private Task<HttpResponseMessage> RawDeleteCustomJson(string userId, string scopes, string ns) {
            return _client.DeleteAsync($"users/{userId}/custom_data/{scopes}" + BuildQueryString(("ns", ns)));
        }

        /// <summary>
        /// Delete arbitrary user data stored with <see cref="StoreCustomJson"/>.
        /// Corresponds to the API endpoint <c>DELETE /api/v1/users/:user_id/custom_data(/*scope)</c>.
        /// </summary>
        /// <param name="ns">The namespace under which the data is stored.</param>
        /// <param name="scopes">The scope, and optionally subscopes, under which the data is stored. Omit scope to delete all custom data.</param>
        /// <param name="userId">The id of the user.</param>
        /// <returns>The JSON data that was deleted.</returns>
        /// <exception cref="Exception"></exception>
        /// <seealso cref="StoreCustomJson"/>
        public async Task<JObject> DeleteCustomJson(string ns, string scopes = "", ulong? userId = null) {
            var response = await RawDeleteCustomJson(userId?.ToString() ?? "self", scopes, ns);
            
            response.AssertSuccess();

            var responseStr = await response.Content.ReadAsStringAsync();
            return JObject.Parse(responseStr);
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
            
            response.AssertSuccess();

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

            response.AssertSuccess();

            var responseStr = await response.Content.ReadAsStringAsync();
            return JObject.Parse(responseStr);
        }
    }
}