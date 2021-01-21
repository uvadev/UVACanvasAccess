using Newtonsoft.Json;

namespace UVACanvasAccess.Model.ExternalTools {
    
    internal class MigrationSelectionModel {
        
        [JsonProperty("url")]
        public string Url { get; set; }
        
        [JsonProperty("enabled")]
        public bool? Enabled { get; set; }
        
        [JsonProperty("message_type")]
        public string MessageType { get; set; }
    }
}
