using Newtonsoft.Json;


namespace UVACanvasAccess.Model.Discussions {
    
    internal class FileAttachmentModel {
        
        [JsonProperty("content_type")]
        public string ContentType { get; set; }
        
        [JsonProperty("url")]
        public string Url { get; set; }
        
        [JsonProperty("filename")]
        public string Filename { get; set; }
        
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
        
        
    }
}