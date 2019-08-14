using System;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Model.Sections {
    
    internal class SectionModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("sis_section_id")]
        [CanBeNull]
        public string SisSectionId { get; set; }
        
        [JsonProperty("integration_id")]
        [CanBeNull]
        public string IntegrationId { get; set; }
        
        [JsonProperty("sis_import_id")]
        public ulong? SisImportId { get; set; }
        
        [JsonProperty("start_at")]
        public DateTime? StartAt { get; set; }
        
        [JsonProperty("end_at")]
        public DateTime? EndAt { get; set; }
        
        [JsonProperty("restrict_enrollments_to_section_dates")]
        public bool? RestrictEnrollmentsToSectionDates { get; set; }
        
        [JsonProperty("nonxlist_course_id")]
        public ulong? NonCrossListedCourseId { get; set; }
        
        [JsonProperty("total_students")]
        [OptIn]
        public uint? TotalStudents { get; set; }
    }
}
