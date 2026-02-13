using System;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.BlueprintCourses {
    
    internal class BlueprintTemplateModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("course_id")]
        public ulong CourseId { get; set; }
        
        [JsonProperty("last_export_completed_at")]
        public DateTime? LastExportCompletedAt { get; set; }
        
        [JsonProperty("associated_course_count")]
        public uint? AssociatedCourseCount { get; set; }
        
        [JsonProperty("latest_migration")]
        [CanBeNull]
        public BlueprintMigrationModel LatestMigration { get; set; }
    }
}
