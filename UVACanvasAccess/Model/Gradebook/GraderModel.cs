using System.Collections.Generic;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Gradebook {
    
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class GraderModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("assignments")]
        public IEnumerable<ulong> Assignments { get; set; }
    }
}