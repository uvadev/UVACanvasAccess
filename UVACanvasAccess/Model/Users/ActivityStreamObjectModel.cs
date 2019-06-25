using System;
using Newtonsoft.Json;
using StatePrinting;

namespace UVACanvasAccess.Model.Users {
    
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ActivityStreamObjectModel {
        
        // General
        
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        
        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("message")]
        public string Message { get; set; }
        
        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("read_state")]
        public bool ReadState { get; set; }
        
        [JsonProperty("context_type")]
        public string ContextType { get; set; }
        
        [JsonProperty("course_id")]
        public ulong? CourseId { get; set; }
        
        [JsonProperty("group_id")]
        public ulong? GroupId { get; set; }
        
        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }
        
        //
        // Type = DiscussionTopic | Announcement
        
        [JsonProperty("total_root_discussion_entries")]
        public uint? TotalRootDiscussionEntries { get; set; }
        
        [JsonProperty("require_initial_post")]
        public bool? RequireInitialPost { get; set; }
        
        [JsonProperty("user_has_posted")]
        public bool? UserHasPosted { get; set; }
        
        [JsonProperty("root_discussion_entries")]
        public object RootDiscussionEntries { get; set; } // todo this class/model
        
        //
        // Type = DiscussionTopic
        
        [JsonProperty("discussion_topic_id")]
        public ulong? DiscussionTopicId { get; set; }
        
        //
        // Type = Announcement
        
        [JsonProperty("announcement_id")]
        public ulong? AnnouncementId  { get; set; }
        
        //
        // Type = Conversation
        
        [JsonProperty("conversation_id")]
        public ulong? ConversationId { get; set; }
        
        [JsonProperty("private")]
        public bool? Private { get; set; } 
        
        [JsonProperty("participant_count")]
        public uint? ParticipantCount { get; set; }
        
        //
        // Type = Message
        
        [JsonProperty("message_id")]
        public ulong? MessageId { get; set; }
        
        [JsonProperty("notification_category")]
        public string NotificationCategory { get; set; }
        
        //
        // Type = Submission
        // According to the API, "Returns an Submission with its Course and Assignment data."
        // ... whether that means this entire object will just be a SubmissionModel instead or if it will be inside
        //     is unclear and requires testing
        // todo
        
        // 
        // Type = Conference
        
        [JsonProperty("web_conference_id")]
        public ulong? WebConferenceId { get; set; }
        
        //
        // Type = Collaboration
        
        [JsonProperty("collaboration_id")]
        public ulong? CollaborationId { get; set; }
        
        //
        // Type = AssignmentRequest
        
        [JsonProperty("assignment_request_id")]
        public ulong? AssignmentRequestId { get; set; }
        
        private static readonly Stateprinter Printer = new Stateprinter();
        public override string ToString() {
            return Printer.PrintObject(this);
        }
    }
}