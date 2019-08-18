using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Files {
    internal class LicenseModel {
        
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
