using Newtonsoft.Json;


namespace UVACanvasAccess.Model.Users {
    internal class AnonymousUserDisplayModel {
        
        [JsonProperty("anonymous_id")]
        public string AnonymousId { get; set; }
        
        [JsonProperty("avatar_image_url")]
        public string AvatarImageUrl { get; set; }

        
    }
}