using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Submissions {
    
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MediaCommentModel {
        
        [JsonProperty("content-type")]
        public string ContentType { get; set; }
        
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
        
        [JsonProperty("media_id")]
        public string MediaId { get; set; }
        
        [JsonProperty("media_type")]
        public string MediaType { get; set; }
        
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}