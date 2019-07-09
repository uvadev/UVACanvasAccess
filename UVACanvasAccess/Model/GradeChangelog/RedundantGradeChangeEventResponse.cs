using System.Collections.Generic;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.GradeChangelog {
    
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class RedundantGradeChangeEventResponse {
        
        [JsonProperty("events")]
        public IEnumerable<GradeChangeEventModel> Events { get; set; }
    }
}