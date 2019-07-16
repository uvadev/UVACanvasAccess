using System;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.GradeChangelog {
    
    internal class GradeChangeEventModel {
        
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        
        [JsonProperty("event_type")]
        public string EventType { get; set; }
        
        [JsonProperty("excused_after")]
        public bool ExcusedAfter { get; set; }
        
        [JsonProperty("excused_before")]
        public bool ExcusedBefore { get; set; }
        
        [JsonProperty("grade_after")]
        public string GradeAfter { get; set; }
        
        [JsonProperty("grade_before")]
        public string GradeBefore { get; set; }
        
        [JsonProperty("graded_anonymously")]
        public bool? GradedAnonymously { get; set; }
        
        [JsonProperty("version_number")]
        public string VersionNumber { get; set; }
        
        [JsonProperty("request_id")]
        public string RequestId { get; set; }
        
        [JsonProperty("links")]
        [CanBeNull]
        public GradeChangeEventLinksModel Links { get; set; }
    }
}