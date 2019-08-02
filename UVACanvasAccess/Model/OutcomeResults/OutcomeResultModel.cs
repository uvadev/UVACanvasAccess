using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.OutcomeResults {
    
    internal class OutcomeResultModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("score")]
        public uint Score { get; set; }
        
        [JsonProperty("submitted_or_assessed_at")]
        public DateTime SubmittedOrAssessedAt { get; set; }
        
        [JsonProperty("links")]
        public Dictionary<string, object> Links { get; set; } // todo
        
        [JsonProperty("percent")]
        public decimal Percent { get; set; }
    }
}
