using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Courses {
    
    internal class ShortCourseModel {
    
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
