using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UVACanvasAccess.Model.Quizzes {
    
    internal class QuizSubmissionEventModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }
        
        [JsonProperty("event_type")]
        public string EventType { get; set; }
        
        [JsonProperty("event_data")]
        public JToken EventData { get; set; }
    }

    internal class QuizSubmissionEventListModel {
        
        [JsonProperty("quiz_submission_events")]
        public IEnumerable<QuizSubmissionEventModel> QuizSubmissionEvents { get; set; }
    }
}
