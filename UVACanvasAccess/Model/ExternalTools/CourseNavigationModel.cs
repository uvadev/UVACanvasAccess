using JetBrains.Annotations;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.ExternalTools {
    
    internal class CourseNavigationModel {
        
        [JsonProperty("enabled")]
        public bool? Enabled { get; set; }
        
        [JsonProperty("text")]
        public string Text { get; set; }
        
        [JsonProperty("visible")]
        public string Visibility { get; set; } 
        
        [JsonProperty("window_target")]
        public string WindowTarget { get; set; } 
        
        [JsonProperty("default")]
        [CanBeNull]
        public string Default { get; set; }
        
        [JsonProperty("display_type")]
        public string DisplayType { get; set; } 
    }
}
