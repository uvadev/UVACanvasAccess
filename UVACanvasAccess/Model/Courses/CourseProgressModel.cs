using System;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Courses {
    
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CourseProgressModel {
        
        [JsonProperty("requirement_count")]
        public uint? RequirementCount { get; set; }
        
        [JsonProperty("requirement_completed_count")]
        public uint? RequirementCompletedCount { get; set; }
        
        [JsonProperty("next_requirement_url")]
        [CanBeNull]
        public string NextRequirementUrl { get; set; }
        
        [JsonProperty("completed_at")]
        public DateTime? CompletedAt { get; set; }
    }
}