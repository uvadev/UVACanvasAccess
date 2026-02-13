using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.BlueprintCourses {
    
    internal class ChangeRecordModel {
        
        [JsonProperty("asset_id")]
        public ulong AssetId { get; set; }
        
        [JsonProperty("asset_type")]
        public string AssetType { get; set; }
        
        [JsonProperty("asset_name")]
        public string AssetName { get; set; }
        
        [JsonProperty("change_type")]
        public string ChangeType { get; set; }
        
        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }
        
        [JsonProperty("locked")]
        public bool? Locked { get; set; }
        
        [JsonProperty("exceptions")]
        [CanBeNull]
        public IEnumerable<ExceptionRecordModel> Exceptions { get; set; }
    }
}
