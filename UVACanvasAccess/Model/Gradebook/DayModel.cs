using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Gradebook {
    
    internal class DayModel {
        
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("graders")]
        public IEnumerable<GraderModel> Graders { get; set; }
    }
}