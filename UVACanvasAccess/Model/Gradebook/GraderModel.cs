using System.Collections.Generic;
using Newtonsoft.Json;
using UVACanvasAccess.Model.Assignments;

namespace UVACanvasAccess.Model.Gradebook {
    
    internal class GraderModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        // the docs say this is a list of integers, but it isn't.
        [JsonProperty("assignments")]
        public IEnumerable<AssignmentModel> Assignments { get; set; }
    }
}