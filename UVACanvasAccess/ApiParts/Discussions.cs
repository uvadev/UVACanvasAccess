using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UVACanvasAccess.Builders;
using UVACanvasAccess.Model.Discussions;
using UVACanvasAccess.Structures.Discussions;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.ApiParts {
    
    public partial class Api {
        
        /// <summary>
        /// Categories of optional data that can be requested for inclusion within <see cref="DiscussionTopic"/> objects.
        /// </summary>
        [Flags]
        public enum DiscussionTopicIncludes {
            /// <summary>
            /// Include no optional data.
            /// </summary>
            Default = 0,
            /// <summary>
            /// Include relevant dates.
            /// </summary>
            [ApiRepresentation("all_dates")]
            AllDates = 1 << 0,
            /// <summary>
            /// Include section information.
            /// </summary>
            [ApiRepresentation("sections")]
            Sections = 1 << 1,
            /// <summary>
            /// Include quantitative section information.
            /// </summary>
            [ApiRepresentation("sections_user_count")]
            SectionsUserCount = 1 << 2,
            /// <summary>
            /// Include any overrides.
            /// </summary>
            [ApiRepresentation("overrides")]
            Overrides = 1 << 3
        }

        /// <summary>
        /// An ordering that can be applied to sets of <see cref="DiscussionTopic"/>.
        /// </summary>
        public enum DiscussionTopicOrdering {
            /// <summary>
            /// Order by the topic position.
            /// </summary>
            [ApiRepresentation("position")]
            Position,
            /// <summary>
            /// Order by recent activity.
            /// </summary>
            [ApiRepresentation("recent_activity")]
            RecentActivity,
            /// <summary>
            /// Order by the topic title.
            /// </summary>
            [ApiRepresentation("title")]
            Title
        }

        /// <summary>
        /// Scopes that <see cref="DiscussionTopic"/>s can be filtered by.
        /// </summary>
        [Flags]
        [PublicAPI]
        public enum DiscussionTopicScopes {
            /// <summary>
            /// The topic is locked.
            /// </summary>
            [ApiRepresentation("locked")]
            Locked = 1 << 0,
            /// <summary>
            /// The topic is unlocked.
            /// </summary>
            [ApiRepresentation("unlocked")]
            Unlocked = 1 << 1,
            /// <summary>
            /// The discussion is pinned to the top.
            /// </summary>
            [ApiRepresentation("pinned")]
            Pinned = 1 << 2, 
            /// <summary>
            /// The discussion is not pinned.
            /// </summary>
            [ApiRepresentation("unpinned")]
            Unpinned = 1 << 3
        }

        private Task<HttpResponseMessage> RawGetDiscussionTopic(string type, 
                                                                string baseId, 
                                                                string topicId, 
                                                                DiscussionTopicIncludes includes) {
            var url = $"{type}/{baseId}/discussion_topics/{topicId}" + BuildQueryString(includes.GetTuples().ToArray());
            return client.GetAsync(url);
        }

        /// <summary>
        /// Gets a single course discussion topic by its id.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="discussionId">The discussion id.</param>
        /// <param name="includes">Extra data to include in the result. See <see cref="DiscussionTopicIncludes"/>.</param>
        /// <returns>The discussion topic.</returns>
        public async Task<DiscussionTopic> GetCourseDiscussionTopic(ulong courseId, 
                                                                    ulong discussionId, 
                                                                    DiscussionTopicIncludes includes = DiscussionTopicIncludes.Default) {
            var response = await RawGetDiscussionTopic("courses",
                                                       courseId.ToString(),
                                                       discussionId.ToString(),
                                                       includes);
            response.AssertSuccess();

            var model = JsonConvert.DeserializeObject<DiscussionTopicModel>(await response.Content.ReadAsStringAsync());
            return new DiscussionTopic(this, model, DiscussionHome.Course, courseId);
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
                                                                  DiscussionTopicIncludes includes) {
            var url = $"{type}/{id}/discussion_topics";
            
            
            var args = new List<(string, string)> {
                                                      ("order_by", orderBy), 
                                                      ("scope", scopes?.GetString()), 
                                                      ("only_announcements", onlyAnnouncements?.ToShortString()), 
                                                      ("filter_by", filterBy), 
                                                      ("search_term", searchTerm), 
                                                      ("exclude_context_module_locked_topics", 
                                                       excludeContextModuleLockedTopics?.ToShortString())
                                                  };

            args.AddRange(includes.GetTuples());
            
            url += BuildQueryString(args.ToArray());

            return client.GetAsync(url);
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
        /// <param name="includes">Extra data to include in the result. See <see cref="DiscussionTopicIncludes"/>.</param>
        /// <returns>The list of discussion topics.</returns>
        /// <seealso cref="DiscussionTopicIncludes"/>
        /// <seealso cref="DiscussionTopicOrdering"/>
        /// <seealso cref="DiscussionTopicScopes"/>
        public async Task<IEnumerable<DiscussionTopic>> ListCourseDiscussionTopics(ulong id,
                                                                                   DiscussionTopicOrdering? orderBy = null,
                                                                                   DiscussionTopicScopes? scopes = null,
                                                                                   bool? onlyAnnouncements = null, 
                                                                                   bool filterByUnread = false,
                                                                                   string searchTerm = null,
                                                                                   bool? excludeContextModuleLockedTopics = null,
                                                                                   DiscussionTopicIncludes includes = DiscussionTopicIncludes.Default) {
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
                   select new DiscussionTopic(this, model, DiscussionHome.Course, id);
        }

        private Task<HttpResponseMessage> RawPostNewDiscussionTopic(string type,
                                                                    string baseId,
                                                                    HttpContent content) {
            var url = $"{type}/{baseId}/discussion_topics";

            return client.PostAsync(url, content);
        }

        internal async Task<DiscussionTopic> PostNewDiscussionTopic(CreateDiscussionTopicBuilder builder) {
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
        public CreateDiscussionTopicBuilder CreateCourseDiscussionTopic(ulong courseId) {
            return new CreateDiscussionTopicBuilder(this, "courses", courseId);
        }

        [PaginatedResponse]
        private Task<HttpResponseMessage> RawListDiscussionEntryReplies(string type,
                                                                        string baseId,
                                                                        string topicId,
                                                                        string entryId) {
            var url = $"{type}/{baseId}/discussion_topics/{topicId}/entries/{entryId}/replies";
            return client.GetAsync(url);
        }

        /// <summary>
        /// Gets the list of replies to a discussion entry.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="topicId">The topic id.</param>
        /// <param name="entryId">The discussion entry id.</param>
        /// <param name="type">(Optional; default = 'courses') The type of discussion entry.</param>
        /// <returns>The list of replies.</returns>
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
            return models.Select(model => new TopicReply(this, model));
        }

        private Task<HttpResponseMessage> RawPostDiscussionEntry(string type, 
                                                                 string baseId, 
                                                                 string topicId, 
                                                                 string body,
                                                                 byte[] attachment,
                                                                 string filePath) {
            var url = $"{type}/{baseId}/discussion_topics/{topicId}/entries";

            // Only if there is an attachment, the request must be a multipart, otherwise it must not.
            // This is undocumented.
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
                return client.PostAsync(url, dataContent);
            }

            var content = BuildHttpArguments(new[] {
                                                       ("message", body)
                                                   });
            return client.PostAsync(url, content);
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
            return new TopicEntry(this, model, DiscussionHome.Course, courseId, discussionId);
        }

        [PaginatedResponse]
        private Task<HttpResponseMessage> RawListTopicEntries(string type,
                                                              string baseId,
                                                              string topicId) {
            
            var url = $"{type}/{baseId}/discussion_topics/{topicId}/entries";

            return client.GetAsync(url);
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
                   select new TopicEntry(this, model, DiscussionHome.Course, courseId, topicId);
        }
        
        /// <summary>
        /// Gets a list of discussion topic entries for a group topic, given the group and topic IDs.
        /// </summary>
        /// <param name="groupId">The group id.</param>
        /// <param name="topicId">The topic id.</param>
        /// <returns>The list of <see cref="TopicEntry"/>.</returns>
        public async Task<IEnumerable<TopicEntry>> ListGroupDiscussionTopicEntries(ulong groupId,
                                                                                   ulong topicId) {
            var response = await RawListTopicEntries("groups", groupId.ToString(), topicId.ToString());
            response.AssertSuccess();

            var models = await AccumulateDeserializePages<TopicEntryModel>(response);
            
            return from model in models
                   select new TopicEntry(this, model, DiscussionHome.Group, groupId, topicId);
        }
    }
}