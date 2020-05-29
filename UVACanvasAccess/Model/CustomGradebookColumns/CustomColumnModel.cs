using Newtonsoft.Json;

namespace UVACanvasAccess.Model.CustomGradebookColumns {
    
    internal class CustomColumnModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("teacher_notes")]
        public bool? TeacherNotes { get; set; }
        
        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("position")]
        public int? Position { get; set; }
        
        [JsonProperty("hidden")]
        public bool? Hidden { get; set; }
        
        [JsonProperty("read_only")]
        public bool? ReadOnly { get; set; }
    }
}
