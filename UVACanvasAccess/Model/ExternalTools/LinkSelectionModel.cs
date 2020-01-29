using Newtonsoft.Json;

namespace UVACanvasAccess.Model.ExternalTools {
    
    internal class LinkSelectionModel {
    
        [JsonProperty("url")]
        public string Url { get; set; }
        
        [JsonProperty("enabled")]
        public bool? Enabled { get; set; }
        
        [JsonProperty("text")]
        public string Text { get; set; }
        
        [JsonProperty("message_type")]
        public string MessageType { get; set; }
    }
}
