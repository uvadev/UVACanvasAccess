using System;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Authentications {
    
    // NOTE: the api documentation for this model is incorrect
    internal class AuthenticationEventModel {
        
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        
        [JsonProperty("event_type")]
        public string EventType { get; set; }
        
        [JsonProperty("links")]
        public AuthenticationEventLinksModel Links { get; set; }
    }

    internal struct AuthenticationEventLinksModel {
        
        [JsonProperty("login")]
        public ulong Login { get; set; }
        
        [JsonProperty("account")]
        public ulong Account { get; set; }
        
        [JsonProperty("user")]
        public ulong User { get; set; }
        
        [JsonProperty("page_view")]
        public ulong? PageView { get; set; }
    }
}