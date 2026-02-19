using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Quizzes {
    
    internal class QuizIpFilterModel {
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("account_id")]
        public ulong? AccountId { get; set; }
        
        [JsonProperty("filter")]
        public string Filter { get; set; }
    }
}
