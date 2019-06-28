using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Assignments {
    
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ExternalToolTagAttributesModel {
        
        [JsonProperty("url")]
        public string Url { get; set; }
        
        [JsonProperty("new_tab")]
        public bool NewTab { get; set; }
        
        [JsonProperty("resource_link_id")]
        public string ResourceLinkId { get; set; }
    }
}