using System;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UVACanvasAccess.Model.Users;

namespace UVACanvasAccess.Model.Enrollments {
    
    internal class EnrollmentModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("course_id")]
        public ulong CourseId { get; set; }
        
        [JsonProperty("sis_course_id")]
        [CanBeNull]
        public string SisCourseId { get; set; }
        
        [JsonProperty("course_integration_id")]
        [CanBeNull]
        public string CourseIntegrationId { get; set; }
        
        [JsonProperty("course_section_id")]
        public ulong? CourseSectionId { get; set; }
        
        [JsonProperty("section_integration_id")]
        [CanBeNull]
        public string SectionIntegrationId { get; set; }
        
        [JsonProperty("sis_account_id")]
        [CanBeNull]
        public string SisAccountId { get; set; }
        
        [JsonProperty("sis_section_id")]
        [CanBeNull]
        public string SisSectionId { get; set; }
        
        [JsonProperty("sis_user_id")]
        [CanBeNull]
        public string SisUserId { get; set; }
        
        [JsonProperty("enrollment_state")]
        public string EnrollmentState { get; set; }
        
        [JsonProperty("limit_privileges_to_course_section")]
        public bool? LimitPrivilegesToCourseSection { get; set; }
        
        [JsonProperty("sis_import_id")]
        public ulong? SisImportId { get; set; }
        
        [JsonProperty("root_account_id")]
        public ulong? RootAccountId { get; set; }
        
        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("user_id")]
        public ulong UserId { get; set; }
        
        [JsonProperty("associated_user_id")]
        public ulong? AssociatedUserId { get; set; }
        
        [JsonProperty("role")]
        public string Role { get; set; }
        
        [JsonProperty("role_id")]
        public ulong RoleId { get; set; }
        
        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }
        
        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        
        [JsonProperty("start_at")]
        public DateTime? StartAt { get; set; }
        
        [JsonProperty("end_at")]
        public DateTime? EndAt { get; set; }
        
        [JsonProperty("last_activity_at")]
        public DateTime? LastActivityAt { get; set; }
        
        [JsonProperty("last_attended_at")]
        public DateTime? LastAttendedAt { get; set; }
        
        [JsonProperty("total_activity_time")]
        public ulong? TotalActivityTime { get; set; }
        
        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }
        
        [JsonProperty("grades")]
        public GradeModel Grades { get; set; }
        
        [JsonProperty("user")]
        public UserDisplayModel User { get; set; }
        
        [JsonProperty("override_grade")]
        public string OverrideGrade { get; set; }
        
        [JsonProperty("override_score")]
        public decimal? OverrideScore { get; set; }
        
        [JsonProperty("unposted_current_grade")]
        [CanBeNull]
        public string UnpostedCurrentGrade { get; set; }
        
        [JsonProperty("unposted_final_grade")]
        [CanBeNull]
        public string UnpostedFinalGrade { get; set; }
        
        [JsonProperty("unposted_current_score")]
        [CanBeNull]
        public string UnpostedCurrentScore { get; set; }
        
        [JsonProperty("unposted_final_score")]
        [CanBeNull]
        public string UnpostedFinalScore { get; set; }
        
        [JsonProperty("has_grading_periods")]
        [CanBeNull]
        public bool? HasGradingPeriods { get; set; }

        [JsonProperty("totals_for_all_grading_periods_option")]
        [CanBeNull]
        public bool? TotalsForAllGradingPeriodsOption { get; set; }
        
        [JsonProperty("current_grading_period_title")]
        [CanBeNull]
        public string CurrentGradingPeriodTitle { get; set; }
        
        [JsonProperty("current_grading_period_id")]
        [CanBeNull]
        public ulong? CurrentGradingPeriodId { get; set; }
        
        [JsonProperty("current_period_override_grade")]
        [CanBeNull]
        public string CurrentPeriodOverrideGrade { get; set; }
        
        [JsonProperty("current_period_override_score")]
        [CanBeNull]
        public decimal? CurrentPeriodOverrideScore { get; set; }
        
        [JsonProperty("current_period_unposted_final_score")]
        [CanBeNull]
        public decimal? CurrentPeriodUnpostedFinalScore { get; set; }
        
        [JsonProperty("current_period_unposted_current_grade")]
        [CanBeNull]
        public string CurrentPeriodUnpostedCurrentGrade { get; set; }
        
        [JsonProperty("current_period_unposted_final_grade")]
        [CanBeNull]
        public string CurrentPeriodUnpostedFinalGrade { get; set; }
    }
}