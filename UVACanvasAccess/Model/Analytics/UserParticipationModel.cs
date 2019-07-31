using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Analytics {
    
    internal struct UserParticipationModel {
        
        [JsonProperty("page_views")]
        public Dictionary<DateTime, ulong> PageViews { get; set; }
        
        [JsonProperty("participations")]
        public IEnumerable<UserParticipationEventModel> Participations { get; set; }
    }

    internal struct UserParticipationEventModel {
        
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
