using Newtonsoft.Json;

namespace UVACanvasAccess.Model.SisImports {

    internal class SisImportCountsModel {
        
        [JsonProperty("accounts")]
        public ulong Accounts { get; set; }
        
        [JsonProperty("terms")]
        public ulong Terms { get; set; }
        
        [JsonProperty("abstract_courses")]
        public ulong AbstractCourses { get; set; }
        
        [JsonProperty("courses")]
        public ulong Courses { get; set; }
        
        [JsonProperty("sections")]
        public ulong Sections { get; set; }
        
        [JsonProperty("xlists")]
        public ulong CrossLists { get; set; }
        
        [JsonProperty("users")]
        public ulong Users { get; set; }
        
        [JsonProperty("enrollments")]
        public ulong Enrollments { get; set; }
        
        [JsonProperty("groups")]
        public ulong Groups { get; set; }
        
        [JsonProperty("group_memberships")]
        public ulong GroupMemberships { get; set; }
        
        [JsonProperty("grade_publishing_results")]
        public ulong GradePublishingResults { get; set; }
        
        // the following three specific fields are null when 0, unlike the other ones which are 0 when 0
        
        [JsonProperty("batch_courses_deleted")]
        public ulong? BatchCoursesDeleted { get; set; }
        
        [JsonProperty("batch_sections_deleted")]
        public ulong? BatchSectionsDeleted { get; set; }
        
        [JsonProperty("batch_enrollments_deleted")]
        public ulong? BatchEnrollmentsDeleted { get; set; }
        
        [JsonProperty("error_count")]
        public ulong Errors { get; set; }
        
        [JsonProperty("warning_count")]
        public ulong Warnings { get; set; }
    }
}
