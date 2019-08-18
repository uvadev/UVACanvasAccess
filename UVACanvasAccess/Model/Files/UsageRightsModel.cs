using System.Collections.Generic;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Files {
    internal class UsageRightsModel {
        
        [JsonProperty("legal_copyright")]
        public string LegalCopyright { get; set; }
        
        [JsonProperty("use_justification")]
        public string UsageJustification { get; set; }
        
        [JsonProperty("license")]
        public string License { get; set; }
        
        [JsonProperty("license_name")]
        public string LicenseName { get; set; }
        
        [JsonProperty("message")]
        public string Message { get; set; }
        
        [JsonProperty("file_ids")]
        public IEnumerable<ulong> FileIds { get; set; }
    }
}
