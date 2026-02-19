using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Quizzes {
    
    internal class QuizPermissionsModel {
        
        [JsonProperty("read")]
        public bool? Read { get; set; }
        
        [JsonProperty("submit")]
        public bool? Submit { get; set; }
        
        [JsonProperty("create")]
        public bool? Create { get; set; }
        
        [JsonProperty("manage")]
        public bool? Manage { get; set; }
        
        [JsonProperty("read_statistics")]
        public bool? ReadStatistics { get; set; }
        
        [JsonProperty("review_grades")]
        public bool? ReviewGrades { get; set; }
        
        [JsonProperty("update")]
        public bool? Update { get; set; }
    }
}
