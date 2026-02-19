using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UVACanvasAccess.Model.Assignments;

namespace UVACanvasAccess.Model.Quizzes {
    
    internal class QuizModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }
        
        [JsonProperty("mobile_url")]
        public string MobileUrl { get; set; }
        
        [JsonProperty("preview_url")]
        public string PreviewUrl { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("quiz_type")]
        public string QuizType { get; set; }
        
        [JsonProperty("assignment_group_id")]
        public ulong? AssignmentGroupId { get; set; }
        
        [JsonProperty("time_limit")]
        public decimal? TimeLimit { get; set; }
        
        [JsonProperty("shuffle_answers")]
        public bool? ShuffleAnswers { get; set; }
        
        [CanBeNull] 
        [JsonProperty("hide_results")]
        public string HideResults { get; set; }
        
        [JsonProperty("show_correct_answers")]
        public bool? ShowCorrectAnswers { get; set; }
        
        [JsonProperty("show_correct_answers_last_attempt")]
        public bool? ShowCorrectAnswersLastAttempt { get; set; }
        
        [JsonProperty("show_correct_answers_at")]
        public DateTime? ShowCorrectAnswersAt { get; set; }
        
        [JsonProperty("hide_correct_answers_at")]
        public DateTime? HideCorrectAnswersAt { get; set; }
        
        [JsonProperty("one_time_results")]
        public bool? OneTimeResults { get; set; }
        
        [CanBeNull] 
        [JsonProperty("scoring_policy")]
        public string ScoringPolicy { get; set; }
        
        [JsonProperty("allowed_attempts")]
        public int? AllowedAttempts { get; set; }
        
        [JsonProperty("one_question_at_a_time")]
        public bool? OneQuestionAtATime { get; set; }
        
        [JsonProperty("question_count")]
        public uint? QuestionCount { get; set; }
        
        [JsonProperty("points_possible")]
        public decimal? PointsPossible { get; set; }
        
        [JsonProperty("cant_go_back")]
        public bool? CantGoBack { get; set; }
        
        [CanBeNull]
        [JsonProperty("access_code")]
        public string AccessCode { get; set; }
        
        [CanBeNull] 
        [JsonProperty("ip_filter")]
        public string IpFilter { get; set; }
        
        [JsonProperty("due_at")]
        public DateTime? DueAt { get; set; }
        
        [JsonProperty("lock_at")]
        public DateTime? LockAt { get; set; }
        
        [JsonProperty("unlock_at")]
        public DateTime? UnlockAt { get; set; }
        
        [JsonProperty("published")]
        public bool? Published { get; set; }
        
        [JsonProperty("unpublishable")]
        public bool? Unpublishable { get; set; }
        
        [JsonProperty("locked_for_user")]
        public bool? LockedForUser { get; set; }
        
        [CanBeNull]
        [JsonProperty("lock_info")]
        public LockInfoModel LockInfo { get; set; }
        
        [CanBeNull]
        [JsonProperty("lock_explanation")]
        public string LockExplanation { get; set; }
        
        [CanBeNull]
        [JsonProperty("speedgrader_url")]
        public string SpeedGraderUrl { get; set; }
        
        [JsonProperty("quiz_extensions_url")]
        public string QuizExtensionsUrl { get; set; }
        
        [CanBeNull]
        [JsonProperty("permissions")]
        public QuizPermissionsModel Permissions { get; set; }
        
        [CanBeNull]
        [JsonProperty("all_dates")]
        public IEnumerable<AssignmentDateModel> AllDates { get; set; } 
        
        [JsonProperty("version_number")]
        public uint? VersionNumber { get; set; }
        
        [CanBeNull]
        [JsonProperty("question_types")]
        public IEnumerable<string> QuestionTypes { get; set; }
        
        [JsonProperty("anonymous_submissions")]
        public bool? AnonymousSubmissions { get; set; }
    }
}
