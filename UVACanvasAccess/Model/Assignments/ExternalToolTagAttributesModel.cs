using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Assignments {
    
    internal class ExternalToolTagAttributesModel {
        
        [JsonProperty("url")]
        public string Url { get; set; }
        
        [JsonProperty("new_tab")]
        public bool? NewTab { get; set; }
        
        [JsonProperty("resource_link_id")]
        public string ResourceLinkId { get; set; }
    }
}