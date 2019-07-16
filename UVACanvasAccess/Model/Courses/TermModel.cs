using System;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Courses {
    
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class TermModel {
        
        [JsonProperty("")]
        public ulong Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("start_at")]
        public DateTime? StartAt { get; set; }
        
        [JsonProperty("end_at")]
        public DateTime? EndAt { get; set; }
    }
}