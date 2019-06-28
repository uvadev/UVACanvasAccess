using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Assignments {
    
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class NeedsGradingCountModel {
        
        [JsonProperty("section_id")]
        public string SectionId { get; set; }
        
        [JsonProperty("needs_grading_count")]
        public uint NeedsGradingCount { get; set; }
    }
}