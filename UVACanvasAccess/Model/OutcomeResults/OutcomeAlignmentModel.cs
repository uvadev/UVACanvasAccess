using JetBrains.Annotations;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.OutcomeResults {
    
    internal class OutcomeAlignmentModel {
        
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [CanBeNull]
        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }
    }
}
