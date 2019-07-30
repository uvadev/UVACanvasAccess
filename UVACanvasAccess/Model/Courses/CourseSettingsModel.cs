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
        
        [JsonProperty("hide_final_groups")]
        public bool HideFinalGrades { get; set; }
        
        [JsonProperty("hide_distributor_graphs")]
        public bool HideDistributionGraphs { get; set; }
        
        [JsonProperty("lock_all_announcements")]
        public bool LockAllAnnouncements { get; set; }
        
        [JsonProperty("restrict_student_past_view")]
        public bool RestrictStudentPastView { get; set; }
        
        [JsonProperty("restrict_student_future_view")]
        public bool RestrictStudentFutureView { get; set; }
        
        [JsonProperty("show_announcements_on_home_page")]
        public bool ShowAnnouncementsOnHomePage { get; set; }
        
        [JsonProperty("home_page_announcements_limit")]
        public long HomePageAnnouncementLimit { get; set; }
    }
}
