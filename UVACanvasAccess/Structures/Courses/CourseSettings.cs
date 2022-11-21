using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Courses;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Courses {
    
    /// <summary>
    /// Represents a certain subset of settings for a <see cref="Course">course</see>.
    /// </summary>
    /// <remarks>
    /// All of these settings are reflected through <see cref="Api.GetCourseSettings"/>, but some of them are only documented
    /// in other endpoints. <br/>
    /// It is presumed that the undocumented settings cannot be updated through <see cref="Api.UpdateCourseSettings"/>, and
    /// must instead be updated through <see cref="Api.EditCourse"/>.
    /// </remarks>
    [PublicAPI]
    public class CourseSettings : IPrettyPrint {
        
        [Enigmatic]
        public bool? AllowFinalGradeOverride { get; }
        
        /// <summary>
        /// Allow students to create discussion topics.
        /// </summary>
        public bool? AllowStudentDiscussionTopics { get; }
        
        /// <summary>
        /// Allow students to upload attachments to discussions.
        /// </summary>
        public bool? AllowStudentForumAttachments { get; }
        
        /// <summary>
        /// Allow students to edit their own replies to discussions.
        /// </summary>
        public bool? AllowStudentDiscussionEditing { get; }
        
        [Enigmatic]
        public bool? GradingStandardEnabled { get; }
        
        [Enigmatic]
        public ulong? GradingStandardId { get; }
        
        /// <summary>
        /// Allow students to organize groups.
        /// </summary>
        public bool? AllowStudentOrganizedGroups { get; }
        
        /// <summary>
        /// Hide final grades in student views, showing only current grades.
        /// </summary>
        public bool? HideFinalGrades { get; }
        
        /// <summary>
        /// Hide grade distribution graphs in student views.
        /// </summary>
        public bool? HideDistributionGraphs { get; }
        
        /// <summary>
        /// Disable comments on all announcements.
        /// </summary>
        public bool? LockAllAnnouncements { get; }
        
        /// <summary>
        /// Forbid students from viewing the course after their enrollment end date.
        /// </summary>
        public bool? RestrictStudentPastView { get; }
        
        /// <summary>
        /// Forbid students from viewing the course before their enrollment start date.
        /// </summary>
        public bool? RestrictStudentFutureView { get; }
        
        /// <summary>
        /// Show announcements from this course on its homepage.
        /// </summary>
        public bool? ShowAnnouncementsOnHomePage { get; }
        
        /// <summary>
        /// How many announcements are shown via <see cref="ShowAnnouncementsOnHomePage"/>.
        /// </summary>
        public long? HomePageAnnouncementLimit { get; }
        
        /// <summary>
        /// Allow students to report discussion topics and replies.
        /// </summary>
        public bool? AllowStudentDiscussionReporting { get; }

        /// <summary>
        /// Allow students to create anonymous discussion topics.
        /// </summary>
        public bool? AllowStudentAnonymousDiscussionTopics { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool? FilterSpeedGraderByStudentGroup { get; }

        [Enigmatic]
        [CanBeNull]
        public object GradePassbackSetting { get; }

        /// <summary>
        /// In student views, hide users from sections besides the one which the current user belongs to.
        /// </summary>
        public bool? HideSectionsOnCourseUsersPage { get; }

        /// <summary>
        /// Require course files to be accompanied by licensing information.
        /// </summary>
        public bool? UsageRightsRequired { get; }

        /// <summary>
        /// Show the course summary on the syllabus page.
        /// </summary>
        public bool? SyllabusCourseSummary { get; }

        [Enigmatic]
        public bool? HomeroomCourse { get; }

        [Enigmatic]
        [CanBeNull]
        public string FriendlyName { get; }

        /// <summary>
        /// The default time shown in the interface when selecting an assignment due date.<br/>
        /// Can be 'inherit', which will inherit the global account setting.
        /// </summary>
        [CanBeNull]
        public string DefaultDueTime { get; }

        /// <summary>
        /// Enable conditional release of learning pathways for individual students.
        /// </summary>
        public bool? ConditionalRelease { get; }

        internal CourseSettings(CourseSettingsModel model) {
            AllowFinalGradeOverride = model.AllowFinalGradeOverride;
            AllowStudentDiscussionTopics = model.AllowStudentDiscussionTopics;
            AllowStudentForumAttachments = model.AllowStudentForumAttachments;
            AllowStudentDiscussionEditing = model.AllowStudentDiscussionEditing;
            GradingStandardEnabled = model.GradingStandardEnabled;
            GradingStandardId = model.GradingStandardId;
            AllowStudentOrganizedGroups = model.AllowStudentOrganizedGroups;
            HideFinalGrades = model.HideFinalGrades;
            HideDistributionGraphs = model.HideDistributionGraphs;
            LockAllAnnouncements = model.LockAllAnnouncements;
            RestrictStudentPastView = model.RestrictStudentPastView;
            RestrictStudentFutureView = model.RestrictStudentFutureView;
            ShowAnnouncementsOnHomePage = model.ShowAnnouncementsOnHomePage;
            HomePageAnnouncementLimit = model.HomePageAnnouncementLimit;
            AllowStudentDiscussionReporting = model.AllowStudentDiscussionReporting;
            AllowStudentAnonymousDiscussionTopics = model.AllowStudentAnonymousDiscussionTopics;
            FilterSpeedGraderByStudentGroup = model.FilterSpeedGraderByStudentGroup;
            GradePassbackSetting = model.GradePassbackSetting;
            HideSectionsOnCourseUsersPage = model.HideSectionsOnCourseUsersPage;
            UsageRightsRequired = model.UsageRightsRequired;
            SyllabusCourseSummary = model.SyllabusCourseSummary;
            HomeroomCourse = model.HomeroomCourse;
            FriendlyName = model.FriendlyName;
            DefaultDueTime = model.DefaultDueTime;
            ConditionalRelease = model.ConditionalRelease;
        }

        internal IEnumerable<(string, string)> GetTuples() {
            return new[] {
                ("allow_final_grade_override", AllowFinalGradeOverride?.ToShortString()),
                ("allow_student_discussion_topics", AllowStudentDiscussionTopics?.ToShortString()),
                ("allow_student_forum_attachments", AllowStudentForumAttachments?.ToShortString()),
                ("allow_student_discussion_editing", AllowStudentDiscussionEditing?.ToShortString()),
                ("grading_standard_enabled", GradingStandardEnabled?.ToShortString()),
                ("grading_standard_id", GradingStandardId?.ToString()),
                ("allow_student_organized_groups", AllowStudentOrganizedGroups?.ToShortString()),
                ("hide_final_grades", HideFinalGrades?.ToShortString()),
                ("hide_distribution_graphs", HideDistributionGraphs?.ToShortString()),
                ("lock_all_announcements", LockAllAnnouncements?.ToShortString()),
                ("restrict_student_past_view", RestrictStudentPastView?.ToShortString()),
                ("restrict_student_future_view", RestrictStudentFutureView?.ToShortString()),
                ("show_announcements_on_home_page", ShowAnnouncementsOnHomePage?.ToShortString()),
                ("home_page_announcement_limit", HomePageAnnouncementLimit?.ToString()),
                ("allow_student_discussion_reporting", AllowStudentDiscussionReporting?.ToShortString()),
                ("allow_student_anonymous_discussion_topics", AllowStudentAnonymousDiscussionTopics?.ToShortString()),
                ("filter_speed_grader_by_student_group", FilterSpeedGraderByStudentGroup?.ToShortString()),
                ("hide_sections_on_course_users_page", HideSectionsOnCourseUsersPage?.ToShortString()),
                ("usage_rights_required", UsageRightsRequired?.ToShortString()),
                ("syllabus_course_summary", SyllabusCourseSummary?.ToShortString()),
                ("default_due_time", DefaultDueTime),
                ("conditional_release", ConditionalRelease?.ToShortString())
            }.Where(t => t.Item2 != null);
        }
        
        /// <summary>
        /// Creates a new CourseSettings instance, used to <see cref="Api.UpdateCourseSettings">update course settings</see>.
        /// Each optional argument corresponds to a property of this class. Set the property to update the value,
        /// or leave it unset to leave the value alone.
        /// </summary>
        public CourseSettings(bool? allowFinalGradeOverride = null,
                              bool? allowStudentDiscussionTopics = null,
                              bool? allowStudentForumAttachments = null, 
                              bool? allowStudentDiscussionEditing = null,
                              bool? gradingStandardEnabled = null,
                              ulong? gradingStandardId = null,
                              bool? allowStudentOrganizedGroups = null,
                              bool? hideFinalGrades = null, 
                              bool? hideDistributionGraphs = null, 
                              bool? lockAllAnnouncements = null,
                              bool? restrictStudentPastView = null,
                              bool? restrictStudentFutureView = null, 
                              bool? showAnnouncementsOnHomePage = null,
                              long? homePageAnnouncementLimit = null,
                              bool? allowStudentDiscussionReporting = null,
                              bool? allowStudentAnonymousDiscussionTopics = null,
                              bool? filterSpeedGraderByStudentGroup = null,
                              bool? hideSectionsOnCourseUsersPage = null,
                              bool? usageRightsRequired = null,
                              bool? syllabusCourseSummary = null,
                              [CanBeNull] string defaultDueTime = null,
                              bool? conditionalRelease = null) {
            
            AllowFinalGradeOverride = allowFinalGradeOverride;
            AllowStudentDiscussionTopics = allowStudentDiscussionTopics;
            AllowStudentForumAttachments = allowStudentForumAttachments;
            AllowStudentDiscussionEditing = allowStudentDiscussionEditing;
            GradingStandardEnabled = gradingStandardEnabled;
            GradingStandardId = gradingStandardId;
            AllowStudentOrganizedGroups = allowStudentOrganizedGroups;
            HideFinalGrades = hideFinalGrades;
            HideDistributionGraphs = hideDistributionGraphs;
            LockAllAnnouncements = lockAllAnnouncements;
            RestrictStudentPastView = restrictStudentPastView;
            RestrictStudentFutureView = restrictStudentFutureView;
            ShowAnnouncementsOnHomePage = showAnnouncementsOnHomePage;
            HomePageAnnouncementLimit = homePageAnnouncementLimit;
            AllowStudentDiscussionReporting = allowStudentDiscussionReporting;
            AllowStudentAnonymousDiscussionTopics = allowStudentAnonymousDiscussionTopics;
            FilterSpeedGraderByStudentGroup = filterSpeedGraderByStudentGroup;
            HideSectionsOnCourseUsersPage = hideSectionsOnCourseUsersPage;
            UsageRightsRequired = usageRightsRequired;
            SyllabusCourseSummary = syllabusCourseSummary;
            DefaultDueTime = defaultDueTime;
            ConditionalRelease = conditionalRelease;
        }

        /// <inheritdoc />
        public string ToPrettyString() {
            return "CourseSettings {" +
                   ($"\n{nameof(AllowFinalGradeOverride)}: {AllowFinalGradeOverride}," +
                   $"\n{nameof(AllowStudentDiscussionTopics)}: {AllowStudentDiscussionTopics}," +
                   $"\n{nameof(AllowStudentForumAttachments)}: {AllowStudentForumAttachments}," +
                   $"\n{nameof(AllowStudentDiscussionEditing)}: {AllowStudentDiscussionEditing}," +
                   $"\n{nameof(GradingStandardEnabled)}: {GradingStandardEnabled}," +
                   $"\n{nameof(GradingStandardId)}: {GradingStandardId}," +
                   $"\n{nameof(AllowStudentOrganizedGroups)}: {AllowStudentOrganizedGroups}," +
                   $"\n{nameof(HideFinalGrades)}: {HideFinalGrades}," +
                   $"\n{nameof(HideDistributionGraphs)}: {HideDistributionGraphs}," +
                   $"\n{nameof(LockAllAnnouncements)}: {LockAllAnnouncements}," +
                   $"\n{nameof(RestrictStudentPastView)}: {RestrictStudentPastView}," +
                   $"\n{nameof(RestrictStudentFutureView)}: {RestrictStudentFutureView}," +
                   $"\n{nameof(ShowAnnouncementsOnHomePage)}: {ShowAnnouncementsOnHomePage}," +
                   $"\n{nameof(HomePageAnnouncementLimit)}: {HomePageAnnouncementLimit}," +
                   $"\n{nameof(AllowStudentDiscussionReporting)}: {AllowStudentDiscussionReporting}," +
                   $"\n{nameof(AllowStudentAnonymousDiscussionTopics)}: {AllowStudentAnonymousDiscussionTopics}," +
                   $"\n{nameof(FilterSpeedGraderByStudentGroup)}: {FilterSpeedGraderByStudentGroup}," +
                   $"\n{nameof(GradePassbackSetting)}: {GradePassbackSetting}," +
                   $"\n{nameof(HideSectionsOnCourseUsersPage)}: {HideSectionsOnCourseUsersPage}," +
                   $"\n{nameof(UsageRightsRequired)}: {UsageRightsRequired}," +
                   $"\n{nameof(SyllabusCourseSummary)}: {SyllabusCourseSummary}," +
                   $"\n{nameof(HomeroomCourse)}: {HomeroomCourse}," +
                   $"\n{nameof(FriendlyName)}: {FriendlyName}," +
                   $"\n{nameof(DefaultDueTime)}: {DefaultDueTime}," +
                   $"\n{nameof(ConditionalRelease)}: {ConditionalRelease}").Indent(4) + 
                   "\n}";
        }
    }
}
