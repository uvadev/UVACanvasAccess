using JetBrains.Annotations;
using Newtonsoft.Json;
using UVACanvasAccess.Model.Assignments;
using UVACanvasAccess.Model.Quizzes;

namespace UVACanvasAccess.Model.ToDos {
    
    [UsedImplicitly]
    internal class ToDoItemModel {
        
        [JsonProperty("context_type")]
        public string ContextType { get; set; }
        
        [JsonProperty("course_id")]
        public ulong? CourseId { get; set; }
        
        [JsonProperty("group_id")]
        public ulong? GroupId { get; set; }
        
        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("ignore")]
        public string IgnoreUrl { get; set; }
        
        [JsonProperty("ignore_permanently")]
        public string PermanentIgnoreUrl { get; set; }
        
        [CanBeNull] 
        [JsonProperty("assignment")]
        public AssignmentModel Assignment { get; set; }
        
        [CanBeNull]
        [JsonProperty("quiz")]
        public QuizModel Quiz { get; set; }
        
        
    }
}
