using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UVACanvasAccess.Model.Assignments;
using UVACanvasAccess.Model.Users;

namespace UVACanvasAccess.Model.Submissions {
    
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SubmissionModel {
        
        [JsonProperty("assignment_id")]
        public ulong AssignmentId { get; set; }
        
        [CanBeNull]
        [JsonProperty("assignment")]
        public AssignmentModel Assignment { get; set; }
        
        [CanBeNull]
        [JsonProperty("course")]
        public object Course { get; set; }
        
        [JsonProperty("attempt")]
        public uint? Attempt { get; set; }
        
        [CanBeNull]
        [JsonProperty("body")]
        public string Body { get; set; }
        
        [JsonProperty("grade")]
        public string Grade { get; set; }
        
        [JsonProperty("grade_matches_current_submission")]
        public bool? GradeMatchesCurrentSubmission { get; set; }
        
        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }
        
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
        public DateTime SubmittedAt { get; set; }
        
        [CanBeNull]
        [JsonProperty("url")]
        public string Url { get; set; }
        
        [JsonProperty("user_id")]
        public ulong UserId { get; set; }
        
        [JsonProperty("grader_id")]
        public ulong? GraderId { get; set; }
        
        [JsonProperty("graded_at")]
        public DateTime? GradedAt { get; set; }
        
        [JsonProperty("user")]
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
    }
}