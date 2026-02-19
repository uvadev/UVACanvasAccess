using System;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Quizzes {
    
    internal class QuizExtensionModel {
        
        [JsonProperty("quiz_id")]
        public ulong QuizId { get; set; }
        
        [JsonProperty("user_id")]
        public ulong UserId { get; set; }
        
        [JsonProperty("extra_attempts")]
        public int? ExtraAttempts { get; set; }
        
        [JsonProperty("extra_time")]
        public int? ExtraTime { get; set; }
        
        [JsonProperty("manually_unlocked")]
        public bool? ManuallyUnlocked { get; set; }
        
        [JsonProperty("end_at")]
        public DateTime? EndAt { get; set; }
    }
}
