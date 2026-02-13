using JetBrains.Annotations;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.BlueprintCourses {
    
    internal class BlueprintCourseModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("course_code")]
        public string CourseCode { get; set; }
        
        [JsonProperty("term_name")]
        [CanBeNull]
        public string TermName { get; set; }
    }
}
