using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UVACanvasAccess.Model.Submissions;
using UVACanvasAccess.Model.Users;

namespace UVACanvasAccess.Model.Quizzes {
    
    internal class QuizSubmissionModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("quiz_id")]
        public ulong QuizId { get; set; }
        
        [JsonProperty("user_id")]
        public ulong UserId { get; set; }
        
        [JsonProperty("submission_id")]
        public ulong SubmissionId { get; set; }
        
        [JsonProperty("score")]
        public decimal? Score { get; set; }
        
        [JsonProperty("kept_score")]
        public decimal? KeptScore { get; set; }
        
        [JsonProperty("started_at")]
        public DateTime? StartedAt { get; set; }
        
        [JsonProperty("finished_at")]
        public DateTime? FinishedAt { get; set; }
        
        [JsonProperty("end_at")]
        public DateTime? EndAt { get; set; }
        
        [JsonProperty("attempt")]
        public uint? Attempt { get; set; }
        
        [JsonProperty("extra_attempts")]
        public uint? ExtraAttempts { get; set; }
        
        [JsonProperty("extra_time")]
        public uint? ExtraTime { get; set; }
        
        [JsonProperty("manually_unlocked")]
        public bool? ManuallyUnlocked { get; set; }
        
        [JsonProperty("time_spent")]
        public uint? TimeSpent { get; set; }
        
        [JsonProperty("score_before_regrade")]
        public decimal? ScoreBeforeRegrade { get; set; }
        
        [JsonProperty("times_graded")]
        public uint? TimesGraded { get; set; }
        
        [JsonProperty("workflow_state")]
        public string WorkflowState { get; set; }
        
        [JsonProperty("validation_token")]
        public string ValidationToken { get; set; }
        
        [JsonProperty("quiz_version")]
        public uint? QuizVersion { get; set; }
        
        [JsonProperty("quiz_points_possible")]
        public decimal? QuizPointsPossible { get; set; }
        
        [JsonProperty("quiz_submitted_at")]
        public DateTime? QuizSubmittedAt { get; set; }
        
        [JsonProperty("user")]
        public UserDisplayModel User { get; set; }
        
        [JsonProperty("quiz")]
        public QuizModel Quiz { get; set; }
        
        [JsonProperty("submission")]
        public SubmissionModel Submission { get; set; }
        
        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }
    }

    internal class QuizSubmissionListModel {
        
        [JsonProperty("quiz_submissions")]
        public IEnumerable<QuizSubmissionModel> QuizSubmissions { get; set; }
    }
}
