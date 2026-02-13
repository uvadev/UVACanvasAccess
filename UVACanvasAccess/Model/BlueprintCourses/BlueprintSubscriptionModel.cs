using JetBrains.Annotations;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.BlueprintCourses {
    
    internal class BlueprintSubscriptionModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("template_id")]
        public ulong TemplateId { get; set; }
        
        [JsonProperty("blueprint_course")]
        [CanBeNull]
        public BlueprintCourseModel BlueprintCourse { get; set; }
    }
}
