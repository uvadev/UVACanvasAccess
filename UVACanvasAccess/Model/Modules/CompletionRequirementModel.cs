using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Modules {
    
    internal class CompletionRequirementModel {
        
        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("min_score")]
        public uint? MinScore { get; set; }
        
        [JsonProperty("completed")]
        public bool? Completed { get; set; }
    }
}
