using Newtonsoft.Json;

namespace UVACanvasAccess.Model.OutcomeResults {
    
    internal class OutcomeRollupLinksModel {
        
        [JsonProperty("course")]
        public ulong? Course { get; set; }
        
        [JsonProperty("user")]
        public ulong? User { get; set; }
        
        [JsonProperty("section")]
        public ulong? Section { get; set; }
    }
}
