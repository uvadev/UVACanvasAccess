using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UVACanvasAccess.Model.Quizzes {
    
    internal class QuizStatisticsModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("quiz_id")]
        public ulong QuizId { get; set; }
        
        [JsonProperty("multiple_attempts_exist")]
        public bool? MultipleAttemptsExist { get; set; }
        
        [JsonProperty("includes_all_versions")]
        public bool? IncludesAllVersions { get; set; }
        
        [JsonProperty("generated_at")]
        public DateTime? GeneratedAt { get; set; }
        
        [JsonProperty("url")]
        public string Url { get; set; }
        
        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }
        
        [JsonProperty("question_statistics")]
        public JToken QuestionStatistics { get; set; }
        
        [JsonProperty("submission_statistics")]
        public JToken SubmissionStatistics { get; set; }
        
        [JsonProperty("links")]
        public JToken Links { get; set; }
    }

    internal class QuizStatisticsListModel {
        
        [JsonProperty("quiz_statistics")]
        public IEnumerable<QuizStatisticsModel> QuizStatistics { get; set; }
        
        [JsonProperty("links")]
        public JToken Links { get; set; }
    }
}
