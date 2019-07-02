using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UVACanvasAccess.Builders;
using UVACanvasAccess.Model.Assignments;
using UVACanvasAccess.Model.Discussions;
using UVACanvasAccess.Model.Files;
using UVACanvasAccess.Model.Submissions;
using UVACanvasAccess.Model.Users;
using UVACanvasAccess.Structures.Assignments;
using UVACanvasAccess.Structures.Discussions;
using UVACanvasAccess.Structures.Users;
using UVACanvasAccess.Structures.Files;
using UVACanvasAccess.Structures.Submissions;
using UVACanvasAccess.Structures.Submissions.NewSubmission;
using UVACanvasAccess.Util;
using static UVACanvasAccess.Api.DiscussionTopicInclusions;
using static UVACanvasAccess.Structures.Discussions.DiscussionTopic.DiscussionHome;

namespace UVACanvasAccess {
    
    // ReSharper disable UnusedMethodReturnValue.Global
    public class Api : IDisposable {

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

        public void Dispose() {
            _client?.Dispose();
        }

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
        private string BuildQueryString(params ValueTuple<string, string>[] args) {
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

        /// <summary>
        /// Constructs a <c>x-www-form-url-encoded</c> <c>HttpContent</c> with the given key-value pairs.
        /// </summary>
        /// <param name="args">The set of key-value tuples.</param>
        /// <returns></returns>
        private HttpContent BuildHttpArguments(IEnumerable<ValueTuple<string, string>> args) {

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

        private MultipartFormDataContent BuildMultipartHttpArguments(IEnumerable<ValueTuple<string, string>> args) {
            var content = new MultipartFormDataContent();
            foreach (var (k, v) in args) {
                content.Add(new StringContent(v), k);
            }

            if (_masquerade != null) {
                content.Add(new StringContent(_masquerade.ToString()), "as_user_id");
            }

            return content;
        }
        
        /// <summary>
        /// Performs a file upload to any Canvas endpoint that accepts file uploads.
        /// This method should be called by a specific endpoint method and not directly.
        /// </summary>
        /// <param name="endpoint">The endpoint to upload to.</param>
        /// <param name="file">The data.</param>
        /// <param name="fileName">The file's name without its extension.</param>
        /// <param name="filePath">The file's path on the local machine.</param>
        /// <param name="parentFolderId">The id of the folder to place the file in.</param>
        /// <param name="parentFolderPath">The path of the folder to place the file in.</param>
        /// <param name="onDuplicate">How to handle a duplicate filename. Can be <c>overwrite</c> or <c>rename</c>.
        /// The default is <c>overwrite</c>. Not applicable in a context where files are not placed in folders.
        /// </param>
        /// <param name="contentType">The MIME type of the file. If absent, it will be determined by calling
        /// <see cref="MimeMapping.GetMimeMapping"/> on the filePath.
        /// </param>
        /// <returns>The uploaded file.</returns>
        /// <exception cref="Exception"></exception>
        /// <remarks>The request will fail if both <c>parentFolderId</c> and <c>parentFolderPath</c> are supplied.</remarks>
        /// <see href="https://canvas.instructure.com/doc/api/file.file_uploads.html"/>
        private async Task<CanvasFile> UploadFile(string endpoint, byte[] file, string fileName, string filePath, 
                                                  string parentFolderId = null, string parentFolderPath = null, 
                                                  string onDuplicate = null, string contentType = null) {

            if (contentType == null) {
                contentType = MimeMapping.GetMimeMapping(filePath);
            }

            var firstPostArgs = BuildHttpArguments(new[] {
                                                              ("name", fileName),
                                                              ("size", file.Length.ToString()),
                                                              ("content_type", contentType),
                                                              ("parent_folder_path", parentFolderPath),
                                                              ("parent_folder_id", parentFolderId),
                                                              ("on_duplicate", onDuplicate)
                                                          });

            var firstPostResponse = await _client.PostAsync(endpoint, firstPostArgs);

            firstPostResponse.AssertSuccess();

            var firstResponseJson = JObject.Parse(await firstPostResponse.Content.ReadAsStringAsync());
            var uploadUrl = firstResponseJson["upload_url"].ToString();
            var uploadParams = (JObject) firstResponseJson["upload_params"];

            var uploadParamsList = from kv in uploadParams.Properties()
                                   select (kv.Name, kv.Value.ToString());

            var secondPostArgs = BuildHttpArguments(uploadParamsList.Append(("file", fileName)));
            
            var bytesContent = new ByteArrayContent(file);
            bytesContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            
            var secondPostData = new MultipartFormDataContent {
                                                                  secondPostArgs,
                                                                  { bytesContent, fileName, filePath }
                                                              };
            
            var secondPostResponse = await _client.PostAsync(uploadUrl, secondPostData);

            secondPostResponse.AssertSuccess();


            CanvasFileModel model;
            if (secondPostResponse.StatusCode != HttpStatusCode.MovedPermanently) {
                model = JsonConvert.DeserializeObject<CanvasFileModel>(await secondPostResponse.Content.ReadAsStringAsync());
            } else {
                var thirdResponse = await _client.GetAsync(secondPostResponse.Headers.Location);
                thirdResponse.AssertSuccess();
                model = JsonConvert.DeserializeObject<CanvasFileModel>(await thirdResponse.Content.ReadAsStringAsync());
            }
            
            return new CanvasFile(this, model);

        }

        /// <summary>
        /// Uploads a file to the current user's personal files section.
        /// </summary>
        /// <param name="file">The data.</param>
        /// <param name="filePath">The URI of the file on the local system.
        /// Must include, at a minimum, the name and the extension.
        /// </param>
        /// <param name="parentFolderName"></param>
        /// <returns>The uploaded file.</returns>
        public Task<CanvasFile> UploadPersonalFile(byte[] file, 
                                                   string filePath,
                                                   string parentFolderName = null) {
            return UploadFile("users/self/files", 
                              file, 
                              Path.GetFileNameWithoutExtension(filePath), 
                              Path.GetFileName(filePath),
                              parentFolderPath: parentFolderName
                             );
        }

        private static HttpContent BuildHttpJsonBody(JObject json) {
            var content = new StringContent(json.ToString(), Encoding.Default, "application/json");
            return content;
        }

        private Task<HttpResponseMessage> RawSubmitAssignment([NotNull] string type,
                                                              [NotNull] string baseId,
                                                              [NotNull] string assignmentId,
                                                              [CanBeNull] string comment,
                                                              [NotNull] INewSubmissionContent submissionContent) {
            var url = $"{type}/{baseId}/assignments/{assignmentId}/submissions";

            var args = submissionContent.GetTuples()
                                        .Append(("submission[submission_type]", submissionContent.Type.GetApiRepresentation()))
                                        .Append(("comment[text_comment]", comment));
            
            return _client.PostAsync(url, BuildHttpArguments(args));
        }

        public async Task<Submission> SubmitCourseAssignment(ulong courseId,
                                                             ulong assignmentId,
                                                             [NotNull] INewSubmissionContent submissionContent,
                                                             string comment = null) {
            var response = await RawSubmitAssignment("courses",
                                                     courseId.ToString(),
                                                     assignmentId.ToString(),
                                                     comment,
                                                     submissionContent);
            response.AssertSuccess();

            var model = JsonConvert.DeserializeObject<SubmissionModel>(await response.Content.ReadAsStringAsync());
            return new Submission(this, model);
        }
        
        [Flags]
        public enum AssignmentInclusions {
            Default = 0,
            [ApiRepresentation("submission")]
            Submission = 1 << 0,
            [ApiRepresentation("assignment_visibility")]
            AssignmentVisibility = 1 << 1,
            [ApiRepresentation("overrides")]
            Overrides = 1 << 2,
            [ApiRepresentation("observed_users")]
            ObservedUsers = 1 << 3,
            [ApiRepresentation("all_dates")]
            AllDates = 1 << 4
        }

        public enum AssignmentBucket {
            [ApiRepresentation("past")]
            Past,
            [ApiRepresentation("overdue")]
            Overdue,
            [ApiRepresentation("undated")]
            Undated,
            [ApiRepresentation("ungraded")]
            Ungraded,
            [ApiRepresentation("ubsubmitted")]
            Unsubmitted,
            [ApiRepresentation("upcoming")]
            Upcoming,
            [ApiRepresentation("future")]
            Future
        }

        private Task<HttpResponseMessage> RawListAssignmentsGeneric(string url,
                                                                    AssignmentInclusions inclusions,
                                                                    [CanBeNull] string searchTerm,
                                                                    bool? overrideAssignmentDates,
                                                                    bool? needsGradingCountBySection, 
                                                                    AssignmentBucket? bucket,
                                                                    [CanBeNull] IEnumerable<ulong> assignmentIds,
                                                                    [CanBeNull] string orderBy) {
            var args = inclusions.GetTuples()
                                 .Append(("search_term", searchTerm))
                                 .Append(("override_assignment_dates", overrideAssignmentDates?.ToString().ToLower()))
                                 .Append(("needs_grading_count_by_section", needsGradingCountBySection?.ToString().ToLower()))
                                 .Append(("bucket", bucket?.GetApiRepresentation()))
                                 .Append(("order_by", orderBy));
            
            if (assignmentIds != null) {
                args = args.Concat(assignmentIds.Select(id => id.ToString())
                                                .Select(s => ("assignment_ids[]", s)));
            }

            return _client.GetAsync(url + BuildQueryString(args.ToArray()));
        }

        private Task<HttpResponseMessage> RawListCourseAssignments(string courseId,
                                                                   AssignmentInclusions inclusions,
                                                                   [CanBeNull] string searchTerm,
                                                                   bool? overrideAssignmentDates,
                                                                   bool? needsGradingCountBySection, 
                                                                   AssignmentBucket? bucket,
                                                                   [CanBeNull] IEnumerable<ulong> assignmentIds,
                                                                   [CanBeNull] string orderBy) {
            return RawListAssignmentsGeneric($"courses/{courseId}/assignments",
                                             inclusions,
                                             searchTerm,
                                             overrideAssignmentDates,
                                             needsGradingCountBySection,
                                             bucket,
                                             assignmentIds,
                                             orderBy);
        }
        
        private Task<HttpResponseMessage> RawListCourseGroupAssignments(string courseId,
                                                                        string assignmentGroupId,
                                                                        AssignmentInclusions inclusions,
                                                                        [CanBeNull] string searchTerm,
                                                                        bool? overrideAssignmentDates,
                                                                        bool? needsGradingCountBySection, 
                                                                        AssignmentBucket? bucket,
                                                                        [CanBeNull] IEnumerable<ulong> assignmentIds,
                                                                        [CanBeNull] string orderBy) {
            return RawListAssignmentsGeneric($"courses/{courseId}/assignment_groups/{assignmentGroupId}/assignments",
                                             inclusions,
                                             searchTerm,
                                             overrideAssignmentDates,
                                             needsGradingCountBySection,
                                             bucket,
                                             assignmentIds,
                                             orderBy);
        }

        public async Task<IEnumerable<Assignment>> ListCourseAssignments(ulong courseId,
                                                                         AssignmentInclusions inclusions = AssignmentInclusions.Default,
                                                                         string searchTerm = null,
                                                                         bool? overrideAssignmentDates = null,
                                                                         bool? needsGradingCountBySection = null,
                                                                         AssignmentBucket? bucket = null,
                                                                         IEnumerable<ulong> assignmentIds = null,
                                                                         string orderBy = null) {

            var response = await RawListCourseAssignments(courseId.ToString(),
                                                          inclusions,
                                                          searchTerm,
                                                          overrideAssignmentDates,
                                                          needsGradingCountBySection,
                                                          bucket,
                                                          assignmentIds,
                                                          orderBy);

            var models = await AccumulateDeserializePages<AssignmentModel>(response);

            return from model in models
                   select new Assignment(this, model);
        }

        private Task<HttpResponseMessage> RawGetSingleAssignment(string courseId,
                                                                 string assignmentId,
                                                                 AssignmentInclusions inclusions,
                                                                 bool? overrideAssignmentDates,
                                                                 bool? needsGradingCountBySection,
                                                                 bool? allDates) {
            var args = inclusions.GetTuples()
                                 .Append(("override_assignment_dates", overrideAssignmentDates?.ToString().ToLower()))
                                 .Append(("needs_grading_count_by_section", needsGradingCountBySection?.ToString().ToLower()))
                                 .Append(("all_dates", allDates?.ToString().ToLower()));
            
            var url = $"courses/{courseId}/assignments/{assignmentId}" + BuildQueryString(args.ToArray());

            return _client.GetAsync(url);
        }

        public async Task<Assignment> GetAssignment(ulong courseId, 
                                                    ulong assignmentId, 
                                                    AssignmentInclusions inclusions = AssignmentInclusions.Default,
                                                    bool? overrideAssignmentDates = null,
                                                    bool? needsGradingCountBySection = null,
                                                    bool? allDates = null) {
            
            var response = await RawGetSingleAssignment(courseId.ToString(),
                                                        assignmentId.ToString(),
                                                        inclusions,
                                                        overrideAssignmentDates,
                                                        needsGradingCountBySection,
                                                        allDates);
            response.AssertSuccess();
            
            var model = JsonConvert.DeserializeObject<AssignmentModel>(await response.Content.ReadAsStringAsync());
            return new Assignment(this, model);
        }

        [Flags]
        public enum DiscussionTopicInclusions {
            Default = 0,
            [ApiRepresentation("all_dates")]
            AllDates = 1 << 0,
            [ApiRepresentation("sections")]
            Sections = 1 << 1,
            [ApiRepresentation("sections_user_count")]
            SectionsUserCount = 1 << 2,
            [ApiRepresentation("overrides")]
            Overrides = 1 << 3
        }

        public enum DiscussionTopicOrdering {
            [ApiRepresentation("position")]
            Position,
            [ApiRepresentation("recent_activity")]
            RecentActivity,
            [ApiRepresentation("title")]
            Title
        }

        [Flags]
        public enum DiscussionTopicScopes {
            [ApiRepresentation("locked")]
            Locked = 1 << 0,
            [ApiRepresentation("unlocked")]
            Unlocked = 1 << 1,
            [ApiRepresentation("pinned")]
            Pinned = 1 << 2, 
            [ApiRepresentation("unpinned")]
            Unpinned = 1 << 3
        }

        private Task<HttpResponseMessage> RawGetDiscussionTopic(string type, 
                                                                string baseId, 
                                                                string topicId, 
                                                                DiscussionTopicInclusions inclusions) {
            var url = $"{type}/{baseId}/discussion_topics/{topicId}" + BuildQueryString(inclusions.GetTuples().ToArray());
            return _client.GetAsync(url);
        }

        /// <summary>
        /// Gets a single course discussion topic by its id.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="discussionId">The discussion id.</param>
        /// <param name="inclusions">Extra data to include in the result. See <see cref="DiscussionTopicInclusions"/>.</param>
        /// <returns>The discussion topic.</returns>
        public async Task<DiscussionTopic> GetCourseDiscussionTopic(ulong courseId, 
                                                                    ulong discussionId, 
                                                                    DiscussionTopicInclusions inclusions = Default) {
            var response = await RawGetDiscussionTopic("courses",
                                                       courseId.ToString(),
                                                       discussionId.ToString(),
                                                       inclusions);
            response.AssertSuccess();

            var model = JsonConvert.DeserializeObject<DiscussionTopicModel>(await response.Content.ReadAsStringAsync());
            return new DiscussionTopic(this, model, Course, courseId);
        }

        [PaginatedResponse]
        private Task<HttpResponseMessage> RawListDiscussionTopics(string type, 
                                                                  string id,
                                                                  string orderBy,
                                                                  DiscussionTopicScopes? scopes,
                                                                  bool? onlyAnnouncements, 
                                                                  string filterBy,
                                                                  string searchTerm,
                                                                  bool? excludeContextModuleLockedTopics,
                                                                  DiscussionTopicInclusions includes) {
            var url = $"{type}/{id}/discussion_topics";
            
            
            var args = new List<(string, string)> {
                                                      ("order_by", orderBy), 
                                                      ("scope", scopes?.GetString()), 
                                                      ("only_announcements", onlyAnnouncements?.ToString().ToLower()), 
                                                      ("filter_by", filterBy), 
                                                      ("search_term", searchTerm), 
                                                      ("exclude_context_module_locked_topics", 
                                                       excludeContextModuleLockedTopics?.ToString().ToLower())
                                                  };

            args.AddRange(includes.GetTuples());
            
            url += BuildQueryString(args.ToArray());

            return _client.GetAsync(url);
        }

        /// <summary>
        /// Returns the list of discussion topics for this course.
        /// </summary>
        /// <param name="id">The course id.</param>
        /// <param name="orderBy">The order of the list. <see cref="DiscussionTopicOrdering.Position"/> is the default.</param>
        /// <param name="scopes">Filter the results by those that match all of these states. No filtering by default.</param>
        /// <param name="onlyAnnouncements">Only return announcements.</param>
        /// <param name="filterByUnread">Only return unread by the current user.</param>
        /// <param name="searchTerm">An optional search term.</param>
        /// <param name="excludeContextModuleLockedTopics">For students, exclude topics that are locked by module progression.</param>
        /// <param name="includes">Extra data to include in the result. See <see cref="DiscussionTopicInclusions"/>.</param>
        /// <returns>The list of discussion topics.</returns>
        /// <seealso cref="DiscussionTopicInclusions"/>
        /// <seealso cref="DiscussionTopicOrdering"/>
        /// <seealso cref="DiscussionTopicScopes"/>
        public async Task<IEnumerable<DiscussionTopic>> ListCourseDiscussionTopics(ulong id,
                                                                                   DiscussionTopicOrdering? orderBy = null,
                                                                                   DiscussionTopicScopes? scopes = null,
                                                                                   bool? onlyAnnouncements = null, 
                                                                                   bool filterByUnread = false,
                                                                                   string searchTerm = null,
                                                                                   bool? excludeContextModuleLockedTopics = null,
                                                                                   DiscussionTopicInclusions includes = Default) {
            var response = await RawListDiscussionTopics("courses",
                                                         id.ToString(),
                                                         orderBy?.GetString(),
                                                         scopes,
                                                         onlyAnnouncements,
                                                         filterByUnread ? "unread" : null,
                                                         searchTerm,
                                                         excludeContextModuleLockedTopics,
                                                         includes);
            response.AssertSuccess();

            var models = await AccumulateDeserializePages<DiscussionTopicModel>(response);

            return from model in models
                   select new DiscussionTopic(this, model, Course, id);
        }

        private Task<HttpResponseMessage> RawPostNewDiscussionTopic(string type,
                                                                    string baseId,
                                                                    HttpContent content) {
            var url = $"{type}/{baseId}/discussion_topics";

            return _client.PostAsync(url, content);
        }

        public async Task<DiscussionTopic> PostNewDiscussionTopic(CreateDiscussionTopicBuilder builder) {
            var content = BuildMultipartHttpArguments(from kv in builder.Fields select (kv.Key, kv.Value));
            var response = await RawPostNewDiscussionTopic(builder.Type, builder.HomeId.ToString(), content);
            response.AssertSuccess();

            var model = JsonConvert.DeserializeObject<DiscussionTopicModel>(await response.Content.ReadAsStringAsync());
            return new DiscussionTopic(this, model, builder.Type, builder.HomeId);
        }

        /// <summary>
        /// Returns a <see cref="CreateDiscussionTopicBuilder"/> for building a new discussion topic for the course.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <returns>The builder.</returns>
        public CreateDiscussionTopicBuilder BuildNewCourseDiscussionTopic(ulong courseId) {
            return new CreateDiscussionTopicBuilder(this, "courses", courseId);
        }

        [PaginatedResponse]
        private Task<HttpResponseMessage> RawListDiscussionEntryReplies(string type,
                                                                        string baseId,
                                                                        string topicId,
                                                                        string entryId) {
            var url = $"{type}/{baseId}/discussion_topics/{topicId}/entries/{entryId}/replies";
            return _client.GetAsync(url);
        }

        public async Task<IEnumerable<TopicReply>> ListDiscussionEntryReplies(ulong courseId,
                                                                              ulong topicId,
                                                                              ulong entryId,
                                                                              string type = "courses") {
            var response = await RawListDiscussionEntryReplies(type,
                                                               courseId.ToString(),
                                                               topicId.ToString(),
                                                               entryId.ToString());
            response.AssertSuccess();

            var models = await AccumulateDeserializePages<TopicReplyModel>(response);

            return from model in models
                   select new TopicReply(this, model);
        }

        private Task<HttpResponseMessage> RawPostDiscussionEntry(string type, 
                                                                 string baseId, 
                                                                 string topicId, 
                                                                 string body,
                                                                 byte[] attachment,
                                                                 string filePath) {
            var url = $"{type}/{baseId}/discussion_topics/{topicId}/entries";

            // Only if there is an attachment, the request must be a multipart, otherwise it must not.
            // This is not documented by the API.
            if (attachment != null) {
                var bytesContent = new ByteArrayContent(attachment);
                bytesContent.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(filePath));

                var dataContent = new MultipartFormDataContent {
                                                                   {
                                                                       new StringContent(body), 
                                                                       "message"
                                                                   }, 
                                                                   {
                                                                       bytesContent, 
                                                                       "attachment", 
                                                                       Path.GetFileName(filePath)
                                                                   }
                                                           };
                return _client.PostAsync(url, dataContent);
            }

            var content = BuildHttpArguments(new[] {
                                                       ("message", body)
                                                   });
            return _client.PostAsync(url, content);
        }

        /// <summary>
        /// Creates a new entry in the discussion topic.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="discussionId">The discussion id.</param>
        /// <param name="messageBody">The message body to post.</param>
        /// <param name="attachment">(Optional) The attachment to post.</param>
        /// <param name="filePath">The filename with its extension, required if uploading an attachment.</param>
        /// <returns>The entry.</returns>
        public async Task<TopicEntry> PostCourseDiscussionEntry(ulong courseId, 
                                                            ulong discussionId,
                                                            string messageBody,
                                                            byte[] attachment = null,
                                                            string filePath = null) {
            
            var response = await RawPostDiscussionEntry("courses", courseId.ToString(),
                                                        discussionId.ToString(),
                                                        messageBody,
                                                        attachment,
                                                        filePath);
            response.AssertSuccess();

            var model = JsonConvert.DeserializeObject<TopicEntryModel>(await response.Content.ReadAsStringAsync());
            return new TopicEntry(this, model, Course, courseId, discussionId);
        }

        [PaginatedResponse]
        private Task<HttpResponseMessage> RawListTopicEntries(string type,
                                                              string baseId,
                                                              string topicId) {
            
            var url = $"{type}/{baseId}/discussion_topics/{topicId}/entries";

            return _client.GetAsync(url);
        }

        /// <summary>
        /// Returns the list of discussion topic entries for the course topic.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="topicId">The course topic.</param>
        /// <returns>The list of discussion topic entries.</returns>
        public async Task<IEnumerable<TopicEntry>> ListCourseDiscussionTopicEntries(ulong courseId,
                                                                                    ulong topicId) {
            var response = await RawListTopicEntries("courses", courseId.ToString(), topicId.ToString());
            response.AssertSuccess();

            var models = await AccumulateDeserializePages<TopicEntryModel>(response);
            
            return from model in models
                   select new TopicEntry(this, model, Course, courseId, topicId);
        }

        [PaginatedResponse]
        private Task<HttpResponseMessage> RawGetActivityStream(bool? onlyActiveCourses) {
            return _client.GetAsync("/api/v1/users/activity_stream" +
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

        private Task<HttpResponseMessage> RawGetActivityStreamSummary() {
            return _client.GetAsync("/api/v1/users/self/activity_stream/summary");
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
            return _client.GetAsync($"/api/v1/users/{userId}/page_views" +
                                    BuildQueryString(("start_time", startTime), ("end_time", endTime)));
        }

        /// <summary>
        /// Returns the user's page view history. Page views are returned in descending order; newest to oldest.
        /// </summary>
        /// <param name="userId">The id of the user. Defaults to <c>self</c>.</param>
        /// <param name="startTime">The beginning of the date-time range to retrieve page views from. Defaults to unbounded.</param>
        /// <param name="endTime">The end of the date-time range to retrieve page views from. Defaults to unbounded.</param>
        /// <returns>The collection of page views.</returns>
        public async Task<IEnumerable<PageView>> GetUserPageViews(ulong? userId = null, 
                                                                  DateTime? startTime = null,
                                                                  DateTime? endTime = null) {
            var response = await RawGetUserPageViews(userId?.ToString() ?? "self", 
                                                     startTime == null ? null : JsonConvert.SerializeObject(startTime), 
                                                     endTime == null ? null : JsonConvert.SerializeObject(endTime));
            response.AssertSuccess();

            var models = await AccumulateDeserializePages<PageViewModel>(response);

            return from model in models
                   select new PageView(this, model);
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
        public async Task<User> GetUserDetails(ulong? id = null) {
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
            response.AssertSuccess();

            var userModels = await AccumulateDeserializePages<UserModel>(response);

            return from userModel in userModels
                   select new User(this, userModel);
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
            
            while (response.Headers.TryGetValues("Link", out var linkValues)) {
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
    }
}