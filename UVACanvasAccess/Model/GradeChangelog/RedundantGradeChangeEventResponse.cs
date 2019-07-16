using System.Collections.Generic;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.GradeChangelog {
    
    internal class RedundantGradeChangeEventResponse {
        
        [JsonProperty("events")]
        public IEnumerable<GradeChangeEventModel> Events { get; set; }
    }
}