using System;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Quizzes {
    
    internal class QuizSubmissionTimeModel {
        
        [JsonProperty("end_at")]
        public DateTime? EndAt { get; set; }
        
        [JsonProperty("time_left")]
        public int? TimeLeft { get; set; }
    }
}
