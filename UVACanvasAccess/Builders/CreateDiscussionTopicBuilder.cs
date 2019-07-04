using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Structures.Discussions;

namespace UVACanvasAccess.Builders {
    
    /// <summary>
    /// A class used to create new DiscussionTopics using the builder pattern.
    /// When all desired fields are set, call <see cref="Post"/> to create the topic.
    /// </summary>
    public class CreateDiscussionTopicBuilder {
        private readonly Api _api;
        internal string Type { get; }
        internal ulong HomeId { get; }
        internal Dictionary<string, string> Fields { get; } = new Dictionary<string, string>();

        internal CreateDiscussionTopicBuilder(Api api, string type, ulong homeId) {
            _api = api;
            Type = type;
            HomeId = homeId;
        }

        /// <summary>
        /// The title.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public CreateDiscussionTopicBuilder WithTitle(string title) {
            Fields["title"] = title;
            return this;
        }
        
        
        /// <summary>
        /// The message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public CreateDiscussionTopicBuilder WithMessage(string message) {
            Fields["message"] = message;
            return this;
        }
        
        /// <summary>
        /// <c>The type of discussion.
        /// Defaults to side_comment if not value is given.
        /// Accepted values are 'side_comment', for discussions that only allow one level of nested comments,
        /// and 'threaded' for fully threaded discussions.</c>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public CreateDiscussionTopicBuilder WithDiscussionType(string type) {
            Fields["discussion_type"] = type;
            return this;
        }
        
        /// <summary>
        /// If the topic should be published or remain a draft.
        /// By default, if the current user is a teacher or a TA, the topic will remain a draft.
        /// Otherwise, the topic will be published.
        /// </summary>
        /// <param name="published"></param>
        /// <returns></returns>
        public CreateDiscussionTopicBuilder WithPublishedStatus(bool published = true) {
            Fields["published"] = published.ToString().ToLower();
            return this;
        }
        
        /// <summary>
        /// If set, the topic will not be published until this time.
        /// </summary>
        /// <param name="postAt"></param>
        /// <returns></returns>
        public CreateDiscussionTopicBuilder WithDelayedPosting(DateTime postAt) {
            Fields["delayed_post_at"] = JsonConvert.SerializeObject(postAt);
            return this;
        }
        
        /// <summary>
        /// Whether or not users are allowed to rate entries under this topic.
        /// </summary>
        /// <param name="allowed"></param>
        /// <returns></returns>
        public CreateDiscussionTopicBuilder WithRatingsAllowed(bool allowed = true) {
            Fields["allow_rating"] = allowed.ToString().ToLower();
            return this;
        }
        
        /// <summary>
        /// If set, the topic will lock itself at this time.
        /// </summary>
        /// <param name="lockAt"></param>
        /// <returns></returns>
        public CreateDiscussionTopicBuilder WithScheduledLock(DateTime lockAt) {
            Fields["lock_at"] = JsonConvert.SerializeObject(lockAt);
            return this;
        }
        
        /// <summary>
        /// If enabled, the topic will have a podcast feed.
        /// </summary>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public CreateDiscussionTopicBuilder WithPodcastEnabled(bool enabled = true) {
            Fields["podcast_enabled"] = enabled.ToString().ToLower();
            return this;
        }
        
        /// <summary>
        /// If enabled, the topic will have a podcast feed that includes student posts.
        /// </summary>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public CreateDiscussionTopicBuilder WithPodcastIncludingStudents(bool enabled = true) {
            Fields["podcast_has_student_posts"] = enabled.ToString().ToLower();
            return this;
        }
        
        /// <summary>
        /// If enabled, users cannot reply to other replies until they themselves have posted a reply.
        /// </summary>
        /// <param name="require"></param>
        /// <returns></returns>
        public CreateDiscussionTopicBuilder WithRequiredInitialPost(bool require = true) {
            Fields["require_initial_post"] = require.ToString().ToLower();
            return this;
        }
        
        /// <summary>
        /// Not yet supported.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public CreateDiscussionTopicBuilder WithAssignment(object o) {
            throw new NotImplementedException("not yet supported");
        }
        
        /// <summary>
        /// If enabled, this topic will appear in students' announcements section. 
        /// </summary>
        /// <param name="isAnnouncement"></param>
        /// <returns></returns>
        public CreateDiscussionTopicBuilder AsAnnouncement(bool isAnnouncement = true) {
            Fields["is_announcement"] = isAnnouncement.ToString().ToLower();
            return this;
        }
        
        /// <summary>
        /// If true, this topic will appear in students' pinned discussions section.
        /// </summary>
        /// <param name="pinned"></param>
        /// <returns></returns>
        public CreateDiscussionTopicBuilder AsPinned(bool pinned = true) {
            Fields["pinned"] = pinned.ToString().ToLower();
            return this;
        }
        
        /// <summary>
        /// By default, topics are sorted chronologically.
        /// If set, this topic will instead appear after a specific topic.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CreateDiscussionTopicBuilder WithPositionAfter(string id) {
            Fields["position_after"] = id;
            return this;
        }
        
        /// <summary>
        /// If set, this topic will become a group discussion.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CreateDiscussionTopicBuilder WithGroupCategory(uint id) {
            Fields["group_category_id"] = id.ToString();
            return this;
        }
        
        /// <summary>
        /// If set, only graders will be able to rate entries.
        /// </summary>
        /// <param name="graderRatingsOnly"></param>
        /// <returns></returns>
        public CreateDiscussionTopicBuilder WithGraderRatingsOnly(bool graderRatingsOnly = true) {
            Fields["only_graders_can_rate"] = graderRatingsOnly.ToString().ToLower();
            return this;
        }
        
        /// <summary>
        /// If set, entries will be sorted by rating.
        /// </summary>
        /// <param name="sortByRating"></param>
        /// <returns></returns>
        public CreateDiscussionTopicBuilder WithSortingByRating(bool sortByRating = true) {
            Fields["sort_by_rating"] = sortByRating.ToString().ToLower();
            return this;
        }
        
        /// <summary>
        /// Not yet supported.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public CreateDiscussionTopicBuilder WithAttachment() {
            throw new NotImplementedException("not yet supported");
        }
        
        /// <summary>
        /// If this topic is an announcement, lives in a course, and this is set, the topic will only appear to these
        /// specific sections.
        /// </summary>
        /// <param name="sections"></param>
        /// <returns></returns>
        public CreateDiscussionTopicBuilder WithSpecificSections(IEnumerable<ulong> sections) {
            Fields["specific_sections"] = string.Join(",", from s in sections select s.ToString());
            return this;
        }

        /// <summary>
        /// Creates the new discussion topic.
        /// </summary>
        /// <returns>The new discussion topic.</returns>
        public Task<DiscussionTopic> Post() {
            return _api.PostNewDiscussionTopic(this);
        }
    }
}