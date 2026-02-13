using System.Collections.Generic;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.BlueprintCourses {
    
    internal class ExceptionRecordModel {
        
        [JsonProperty("course_id")]
        public ulong CourseId { get; set; }
        
        [JsonProperty("conflicting_changes")]
        public IEnumerable<string> ConflictingChanges { get; set; }
    }
}
