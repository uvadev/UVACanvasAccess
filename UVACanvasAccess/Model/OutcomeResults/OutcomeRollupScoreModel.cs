using Newtonsoft.Json;

namespace UVACanvasAccess.Model.OutcomeResults {
    
    internal class OutcomeRollupScoreModel {
        
        [JsonProperty("score")]
        public uint? Score { get; set; }
        
        [JsonProperty("count")]
        public uint? Count { get; set; }
        
        [JsonProperty("links")]
        public OutcomeRollupScoreLinksModel Links { get; set; }
    }
}
