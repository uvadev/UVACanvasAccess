using JetBrains.Annotations;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Courses {
    
    internal class CourseSettingsModel {
        
        [JsonProperty("allow_final_grade_override")]
        public bool AllowFinalGradeOverride { get; set; }
        
        [JsonProperty("allow_student_discussion_topics")]
        public bool AllowStudentDiscussionTopics { get; set; }
        
        [JsonProperty("allow_student_forum_attachments")]
        public bool AllowStudentForumAttachments { get; set; }
        
        [JsonProperty("allow_student_discussion_editing")]
        public bool AllowStudentDiscussionEditing { get; set; }
        
        [JsonProperty("grading_standard_enabled")]
        public bool GradingStandardEnabled { get; set; }
        
        [JsonProperty("grading_standard_id")]
        public ulong? GradingStandardId { get; set; }
        
        [JsonProperty("allow_student_organized_groups")]
        public bool AllowStudentOrganizedGroups { get; set; }
        
        [JsonProperty("hide_final_grades")]
        public bool HideFinalGrades { get; set; }
        
        [JsonProperty("hide_distribution_graphs")]
        public bool HideDistributionGraphs { get; set; }
        
        [JsonProperty("lock_all_announcements")]
        public bool LockAllAnnouncements { get; set; }
        
        [JsonProperty("restrict_student_past_view")]
        public bool RestrictStudentPastView { get; set; }
        
        [JsonProperty("restrict_student_future_view")]
        public bool RestrictStudentFutureView { get; set; }
        
        [JsonProperty("show_announcements_on_home_page")]
        public bool ShowAnnouncementsOnHomePage { get; set; }
        
        [JsonProperty("home_page_announcement_limit")]
        public long HomePageAnnouncementLimit { get; set; }
        
        [JsonProperty("allow_student_discussion_reporting")]
        public bool AllowStudentDiscussionReporting { get; set; }
        
        [JsonProperty("allow_student_anonymous_discussion_topics")]
        public bool AllowStudentAnonymousDiscussionTopics { get; set; }
        
        [JsonProperty("filter_speed_grader_by_student_group")]
        public bool FilterSpeedGraderByStudentGroup { get; set; }
        
        // Undocumented
        [JsonProperty("grade_passback_setting")]
        [CanBeNull]
        public object GradePassbackSetting { get; set; }
        
        [JsonProperty("hide_sections_on_course_users_page")]
        public bool HideSectionsOnCourseUsersPage { get; set; }
        
        [JsonProperty("usage_rights_required")]
        public bool UsageRightsRequired { get; set; }
        
        [JsonProperty("syllabus_course_summary")]
        public bool SyllabusCourseSummary { get; set; }
        
        // Undocumented; perhaps mutated through a different endpoint
        [JsonProperty("homeroom_course")]
        public bool? HomeroomCourse { get; set; }
        
        // Undocumented; perhaps mutated through a different endpoint
        [JsonProperty("friendly_name")]
        [CanBeNull]
        public string FriendlyName { get; set; }
        
        [JsonProperty("default_due_time")]
        public string DefaultDueTime { get; set; }
        
        [JsonProperty("conditional_release")]
        public bool ConditionalRelease { get; set; }
    }
}
