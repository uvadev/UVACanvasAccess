using Newtonsoft.Json;

namespace UVACanvasAccess.Model.ProficiencyRatings {
    
    internal class ProficiencyRatingModel {
        
        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("points")]
        public uint Points { get; set; }
        
        [JsonProperty("mastery")]
        public bool Mastery { get; set; }
        
        [JsonProperty("color")]
        public string Color { get; set; }
    }
}
