using Newtonsoft.Json;


namespace UVACanvasAccess.Model.Users {
    
    internal class ActivityStreamSummaryEntryModel {
        
        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("unread_count")]
        public uint UnreadCount { get; set; }
        
        [JsonProperty("count")]
        public uint Count { get; set; }
        
        
    }
}