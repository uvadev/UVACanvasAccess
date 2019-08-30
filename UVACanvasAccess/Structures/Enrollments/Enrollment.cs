using System;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Enrollments;
using UVACanvasAccess.Structures.Users;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Enrollments {
    
    [PublicAPI]
    public class Enrollment : IPrettyPrint {
        private readonly Api _api;
        
        public ulong Id { get; }
        
        public ulong CourseId { get; }
        
        [CanBeNull]
        public string SisCourseId { get; }
        
        [CanBeNull]
        public string CourseIntegrationId { get; }
        
        public ulong? CourseSectionId { get; }
        
        [CanBeNull]
        public string SectionIntegrationId { get; }
        
        [CanBeNull]
        public string SisAccountId { get; }
        
        [CanBeNull]
        public string SisSectionId { get; }
        
        [CanBeNull]
        public string SisUserId { get; }
        
        public string EnrollmentState { get; }
        
        public bool? LimitPrivilegesToCourseSection { get; }
        
        public ulong? SisImportId { get; }
        
        public ulong? RootAccountId { get; }
        
        public string Type { get; }
        
        public ulong UserId { get; }
        
        public ulong? AssociatedUserId { get; }
        
        public string Role { get; }
        
        public ulong RoleId { get; }
        
        public DateTime? CreatedAt { get; }
        
        public DateTime? UpdatedAt { get; }
        
        public DateTime? StartAt { get; }
        
        public DateTime? EndAt { get; }
        
        public DateTime? LastActivityAt { get; }
        
        public DateTime? LastAttendedAt { get; }
        
        public ulong? TotalActivityTime { get; }
        
        public string HtmlUrl { get; }
        
        public Grade Grades { get; }
        
        public UserDisplay User { get; }
        
        public string OverrideGrade { get; }
        
        public decimal? OverrideScore { get; }
        
        [CanBeNull]
        public string UnpostedCurrentGrade { get; }
        
        [CanBeNull]
        public string UnpostedFinalGrade { get; }
        
        [CanBeNull]
        public string UnpostedCurrentScore { get; }
        
        [CanBeNull]
        public string UnpostedFinalScore { get; }
        
        [CanBeNull]
        public bool? HasGradingPeriods { get; }
        
        [CanBeNull]
        public bool? TotalsForAllGradingPeriodsOption { get; }
        
        [CanBeNull]
        public string CurrentGradingPeriodTitle { get; }
        
        [CanBeNull]
        public ulong? CurrentGradingPeriodId { get; }
        
        [CanBeNull]
        public string CurrentPeriodOverrideGrade { get; }
        
        [CanBeNull]
        public decimal? CurrentPeriodOverrideScore { get; }
        
        [CanBeNull]
        public decimal? CurrentPeriodUnpostedFinalScore { get; }
        
        [CanBeNull]
        public string CurrentPeriodUnpostedCurrentGrade { get; }
        
        [CanBeNull]
        public string CurrentPeriodUnpostedFinalGrade { get; }

        internal Enrollment(Api api, EnrollmentModel model) {
            _api = api;
            Id = model.Id;
            CourseId = model.CourseId;
            SisCourseId = model.SisCourseId;
            CourseIntegrationId = model.CourseIntegrationId;
            CourseSectionId = model.CourseSectionId;
            SectionIntegrationId = model.SectionIntegrationId;
            SisAccountId = model.SisAccountId;
            SisSectionId = model.SisSectionId;
            SisUserId = model.SisUserId;
            EnrollmentState = model.EnrollmentState;
            LimitPrivilegesToCourseSection = model.LimitPrivilegesToCourseSection;
            SisImportId = model.SisImportId;
            RootAccountId = model.RootAccountId;
            Type = model.Type;
            UserId = model.UserId;
            AssociatedUserId = model.AssociatedUserId;
            Role = model.Role;
            RoleId = model.RoleId;
            CreatedAt = model.CreatedAt;
            UpdatedAt = model.UpdatedAt;
            StartAt = model.StartAt;
            EndAt = model.EndAt;
            LastActivityAt = model.LastActivityAt;
            LastAttendedAt = model.LastAttendedAt;
            TotalActivityTime = model.TotalActivityTime;
            HtmlUrl = model.HtmlUrl;
            Grades = model.Grades.ConvertIfNotNull(m => new Grade(api, m));
            User = model.User.ConvertIfNotNull(m => new UserDisplay(api, m));
            OverrideGrade = model.OverrideGrade;
            OverrideScore = model.OverrideScore;
            UnpostedCurrentGrade = model.UnpostedCurrentGrade;
            UnpostedFinalGrade = model.UnpostedFinalGrade;
            UnpostedCurrentScore = model.UnpostedCurrentScore;
            UnpostedFinalScore = model.UnpostedFinalScore;
            HasGradingPeriods = model.HasGradingPeriods;
            TotalsForAllGradingPeriodsOption = model.TotalsForAllGradingPeriodsOption;
            CurrentGradingPeriodTitle = model.CurrentGradingPeriodTitle;
            CurrentGradingPeriodId = model.CurrentGradingPeriodId;
            CurrentPeriodOverrideGrade = model.CurrentPeriodOverrideGrade;
            CurrentPeriodOverrideScore = model.CurrentPeriodOverrideScore;
            CurrentPeriodUnpostedFinalScore = model.CurrentPeriodUnpostedFinalScore;
            CurrentPeriodUnpostedCurrentGrade = model.CurrentPeriodUnpostedCurrentGrade;
            CurrentPeriodUnpostedFinalGrade = model.CurrentPeriodUnpostedFinalGrade;
        }

        public string ToPrettyString() {
            return "Enrollment {" + 
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(CourseId)}: {CourseId}," +
                   $"\n{nameof(SisCourseId)}: {SisCourseId}," +
                   $"\n{nameof(CourseIntegrationId)}: {CourseIntegrationId}," +
                   $"\n{nameof(CourseSectionId)}: {CourseSectionId}," +
                   $"\n{nameof(SectionIntegrationId)}: {SectionIntegrationId}," +
                   $"\n{nameof(SisAccountId)}: {SisAccountId}," +
                   $"\n{nameof(SisSectionId)}: {SisSectionId}," +
                   $"\n{nameof(SisUserId)}: {SisUserId}," +
                   $"\n{nameof(EnrollmentState)}: {EnrollmentState}," +
                   $"\n{nameof(LimitPrivilegesToCourseSection)}: {LimitPrivilegesToCourseSection}," +
                   $"\n{nameof(SisImportId)}: {SisImportId}," +
                   $"\n{nameof(RootAccountId)}: {RootAccountId}," +
                   $"\n{nameof(Type)}: {Type}," +
                   $"\n{nameof(UserId)}: {UserId}," +
                   $"\n{nameof(AssociatedUserId)}: {AssociatedUserId}," +
                   $"\n{nameof(Role)}: {Role}," +
                   $"\n{nameof(RoleId)}: {RoleId}," +
                   $"\n{nameof(CreatedAt)}: {CreatedAt}," +
                   $"\n{nameof(UpdatedAt)}: {UpdatedAt}," +
                   $"\n{nameof(StartAt)}: {StartAt}," +
                   $"\n{nameof(EndAt)}: {EndAt}," +
                   $"\n{nameof(LastActivityAt)}: {LastActivityAt}," +
                   $"\n{nameof(LastAttendedAt)}: {LastAttendedAt}," +
                   $"\n{nameof(TotalActivityTime)}: {TotalActivityTime}," +
                   $"\n{nameof(HtmlUrl)}: {HtmlUrl}," +
                   $"\n{nameof(Grades)}: {Grades.ToPrettyString()}," +
                   $"\n{nameof(User)}: {User.ToPrettyString()}," +
                   $"\n{nameof(OverrideGrade)}: {OverrideGrade}," +
                   $"\n{nameof(OverrideScore)}: {OverrideScore}," +
                   $"\n{nameof(UnpostedCurrentGrade)}: {UnpostedCurrentGrade}," +
                   $"\n{nameof(UnpostedFinalGrade)}: {UnpostedFinalGrade}," +
                   $"\n{nameof(UnpostedCurrentScore)}: {UnpostedCurrentScore}," +
                   $"\n{nameof(UnpostedFinalScore)}: {UnpostedFinalScore}," +
                   $"\n{nameof(HasGradingPeriods)}: {HasGradingPeriods}," +
                   $"\n{nameof(TotalsForAllGradingPeriodsOption)}: {TotalsForAllGradingPeriodsOption}," +
                   $"\n{nameof(CurrentGradingPeriodTitle)}: {CurrentGradingPeriodTitle}," +
                   $"\n{nameof(CurrentGradingPeriodId)}: {CurrentGradingPeriodId}," +
                   $"\n{nameof(CurrentPeriodOverrideGrade)}: {CurrentPeriodOverrideGrade}," +
                   $"\n{nameof(CurrentPeriodOverrideScore)}: {CurrentPeriodOverrideScore}," +
                   $"\n{nameof(CurrentPeriodUnpostedFinalScore)}: {CurrentPeriodUnpostedFinalScore}," +
                   $"\n{nameof(CurrentPeriodUnpostedCurrentGrade)}: {CurrentPeriodUnpostedCurrentGrade}," +
                   $"\n{nameof(CurrentPeriodUnpostedFinalGrade)}: {CurrentPeriodUnpostedFinalGrade}").Indent(4) +
                   "\n}";
        }
    }
}