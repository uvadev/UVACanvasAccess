using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Assignments {
    
    internal class RubricRatingModel {
        
        [JsonProperty("points")]
        public uint Points { get; set; }
        
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("long_description")]
        public string LongDescription { get; set; }
    }
}