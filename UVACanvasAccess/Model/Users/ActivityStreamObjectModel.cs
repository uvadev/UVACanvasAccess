using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UVACanvasAccess.Model.Assignments;
using UVACanvasAccess.Model.Courses;
using UVACanvasAccess.Model.Submissions;
using UVACanvasAccess.Util;


namespace UVACanvasAccess.Model.Users {
    
    internal class ActivityStreamObjectModel {
        
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
        
        
        
        [JsonProperty("assignment_id")]
        public ulong? AssignmentId { get; set; }
        
        [CanBeNull]
        [JsonProperty("assignment")]
        public AssignmentModel Assignment { get; set; }
        
        [CanBeNull]
        [JsonProperty("course")]
        public CourseModel Course { get; set; }
        
        [JsonProperty("attempt")]
        public uint? Attempt { get; set; }
        
        [CanBeNull]
        [JsonProperty("body")]
        public string Body { get; set; }
        
        [JsonProperty("grade")]
        public string Grade { get; set; }
        
        [JsonProperty("grade_matches_current_submission")]
        public bool? GradeMatchesCurrentSubmission { get; set; }

        [JsonProperty("preview_url")]
        public string PreviewUrl { get; set; }
        
        [JsonProperty("score")]
        public decimal? Score { get; set; }
        
        [CanBeNull]
        [JsonProperty("submission_comments")]
        public IEnumerable<SubmissionCommentModel> SubmissionComments { get; set; }
        
        [JsonProperty("submission_type")]
        public string SubmissionType { get; set; }
        
        [JsonProperty("submitted_at")]
        public DateTime? SubmittedAt { get; set; }
        
        [CanBeNull]
        [JsonProperty("url")]
        public string Url { get; set; }
        
        [JsonProperty("user_id")]
        public ulong? UserId { get; set; }
        
        [JsonProperty("grader_id")]
        [Enigmatic]
        public long? GraderId { get; set; } // why can this be negative???
        
        [JsonProperty("graded_at")]
        public DateTime? GradedAt { get; set; }
        
        [JsonProperty("user")]
        [CanBeNull]
        public UserModel User { get; set; }
        
        [JsonProperty("late")]
        public bool? Late { get; set; }
        
        [JsonProperty("assignment_visible")]
        public bool? AssignmentVisible { get; set; }
        
        [JsonProperty("excused")]
        public bool? Excused { get; set; }
        
        [JsonProperty("missing")]
        public bool? Missing { get; set; }
        
        [JsonProperty("late_policy_status")]
        public string LatePolicyStatus { get; set; }
        
        [JsonProperty("points_deducted")]
        public double? PointsDeducted { get; set; }
        
        [JsonProperty("seconds_late")]
        public double? SecondsLate { get; set; }
        
        [JsonProperty("workflow_state")]
        public string WorkflowState { get; set; }

        [JsonProperty("extra_attempts")]
        public uint? ExtraAttempts { get; set; }
        
        [CanBeNull]
        [JsonProperty("anonymous_id")]
        public string AnonymousId { get; set; }
        
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
        
        
    }
}