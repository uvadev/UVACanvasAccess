using System;
using Newtonsoft.Json;


namespace UVACanvasAccess.Model.Users {
    internal class PageViewModel {
        
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("app_name")]
        public string AppName { get; set; }
        
        [JsonProperty("url")]
        public string Url { get; set; }
        
        [JsonProperty("context_type")]
        public string ContextType { get; set; }
        
        [JsonProperty("asset_type")]
        public string AssetType { get; set; }
        
        [JsonProperty("controller")]
        public string Controller { get; set; }
        
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("interaction_seconds")]
        public decimal? InteractionSeconds { get; set; }
        
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        
        [JsonProperty("user_request")]
        public bool? UserRequest { get; set; }
        
        [JsonProperty("render_time")]
        public double? RenderTime { get; set; }
        
        [JsonProperty("user_agent")]
        public string UserAgent { get; set; }
        
        [JsonProperty("participated")]
        public bool? Participated { get; set; }
        
        [JsonProperty("http_method")]
        public string HttpMethod { get; set; }
        
        [JsonProperty("remote_ip")]
        public string RemoteIp { get; set; }
        
        [JsonProperty("links")]
        public PageViewLinksModel Links { get; set; }

        
    }
}