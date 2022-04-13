using JetBrains.Annotations;
using Newtonsoft.Json;


namespace UVACanvasAccess.Model.Users {
    
    internal class UserDisplayModel {
        
        [JsonProperty("id")]
        public ulong? Id { get; set; }
        
        [JsonProperty("short_name")]
        [CanBeNull]
        public string ShortName { get; set; }
        
        [JsonProperty("display_name")]
        [CanBeNull]
        public string DisplayName { get; set; }
        
        [JsonProperty("avatar_image_url")]
        public string AvatarImageUrl { get; set; }
        
        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }

        [JsonProperty("pronouns")]
        [CanBeNull]
        public string Pronouns { get; set; }
        
        [JsonProperty("anonymous_id")]
        public string AnonymousId { get; set; }
    }
}