using Newtonsoft.Json;

namespace UVACanvasAccess.Model.ExternalTools {
    
    internal class AccountNavigationModel {
        
        [JsonProperty("url")]
        public string Url { get; set; }
        
        [JsonProperty("enabled")]
        public bool? Enabled { get; set; }
        
        [JsonProperty("text")]
        public string Text { get; set; }
        
        [JsonProperty("selection_width")]
        public uint? SelectionWidth { get; set; }
        
        [JsonProperty("selection_height")]
        public uint? SelectionHeight { get; set; }
        
        [JsonProperty("display_type")]
        public string DisplayType { get; set; } // todo enum
    }
}
