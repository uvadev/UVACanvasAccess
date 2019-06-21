using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Users {
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class PageViewModel {
        
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
        
        [JsonProperty("contributed")]
        public bool Contributed { get; set; }
        
        [JsonProperty("interaction_seconds")]
        public double InteractionSeconds { get; set; }
        
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
        
        [JsonProperty("user_request")]
        public bool UserRequest { get; set; }
        
        [JsonProperty("render_time")]
        public double RenderTime { get; set; }
        
        [JsonProperty("user_agent")]
        public string UserAgent { get; set; }
        
        [JsonProperty("participated")]
        public bool Participated { get; set; }
        
        [JsonProperty("http_method")]
        public string HttpMethod { get; set; }
        
        [JsonProperty("remote_ip")]
        public string RemoteIp { get; set; }
        
        [JsonProperty("links")]
        public PageViewLinksModel Links { get; set; }

        public override string ToString() {
            return $"{nameof(Id)}: {Id}," +
                   $"\n{nameof(AppName)}: {AppName}," +
                   $"\n{nameof(Url)}: {Url}," +
                   $"\n{nameof(ContextType)}: {ContextType}," +
                   $"\n{nameof(AssetType)}: {AssetType}," +
                   $"\n{nameof(Controller)}: {Controller}," +
                   $"\n{nameof(Action)}: {Action}," +
                   $"\n{nameof(Contributed)}: {Contributed}," +
                   $"\n{nameof(InteractionSeconds)}: {InteractionSeconds}," +
                   $"\n{nameof(CreatedAt)}: {CreatedAt}," +
                   $"\n{nameof(UserRequest)}: {UserRequest}," +
                   $"\n{nameof(RenderTime)}: {RenderTime}," +
                   $"\n{nameof(UserAgent)}: {UserAgent}," +
                   $"\n{nameof(Participated)}: {Participated}," +
                   $"\n{nameof(HttpMethod)}: {HttpMethod}," +
                   $"\n{nameof(RemoteIp)}: {RemoteIp}," +
                   $"\n{nameof(Links)}: {Links}";
        }
    }
}