using Newtonsoft.Json;

namespace UVACanvasAccess.Model.ContentShares {
    
    internal struct ContentExportIdModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
    }
}
