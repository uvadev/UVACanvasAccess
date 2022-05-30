using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UVACanvasAccess.Model.Pages;
using UVACanvasAccess.Structures.Pages;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {

        /// <summary>
        /// Keys by which a <see cref="Page"/> can be sorted.
        /// </summary>
        [PublicAPI]
        public enum PageSort {
            /// <summary>
            /// Sort by the page title/
            /// </summary>
            [ApiRepresentation("title")]
            Title,
            /// <summary>
            /// Sort by the page creation date.
            /// </summary>
            [ApiRepresentation("created_at")]
            CreatedAt,
            /// <summary>
            /// Sort by the page update date.
            /// </summary>
            [ApiRepresentation("updated_at")]
            UpdatedAt
        }

        private Task<HttpResponseMessage> RawListPages(string type, string id, string sort, string order, string search, bool? published) {
            return client.GetAsync($"{type}/{id}/pages" + BuildQueryString(("sort", sort), 
                                                                            ("order", order), 
                                                                            ("published", published?.ToShortString())));
        }

        /// <summary>
        /// Stream all pages for a course.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="sort">(Optional) Which field to sort results by.</param>
        /// <param name="order">(Optional) The order of sorted results; defaults to <see cref="Order.Ascending"/>.</param>
        /// <param name="searchTerm">(Optional) A search term.</param>
        /// <param name="published">(Optional) If true, only return published courses. If false, exclude published pages.</param>
        /// <returns>The pages.</returns>
        public async IAsyncEnumerable<Page> StreamCoursePages(ulong courseId,
                                                              PageSort? sort = null,
                                                              Order? order = null,
                                                              string searchTerm = null,
                                                              bool? published = null) {
            var response = await RawListPages("courses",
                                              courseId.ToString(),
                                              sort?.GetApiRepresentation(),
                                              order?.GetApiRepresentation(),
                                              searchTerm,
                                              published);
            
            await foreach (var page in StreamDeserializePages<PageModel>(response).Select(m => new Page(this, m, "courses", courseId))) {
                yield return page;
            }
        }

        private Task<HttpResponseMessage> RawGetPage(string type, string baseId, string url) {
            return client.GetAsync($"{type}/{baseId}/pages/{url}");
        }

        /// <summary>
        /// Get a single course page by its URL.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="url">The page URL.</param>
        /// <returns>The page.</returns>
        public async Task<Page> GetCoursePage(ulong courseId, string url) {
            var response = await RawGetPage("courses", courseId.ToString(), url).AssertSuccess();
            var model = JsonConvert.DeserializeObject<PageModel>(await response.Content.ReadAsStringAsync());
            return new Page(this, model, "courses", courseId);
        }

        private Task<HttpResponseMessage> RawGetFrontPage(string type, string id) {
            return client.GetAsync($"{type}/{id}/front_page");
        }

        /// <summary>
        /// Get a course's front page.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <returns>The page.</returns>
        public async Task<Page> GetCourseFrontPage(ulong courseId) {
            var response = await RawGetFrontPage("courses", courseId.ToString()).AssertSuccess();
            var model = JsonConvert.DeserializeObject<PageModel>(await response.Content.ReadAsStringAsync());
            return new Page(this, model, "courses", courseId);
        }

        private Task<HttpResponseMessage> RawListRevisions(string type, string id, string url) {
            return client.GetAsync($"{type}/{id}/pages/{url}/revisions");
        }

        /// <summary>
        /// Streams the history of course page revisions.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="url">The page URL.</param>
        /// <returns>The revision history.</returns>
        public async IAsyncEnumerable<PageRevision> StreamCoursePageRevisionHistory(ulong courseId, string url) {
            var response = await RawListRevisions("courses", courseId.ToString(), url);
            var models = StreamDeserializePages<PageRevisionModel>(response);

            await foreach (var pr in models.Select(pr => new PageRevision(this, pr, "courses", courseId, url))) {
                yield return pr;
            }
        }

        private Task<HttpResponseMessage> RawGetRevision(string type, 
                                                         string id,
                                                         string url, 
                                                         string revisionId,
                                                         bool? omitDetails) {
            return client.GetAsync($"{type}/{id}/pages/{url}/revisions/{revisionId}" + 
                                    BuildQueryString(("summary", omitDetails?.ToShortString())));
        }

        /// <summary>
        /// Returns a single course page revision.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="url">The page URL.</param>
        /// <param name="revisionId">The revision id.</param>
        /// <param name="omitDetails">(Optional) If true, omits page contents.</param>
        /// <returns>The page revision.</returns>
        public async Task<PageRevision> GetCoursePageRevision(ulong courseId, 
                                                              string url,
                                                              ulong revisionId,
                                                              bool? omitDetails = null) {
            var response = await RawGetRevision("courses", courseId.ToString(), url, revisionId.ToString(), omitDetails);

            var model = JsonConvert.DeserializeObject<PageRevisionModel>(await response.Content.ReadAsStringAsync());
            return new PageRevision(this, model, "courses", courseId, url);
        }
    }
}