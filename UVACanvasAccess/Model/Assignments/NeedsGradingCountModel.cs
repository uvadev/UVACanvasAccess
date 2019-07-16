using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Assignments {
    
    internal class NeedsGradingCountModel {
        
        [JsonProperty("section_id")]
        public string SectionId { get; set; }
        
        [JsonProperty("needs_grading_count")]
        public uint NeedsGradingCount { get; set; }
    }
}