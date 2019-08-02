using Newtonsoft.Json;

namespace UVACanvasAccess.Model.OutcomeResults {
    
    internal struct OutcomePathPartModel {
        
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
