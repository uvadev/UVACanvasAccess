using JetBrains.Annotations;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.GradeChangelog {
    
    internal class GradeChangeEventLinksModel {
        
        [JsonProperty("assignment")]
        public ulong Assignment { get; set; }
        
        [JsonProperty("course")]
        public ulong Course { get; set; }
        
        [JsonProperty("student")]
        public ulong Student { get; set; }
        
        [JsonProperty("grader")]
        public ulong Grader { get; set; }
        
        [JsonProperty("page_view")]
        [CanBeNull] 
        public string PageView { get; set; }
    }
}