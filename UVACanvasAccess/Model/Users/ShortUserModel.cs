using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Users {
    
    internal class ShortUserModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
        
        [JsonProperty("avatar_image_url")]
        public string AvatarImageUrl { get; set; }
        
        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }
    }
}
