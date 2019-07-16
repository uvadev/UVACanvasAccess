using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Roles {
    
    internal class RolePermissionsModel {
        
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
        
        [JsonProperty("locked")]
        public bool Locked { get; set; }
        
        [JsonProperty("applies_to_self")]
        public bool AppliesToSelf { get; set; }
        
        [JsonProperty("applies_to_descendants")]
        public bool AppliesToDescendants { get; set; }
        
        [JsonProperty("readonly")]
        public bool Readonly { get; set; }
        
        [JsonProperty("explicit")]
        public bool Explicit { get; set; }
        
        [JsonProperty("prior_default")]
        public bool PriorDefault { get; set; }
    }
}