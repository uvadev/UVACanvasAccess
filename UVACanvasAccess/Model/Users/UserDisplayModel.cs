using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Users {
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class UserDisplayModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("short_name")]
        public string ShortName { get; set; }
        
        [JsonProperty("avatar_image_url")]
        public string AvatarImageUrl { get; set; }
        
        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }

        public override string ToString() {
            return $"{nameof(Id)}: {Id}," +
                   $"\n{nameof(ShortName)}: {ShortName}," +
                   $"\n{nameof(AvatarImageUrl)}: {AvatarImageUrl}," +
                   $"\n{nameof(HtmlUrl)}: {HtmlUrl}";
        }
    }
}