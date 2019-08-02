using Newtonsoft.Json;

namespace UVACanvasAccess.Model.OutcomeResults {
    
    internal class OutcomeRollupScoreLinksModel {
        
        [JsonProperty("outcome")]
        public ulong Outcome { get; set; }
    }
}
