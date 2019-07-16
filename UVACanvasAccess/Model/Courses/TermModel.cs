using System;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Courses {
    
    internal class TermModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("start_at")]
        public DateTime? StartAt { get; set; }
        
        [JsonProperty("end_at")]
        public DateTime? EndAt { get; set; }
    }
}