using JetBrains.Annotations;
using UVACanvasAccess.Model.Courses;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Courses {
    
    [PublicAPI]
    public class CourseSettings : IPrettyPrint {
        public bool? AllowFinalGradeOverride { get; }
        
        public bool? AllowStudentDiscussionTopics { get; }
        
        public bool? AllowStudentForumAttachments { get; }
        
        public bool? AllowStudentDiscussionEditing { get; }
        
        public bool? GradingStandardEnabled { get; }
        
        public ulong? GradingStandardId { get; }
        
        public bool? AllowStudentOrganizedGroups { get; }
        
        public bool? HideFinalGrades { get; }
        
        public bool? HideDistributionGraphs { get; }
        
        public bool? LockAllAnnouncements { get; }
        
        public bool? RestrictStudentPastView { get; }
        
        public bool? RestrictStudentFutureView { get; }
        
        public bool? ShowAnnouncementsOnHomePage { get; }
        
        public long? HomePageAnnouncementLimit { get; }

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
        }
        
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
                              long? homePageAnnouncementLimit = null) {
            
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
        }

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
                   $"\n{nameof(HomePageAnnouncementLimit)}: {HomePageAnnouncementLimit}").Indent(4) + 
                   "\n}";
        }
    }
}
