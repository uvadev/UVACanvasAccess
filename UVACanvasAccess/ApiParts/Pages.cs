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
            return _client.GetAsync($"{type}/{id}/pages" + BuildQueryString(("sort", sort), 
                                                                            ("order", order), 
                                                                            ("published", published?.ToShortString())));
        }

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
            return _client.GetAsync($"{type}/{baseId}/pages/{url}");
        }

        public async Task<Page> GetCoursePage(ulong courseId, string url) {
            var response = await RawGetPage("courses", courseId.ToString(), url).AssertSuccess();
            var model = JsonConvert.DeserializeObject<PageModel>(await response.Content.ReadAsStringAsync());
            return new Page(this, model, "courses", courseId);
        }

        private Task<HttpResponseMessage> RawGetFrontPage(string type, string id) {
            return _client.GetAsync($"{type}/{id}/front_page");
        }

        public async Task<Page> GetCourseFrontPage(ulong courseId) {
            var response = await RawGetFrontPage("courses", courseId.ToString()).AssertSuccess();
            var model = JsonConvert.DeserializeObject<PageModel>(await response.Content.ReadAsStringAsync());
            return new Page(this, model, "courses", courseId);
        }

        private Task<HttpResponseMessage> RawListRevisions(string type, string id, string url) {
            return _client.GetAsync($"{type}/{id}/pages/{url}/revisions");
        }

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
            return _client.GetAsync($"{type}/{id}/pages/{url}/revisions/{revisionId}" + 
                                    BuildQueryString(("summary", omitDetails?.ToShortString())));
        }

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