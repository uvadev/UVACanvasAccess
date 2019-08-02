using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.OutcomeResults {
    
    internal class OutcomeRollupModel {
        
        [CanBeNull]
        [JsonProperty("scores")]
        public IEnumerable<OutcomeRollupScoreModel> Scores { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("links")]
        public OutcomeRollupLinksModel Links { get; set; }
    }
}
