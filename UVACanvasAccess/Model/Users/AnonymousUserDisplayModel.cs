using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Users {
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AnonymousUserDisplayModel {
        
        [JsonProperty("anonymous_id")]
        public string AnonymousId { get; set; }
        
        [JsonProperty("avatar_image_url")]
        public string AvatarImageUrl { get; set; }

        public override string ToString() {
            return $"{nameof(AnonymousId)}: {AnonymousId}," +
                   $"\n{nameof(AvatarImageUrl)}: {AvatarImageUrl}";
        }
    }
}