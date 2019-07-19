using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UVACanvasAccess.Model.Enrollments;

namespace UVACanvasAccess.Model.Courses {
    
    internal class CourseModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("sis_course_id")]
        public string SisCourseId { get; set; }
        
        [JsonProperty("uuid")]
        public string Uuid { get; set; }
        
        [JsonProperty("integration_id")]
        public string IntegrationId { get; set; }
        
        [JsonProperty("sis_import_id")]
        public ulong? SisImportId { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("course_code")]
        public string CourseCode { get; set; }
        
        [JsonProperty("workflow_state")]
        public string WorkflowState { get; set; }
        
        [JsonProperty("account_id")]
        public ulong AccountId { get; set; }
        
        [JsonProperty("root_account_id")]
        public ulong RootAccountId { get; set; }
        
        [JsonProperty("enrollment_term_id")]
        public ulong EnrollmentTermId { get; set; }
        
        [JsonProperty("grading_standard_id")]
        public ulong? GradingStandardId { get; set; }
        
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        
        [JsonProperty("start_at")]
        public DateTime? StartAt { get; set; }
        
        [JsonProperty("end_at")]
        public DateTime? EndAt { get; set; }
        
        [JsonProperty("locale")]
        public string Locale { get; set; }
        
        [CanBeNull] 
        [JsonProperty("enrollments")]
        public IEnumerable<EnrollmentModel> Enrollments { get; set; }
        
        [JsonProperty("total_students")]
        public ulong? TotalStudents { get; set; }
        
        [JsonProperty("calendar")]
        public CalendarLinkModel Calendar { get; set; }
        
        [JsonProperty("default_view")]
        public string DefaultView { get; set; }
        
        [JsonProperty("syllabus_body")]
        public string SyllabusBody { get; set; }
        
        [JsonProperty("needs_grading_count")]
        public uint? NeedsGradingCount { get; set; }
        
        [JsonProperty("term")]
        [CanBeNull]
        public TermModel Term { get; set; }
        
        [JsonProperty("course_progress")]
        [CanBeNull]
        public CourseProgressModel CourseProgress { get; set; }
        
        [JsonProperty("apply_assignment_group_weights")]
        public bool? ApplyAssignmentGroupWeights { get; set; }
        
        [JsonProperty("permissions")]
        public Dictionary<string, bool> Permissions { get; set; }
        
        [JsonProperty("is_public")]
        public bool? IsPublic { get; set; }
        
        [JsonProperty("is_public_to_auth_users")]
        public bool? IsPublicToAuthUsers { get; set; }
        
        [JsonProperty("public_syllabus")]
        public bool? PublicSyllabus { get; set; }
        
        [JsonProperty("public_syllabus_to_auth")]
        public bool? PublicSyllabusToAuth { get; set; }
        
        [JsonProperty("public_description")]
        [CanBeNull]
        public string PublicDescription { get; set; }
        
        [JsonProperty("storage_quota_mb")]
        public ulong StorageQuotaMb { get; set; }
        
        [JsonProperty("storage_quota_used_mb")]
        public ulong StorageQuotaUsedMb { get; set; }
        
        [JsonProperty("hide_final_grades")]
        public bool? HideFinalGrades { get; set; }
        
        [JsonProperty("license")]
        public string License { get; set; }
        
        [JsonProperty("allow_student_assignment_edits")]
        public bool? AllowStudentAssignmentEdits { get; set; }
        
        [JsonProperty("allow_wiki_comments")]
        public bool? AllowWikiComments { get; set; }
        
        [JsonProperty("allow_student_forum_attachments")]
        public bool? AllowStudentForumAttachments { get; set; }
        
        [JsonProperty("open_enrollment")]
        public bool? OpenEnrollment { get; set; }
        
        [JsonProperty("self_enrollment")]
        public bool? SelfEnrollment { get; set; }
        
        [JsonProperty("restrict_enrollments_to_courses")]
        public bool? RestrictEnrollmentsToCourseDates { get; set; }
        
        [JsonProperty("course_format")]
        public string CourseFormat { get; set; }
        
        [JsonProperty("access_restricted_by_date")]
        public bool? AccessRestrictedByDate { get; set; }
        
        [JsonProperty("time_zone")]
        public string TimeZone { get; set; }
        
        [JsonProperty("blueprint")]
        public bool? Blueprint { get; set; }
        
        [CanBeNull]
        [JsonProperty("blueprint_restrictions")]
        public Dictionary<string, bool> BlueprintRestrictions { get; set; }

        [CanBeNull]
        [JsonProperty("blueprint_restrictions_by_object_type")]
        public Dictionary<string, Dictionary<string, bool>> BlueprintRestrictionsByObjectType { get; set; }
    }
}