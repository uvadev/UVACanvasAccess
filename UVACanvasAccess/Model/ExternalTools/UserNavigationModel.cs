using Newtonsoft.Json;

namespace UVACanvasAccess.Model.ExternalTools {
    
    internal class UserNavigationModel {
        
        [JsonProperty("url")]
        public string Url { get; set; }
        
        [JsonProperty("enabled")]
        public bool? Enabled { get; set; }
        
        [JsonProperty("text")]
        public string Text { get; set; }
        
        [JsonProperty("visibility")]
        public string Visibility { get; set; } 
    }
}
