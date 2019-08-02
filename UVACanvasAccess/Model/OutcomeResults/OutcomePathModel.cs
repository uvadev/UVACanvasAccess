using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.OutcomeResults {
    
    internal struct OutcomePathModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [CanBeNull]
        [JsonProperty("parts")]
        public IEnumerable<OutcomePathPartModel> Parts { get; set; }
    }
}
