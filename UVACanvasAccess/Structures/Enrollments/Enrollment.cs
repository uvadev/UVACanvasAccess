using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Enrollments;
using UVACanvasAccess.Structures.Users;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Enrollments {
    
    /// <summary>
    /// Represents an enrollment of a user in a course.
    /// </summary>
    [PublicAPI]
    public class Enrollment : IPrettyPrint {
        private readonly Api api;
        
        /// <summary>
        /// The enrollment id.
        /// </summary>
        /// <remarks>
        /// This is unrelated to the <see cref="CourseId">course id.</see>
        /// </remarks>
        public ulong Id { get; }
        
        /// <summary>
        /// The id of the course which the user is enrolled in.
        /// </summary>
        public ulong CourseId { get; }
        
        /// <summary>
        /// The SIS id of the course which the user is enrolled in.
        /// </summary>
        [CanBeNull]
        public string SisCourseId { get; }
        
        /// <summary>
        /// The course integration id.
        /// </summary>
        [CanBeNull]
        public string CourseIntegrationId { get; }
        
        /// <summary>
        /// The id of the section within the course which the user is enrolled in.
        /// </summary>
        public ulong? CourseSectionId { get; }
        
        /// <summary>
        /// The section integration id.
        /// </summary>
        [CanBeNull]
        public string SectionIntegrationId { get; }
        
        /// <summary>
        /// The SIS id of the account the enrollment is under.
        /// </summary>
        [CanBeNull]
        public string SisAccountId { get; }
        
        /// <summary>
        /// The SIS id of the section within the course which the user is enrolled in.
        /// </summary>
        [CanBeNull]
        public string SisSectionId { get; }
        
        /// <summary>
        /// The SIS id of the user involved in the enrollment.
        /// </summary>
        [CanBeNull]
        public string SisUserId { get; }
        
        /// <summary>
        /// The state of the enrollment.
        /// </summary>
        public Api.CourseEnrollmentState? EnrollmentState { get; }
        
        /// <summary>
        /// Whether the user is limited to their own section within the course.
        /// </summary>
        public bool? LimitPrivilegesToCourseSection { get; }
        
        /// <summary>
        /// The SIS import id.
        /// </summary>
        public ulong? SisImportId { get; }
        
        /// <summary>
        /// The root account id of the account the enrollment is under.
        /// </summary>
        public ulong? RootAccountId { get; }
        
        /// <summary>
        /// The type of enrollment.
        /// </summary>
        public Api.CourseEnrollmentRoleTypes? Type { get; }
        
        /// <summary>
        /// The id of the user involved in the enrollment.
        /// </summary>
        public ulong UserId { get; }
        
        /// <summary>
        /// If the <see cref="Type"/> is <see cref="Api.CourseEnrollmentRoleTypes.ObserverEnrollment"/>, the id of the
        /// observed user.
        /// </summary>
        public ulong? AssociatedUserId { get; }
        
        /// <summary>
        /// The enrollment role in the course. May match <see cref="Type"/>, but individual courses may also
        /// customize these roles.
        /// </summary>
        public string Role { get; }
        
        /// <summary>
        /// The id of the enrollment role.
        /// </summary>
        public ulong RoleId { get; }
        
        /// <summary>
        /// When the enrollment was created.
        /// </summary>
        public DateTime? CreatedAt { get; }
        
        /// <summary>
        /// When the enrollment was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; }
        
        /// <summary>
        /// When the enrollment begins.
        /// </summary>
        public DateTime? StartAt { get; }
        
        /// <summary>
        /// When the enrollment concludes.
        /// </summary>
        public DateTime? EndAt { get; }
        
        /// <summary>
        /// The last activity time for the user in the context of the enrollment.
        /// </summary>
        public DateTime? LastActivityAt { get; }
        
        /// <summary>
        /// The last attendance time for the user in the context of the enrollment.
        /// </summary>
        public DateTime? LastAttendedAt { get; }
        
        /// <summary>
        /// The activity total time, in seconds, for the user in the context of the enrollment.
        /// </summary>
        public ulong? TotalActivityTime { get; }
        
        /// <summary>
        /// The URL to the Canvas webpage displaying this enrollment.
        /// </summary>
        public string HtmlUrl { get; }
        
        /// <summary>
        /// The user's grade summary in this enrollment.
        /// </summary>
        public Grade Grades { get; }
        
        /// <summary>
        /// A <see cref="UserDisplay"/> for the user involved in the enrollment.
        /// </summary>
        public UserDisplay User { get; }
        
        /// <summary>
        /// The override grade.
        /// </summary>
        [CanBeNull]
        public string OverrideGrade { get; }
        
        /// <summary>
        /// The override score.
        /// </summary>
        public decimal? OverrideScore { get; }
        
        /// <summary>
        /// The user's current grade in the course, including unposted assignments.
        /// </summary>
        [CanBeNull]
        public string UnpostedCurrentGrade { get; }
        
        /// <summary>
        /// The user's final grade in the course, including unposted assignments.
        /// </summary>
        [CanBeNull]
        public string UnpostedFinalGrade { get; }
        
        /// <summary>
        /// The user's current score in the course, including unposted assignments.
        /// </summary>
        [CanBeNull]
        public string UnpostedCurrentScore { get; }
        
        /// <summary>
        /// The user's final score in the course, including unposted assignments.
        /// </summary>
        [CanBeNull]
        public string UnpostedFinalScore { get; }
        
        /// <summary>
        /// Whether the course has grading periods.
        /// </summary>
        [CanBeNull]
        [Enigmatic]
        public bool? HasGradingPeriods { get; }
        
        /// <summary>
        /// Whether the course has the 'Display Totals for All Grading Periods' option enabled.
        /// </summary>
        [CanBeNull]
        [Enigmatic]
        public bool? TotalsForAllGradingPeriodsOption { get; }
        
        /// <summary>
        /// The title of the current grading period, if the course has grading periods.
        /// </summary>
        [CanBeNull]
        [Enigmatic]
        public string CurrentGradingPeriodTitle { get; }
        
        /// <summary>
        /// The id of the current grading period, if the course has grading periods.
        /// </summary>
        [CanBeNull]
        public ulong? CurrentGradingPeriodId { get; }
        
        /// <summary>
        /// The override grade for the current grading period, if the course has grading periods.
        /// </summary>
        [CanBeNull]
        public string CurrentPeriodOverrideGrade { get; }
        
        /// <summary>
        /// The override score for the current grading period, if the course has grading periods.
        /// </summary>
        [CanBeNull]
        public decimal? CurrentPeriodOverrideScore { get; }
        
        /// <summary>
        /// The user's final score in the course for the current grading period, including unposted assignments, if the course has grading periods.
        /// </summary>
        [CanBeNull]
        public decimal? CurrentPeriodUnpostedFinalScore { get; }
        
        /// <summary>
        /// The user's current grade in the course for the current grading period, including unposted assignments, if the course has grading periods.
        /// </summary>
        [CanBeNull]
        public string CurrentPeriodUnpostedCurrentGrade { get; }
        
        /// <summary>
        /// The user's final grade in the course for the current grading period, including unposted assignments, if the course has grading periods.
        /// </summary>
        [CanBeNull]
        public string CurrentPeriodUnpostedFinalGrade { get; }

        internal Enrollment(Api api, EnrollmentModel model) {
            this.api = api;
            Id = model.Id;
            CourseId = model.CourseId;
            SisCourseId = model.SisCourseId;
            CourseIntegrationId = model.CourseIntegrationId;
            CourseSectionId = model.CourseSectionId;
            SectionIntegrationId = model.SectionIntegrationId;
            SisAccountId = model.SisAccountId;
            SisSectionId = model.SisSectionId;
            SisUserId = model.SisUserId;
            EnrollmentState = model.EnrollmentState.ToApiRepresentedEnum<Api.CourseEnrollmentState>();
            LimitPrivilegesToCourseSection = model.LimitPrivilegesToCourseSection;
            SisImportId = model.SisImportId;
            RootAccountId = model.RootAccountId;
            Type = model.Type.ToApiRepresentedEnum<Api.CourseEnrollmentRoleTypes>();
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

        /// <summary>
        /// Concludes this enrollment without deleting it. <br/>
        /// This is the same action that occurs automatically when the user reaches the end of their time in the course,
        /// such as at the end of the school year.
        /// </summary>
        /// <returns>The concluded enrollment.</returns>
        /// <remarks>
        /// This object will be outdated once the operation completes. Use the returned object instead.
        /// </remarks>
        [Pure]
        public Task<Enrollment> Conclude() {
            return api.ConcludeEnrollment(CourseId, Id);
        }

        /// <summary>
        /// Irrecoverably deletes this enrollment.
        /// </summary>
        /// <returns>The deleted enrollment.</returns>
        /// <remarks>
        /// This object will be outdated once the operation completes. Use the returned object instead.
        /// </remarks>
        [Pure]
        public Task<Enrollment> Delete() {
            return api.DeleteEnrollment(CourseId, Id);
        }

        /// <summary>
        /// Sets an enrollment to <see cref="Api.CourseEnrollmentState.Inactive"/>.
        /// </summary>
        /// <returns>The inactivated enrollment.</returns>
        /// <remarks>
        /// This object will be outdated once the operation completes. Use the returned object instead.
        /// </remarks>
        [Pure]
        public Task<Enrollment> Deactivate() {
            return api.DeactivateEnrollment(CourseId, Id);
        }
        
        /// <inheritdoc/>
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
                   $"\n{nameof(Grades)}: {Grades?.ToPrettyString()}," +
                   $"\n{nameof(User)}: {User?.ToPrettyString()}," +
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