using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UVACanvasAccess.Structures.Discussions;

namespace UVACanvasAccess.Builders {
    
    public class CreateDiscussionTopicBuilder {
        private readonly Api _api;
        public string Type { get; }
        public ulong HomeId { get; }
        public Dictionary<string, string> Fields { get; } = new Dictionary<string, string>();

        public CreateDiscussionTopicBuilder(Api api, string type, ulong homeId) {
            _api = api;
            Type = type;
            HomeId = homeId;
        }

        public CreateDiscussionTopicBuilder WithTitle(string title) {
            Fields["title"] = title;
            return this;
        }
        
        public CreateDiscussionTopicBuilder WithMessage(string message) {
            Fields["message"] = message;
            return this;
        }
        
        public CreateDiscussionTopicBuilder WithDiscussionType(string type) {
            Fields["discussion_type"] = type;
            return this;
        }
        
        public CreateDiscussionTopicBuilder WithPublishedStatus(bool published = true) {
            Fields["published"] = published.ToString().ToLower();
            return this;
        }
        
        public CreateDiscussionTopicBuilder WithDelayedPosting(DateTime postAt) {
            Fields["delayed_post_at"] = JsonConvert.SerializeObject(postAt);
            return this;
        }
        
        public CreateDiscussionTopicBuilder WithRatingsAllowed(bool allowed = true) {
            Fields["allow_rating"] = allowed.ToString().ToLower();
            return this;
        }
        
        public CreateDiscussionTopicBuilder WithScheduledLock(DateTime lockAt) {
            Fields["lock_at"] = JsonConvert.SerializeObject(lockAt);
            return this;
        }
        
        public CreateDiscussionTopicBuilder WithPodcastEnabled(bool enabled = true) {
            Fields["podcast_enabled"] = enabled.ToString().ToLower();
            return this;
        }
        
        public CreateDiscussionTopicBuilder WithPodcastIncludingStudents(bool enabled = true) {
            Fields["podcast_has_student_posts"] = enabled.ToString().ToLower();
            return this;
        }
        
        public CreateDiscussionTopicBuilder WithRequiredInitialPost(bool require = true) {
            Fields["require_initial_post"] = require.ToString().ToLower();
            return this;
        }
        
        public CreateDiscussionTopicBuilder WithAssignment(object o) {
            throw new NotImplementedException("not yet supported");
        }
        
        public CreateDiscussionTopicBuilder AsAnnouncement(bool isAnnouncement = true) {
            Fields["is_announcement"] = isAnnouncement.ToString().ToLower();
            return this;
        }
        
        public CreateDiscussionTopicBuilder AsPinned(bool pinned = true) {
            Fields["pinned"] = pinned.ToString().ToLower();
            return this;
        }
        
        public CreateDiscussionTopicBuilder WithPositionAfter(string id) {
            Fields["position_after"] = id;
            return this;
        }
        
        public CreateDiscussionTopicBuilder WithGroupCategory(uint id) {
            Fields["group_category_id"] = id.ToString();
            return this;
        }
        
        public CreateDiscussionTopicBuilder WithGraderRatingsOnly(bool graderRatingsOnly = true) {
            Fields["only_graders_can_rate"] = graderRatingsOnly.ToString().ToLower();
            return this;
        }
        
        public CreateDiscussionTopicBuilder WithSortingByRating(bool sortByRating = true) {
            Fields["sort_by_rating"] = sortByRating.ToString().ToLower();
            return this;
        }
        
        public CreateDiscussionTopicBuilder WithAttachment() {
            throw new NotImplementedException("not yet supported");
        }
        
        public CreateDiscussionTopicBuilder WithSpecificSections(IEnumerable<ulong> sections) {
            Fields["specific_sections"] = string.Join(",", from s in sections select s.ToString());
            return this;
        }

        public Task<DiscussionTopic> Post() {
            return _api.PostNewDiscussionTopic(this);
        }
    }
}