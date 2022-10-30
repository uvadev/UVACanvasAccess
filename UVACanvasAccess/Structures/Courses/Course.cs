using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Courses;
using UVACanvasAccess.Structures.Enrollments;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Courses {
    
    /// <summary>
    /// Represents a course.
    /// </summary>
    [PublicAPI]
    public class Course : IPrettyPrint {
        private readonly Api api;
        
        /// <summary>
        /// The course id.
        /// </summary>
        public ulong Id { get; }
        
        /// <summary>
        /// The SIS course id.
        /// </summary>
        public string SisCourseId { get; }
        
        /// <summary>
        /// The course UUID.
        /// </summary>
        public string Uuid { get; }
        
        /// <summary>
        /// The integration id.
        /// </summary>
        public string IntegrationId { get; }
        
        /// <summary>
        /// The SIS import id.
        /// </summary>
        public ulong? SisImportId { get; }
        
        /// <summary>
        /// The name of the course.
        /// </summary>
        /// <remarks>
        /// If the current user has a nickname set for this course, the nickname will be reflected here.
        /// <see cref="OriginalName"/> will contain the original name.
        /// </remarks>
        public string Name { get; }
        
        /// <summary>
        /// The original name of the course, if the current user has a nickname set for this course.
        /// </summary>
        [CanBeNull]
        public string OriginalName { get; }
        
        /// <summary>
        /// The course code.
        /// </summary>
        public string CourseCode { get; }
        
        /// <summary>
        /// The course's workflow state.
        /// One of: {unpublished, available, completed, deleted}
        /// </summary>
        public string WorkflowState { get; }
        
        /// <summary>
        /// The id of the account the course is under.
        /// </summary>
        public ulong AccountId { get; }
        
        /// <summary>
        /// The id of the root account the course is under.
        /// </summary>
        public ulong RootAccountId { get; }
        
        /// <summary>
        /// The id of the enrollment term the course is under.
        /// </summary>
        public ulong EnrollmentTermId { get; }
        
        /// <summary>
        /// The id of the grading standard associated with the course.
        /// </summary>
        public ulong? GradingStandardId { get; }
        
        /// <summary>
        /// When the course was created.
        /// </summary>
        public DateTime CreatedAt { get; }
        
        /// <summary>
        /// The start date of the course.
        /// </summary>
        public DateTime? StartAt { get; }
        
        /// <summary>
        /// The end date of the course.
        /// </summary>
        public DateTime? EndAt { get; }
        
        /// <summary>
        /// The course's locale.
        /// </summary>
        public string Locale { get; }
        
        /// <summary>
        /// Any enrollments for this course which are associated with the current user.
        /// </summary>
        [CanBeNull]
        public IEnumerable<Enrollment> Enrollments { get; }
        
        /// <summary>
        /// The total amount of students in the course.
        /// </summary>
        public ulong? TotalStudents { get; }
        
        /// <summary>
        /// The course calendar link.
        /// </summary>
        public string CalendarLink { get; }
        
        /// <summary>
        /// The view type which users will first be shown when visiting the course page.
        /// </summary>
        public string DefaultView { get; }
        
        /// <summary>
        /// The content of the course syllabus.
        /// </summary>
        [CanBeNull]
        public string SyllabusBody { get; }
        
        /// <summary>
        /// The amount of submissions which need to be graded.
        /// </summary>
        [OptIn]
        public uint? NeedsGradingCount { get; }
        
        /// <summary>
        /// The enrollment term this course is under.
        /// </summary>
        [OptIn]
        [CanBeNull]
        public Term Term { get; }
        
        /// <summary>
        /// Course progress information.
        /// </summary>
        [OptIn]
        [CanBeNull]
        public CourseProgress CourseProgress { get; }
        
        /// <summary>
        /// Whether the course applies assignment group weights.
        /// </summary>
        public bool? ApplyAssignmentGroupWeights { get; }
        
        /// <summary>
        /// The set of permissions the current user has in this course.
        /// </summary>
        [OptIn]
        [Enigmatic]
        [CanBeNull]
        public Dictionary<string, bool> Permissions { get; }
        
        /// <summary>
        /// Whether the course is public
        /// </summary>
        public bool? IsPublic { get; }
        
        /// <summary>
        /// Whether the course is visible to certain authorized users.
        /// </summary>
        public bool? IsPublicToAuthUsers { get; }
        
        /// <summary>
        /// Whether the course has a public syllabus.
        /// </summary>
        public bool? PublicSyllabus { get; }
        
        /// <summary>
        /// Whether the course has a syllabus visible to certain authorized users.
        /// </summary>
        public bool? PublicSyllabusToAuth { get; }
        
        /// <summary>
        /// The public course description, if any.
        /// </summary>
        [CanBeNull]
        public string PublicDescription { get; }
        
        /// <summary>
        /// The course's storage quota, in MiB.
        /// </summary>
        public ulong StorageQuotaMb { get; }
        
        /// <summary>
        /// How many MiB of the storage quota have been used.
        /// </summary>
        public ulong StorageQuotaUsedMb { get; }
        
        /// <summary>
        /// Whether the course hides final grades.
        /// </summary>
        public bool? HideFinalGrades { get; }
        
        /// <summary>
        /// The course's license string.
        /// </summary>
        public string License { get; }
        
        /// <summary>
        /// Whether the course allows assignment edits by students.
        /// </summary>
        public bool? AllowStudentAssignmentEdits { get; }
        
        /// <summary>
        /// Whether the course allows wiki comments.
        /// </summary>
        public bool? AllowWikiComments { get; }
        
        /// <summary>
        /// Whether the course allows forum attachments by students.
        /// </summary>
        public bool? AllowStudentForumAttachments { get; }
        
        /// <summary>
        /// Whether the course allows open enrollment.
        /// </summary>
        public bool? OpenEnrollment { get; }
        
        /// <summary>
        /// Whether the course allows self enrollment.
        /// </summary>
        public bool? SelfEnrollment { get; }
        
        /// <summary>
        /// Whether enrollments are restricted to the dates during which a course is active.
        /// </summary>
        public bool? RestrictEnrollmentsToCourseDates { get; }
        
        /// <summary>
        /// The course's format string.
        /// </summary>
        public string CourseFormat { get; }
        
        /// <summary>
        /// Whether the current user has been prevented from viewing the course due to date restrictions.
        /// </summary>
        public bool? AccessRestrictedByDate { get; }
        
        /// <summary>
        /// The course's IANA time zone.
        /// </summary>
        public string TimeZone { get; }
        
        /// <summary>
        /// Whether this course is a blueprint course.
        /// </summary>
        public bool? Blueprint { get; }
        
        /// <summary>
        /// Set of blueprint restrictions.
        /// </summary>
        [CanBeNull]
        public Dictionary<string, bool> BlueprintRestrictions { get; }
        
        /// <summary>
        /// Object-differentiated blueprint restrictions.
        /// </summary>
        [CanBeNull]
        public Dictionary<string, Dictionary<string, bool>> BlueprintRestrictionsByObjectType { get; }

        internal Course(Api api, CourseModel model) {
            this.api = api;
            Id = model.Id;
            SisCourseId = model.SisCourseId;
            Uuid = model.Uuid;
            IntegrationId = model.IntegrationId;
            SisImportId = model.SisImportId;
            Name = model.Name;
            CourseCode = model.CourseCode;
            WorkflowState = model.WorkflowState;
            AccountId = model.AccountId;
            RootAccountId = model.RootAccountId;
            EnrollmentTermId = model.EnrollmentTermId;
            GradingStandardId = model.GradingStandardId;
            CreatedAt = model.CreatedAt;
            StartAt = model.StartAt;
            EndAt = model.EndAt;
            Locale = model.Locale;
            Enrollments = model.Enrollments.SelectNotNull(m => new Enrollment(api, m));
            TotalStudents = model.TotalStudents;
            CalendarLink = model.Calendar.Ics;
            DefaultView = model.DefaultView;
            SyllabusBody = model.SyllabusBody;
            NeedsGradingCount = model.NeedsGradingCount;
            Term = model.Term.ConvertIfNotNull(m => new Term(api, m));
            CourseProgress = model.CourseProgress.ConvertIfNotNull(m => new CourseProgress(api, m));
            ApplyAssignmentGroupWeights = model.ApplyAssignmentGroupWeights;
            Permissions = model.Permissions;
            IsPublic = model.IsPublic;
            IsPublicToAuthUsers = model.IsPublicToAuthUsers;
            PublicSyllabus = model.PublicSyllabus;
            PublicSyllabusToAuth = model.PublicSyllabusToAuth;
            PublicDescription = model.PublicDescription;
            StorageQuotaMb = model.StorageQuotaMb;
            StorageQuotaUsedMb = model.StorageQuotaUsedMb;
            HideFinalGrades = model.HideFinalGrades;
            License = model.License;
            AllowStudentAssignmentEdits = model.AllowStudentAssignmentEdits;
            AllowWikiComments = model.AllowWikiComments;
            AllowStudentForumAttachments = model.AllowStudentForumAttachments;
            OpenEnrollment = model.OpenEnrollment;
            SelfEnrollment = model.SelfEnrollment;
            RestrictEnrollmentsToCourseDates = model.RestrictEnrollmentsToCourseDates;
            CourseFormat = model.CourseFormat;
            AccessRestrictedByDate = model.AccessRestrictedByDate;
            TimeZone = model.TimeZone;
            Blueprint = model.Blueprint;
            BlueprintRestrictions = model.BlueprintRestrictions;
            BlueprintRestrictionsByObjectType = model.BlueprintRestrictionsByObjectType;
        }

        /// <summary>
        /// Get this course's settings.
        /// </summary>
        /// <returns>This course's settings.</returns>
        public Task<CourseSettings> GetSettings() {
            return api.GetCourseSettings(Id);
        }

        /// <summary>
        /// Update this course's settings.
        /// </summary>
        /// <param name="cs">The new settings to apply.</param>
        /// <returns>A Task which will complete when the operation finishes.</returns>
        public Task UpdateSettings(CourseSettings cs) {
            return api.UpdateCourseSettings(Id, cs);
        }
        
        /// <summary>
        /// Streams all enrollments for this course.
        /// </summary>
        /// <param name="types">(Optional) The set of enrollment types to filter by.</param>
        /// <param name="states">(Optional) The set of enrollment states to filter by.</param>
        /// <param name="includes">(Optional) Data to include in the result.</param>
        /// <returns>The stream of enrollments.</returns>
        public IAsyncEnumerable<Enrollment> StreamEnrollments(IEnumerable<Api.CourseEnrollmentType> types = null,
                                                              IEnumerable<Api.CourseEnrollmentState> states = null,
                                                              Api.CourseEnrollmentIncludes? includes = null) {
            return api.StreamCourseEnrollments(Id, types, states, includes);
        }

        /// <inheritdoc />
        public string ToPrettyString() {
            return "Course {" +
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(SisCourseId)}: {SisCourseId}," +
                   $"\n{nameof(Uuid)}: {Uuid}," +
                   $"\n{nameof(IntegrationId)}: {IntegrationId}," +
                   $"\n{nameof(SisImportId)}: {SisImportId}," +
                   $"\n{nameof(Name)}: {Name}," +
                   $"\n{nameof(CourseCode)}: {CourseCode}," +
                   $"\n{nameof(WorkflowState)}: {WorkflowState}," +
                   $"\n{nameof(AccountId)}: {AccountId}," +
                   $"\n{nameof(RootAccountId)}: {RootAccountId}," +
                   $"\n{nameof(EnrollmentTermId)}: {EnrollmentTermId}," +
                   $"\n{nameof(GradingStandardId)}: {GradingStandardId}," +
                   $"\n{nameof(CreatedAt)}: {CreatedAt}," +
                   $"\n{nameof(StartAt)}: {StartAt}," +
                   $"\n{nameof(EndAt)}: {EndAt}," +
                   $"\n{nameof(Locale)}: {Locale}," +
                   $"\n{nameof(Enrollments)}: {Enrollments?.ToPrettyString()}," +
                   $"\n{nameof(TotalStudents)}: {TotalStudents}," +
                   $"\n{nameof(CalendarLink)}: {CalendarLink}," +
                   $"\n{nameof(DefaultView)}: {DefaultView}," +
                   $"\n{nameof(SyllabusBody)}: {SyllabusBody}," +
                   $"\n{nameof(NeedsGradingCount)}: {NeedsGradingCount}," +
                   $"\n{nameof(Term)}: {Term?.ToPrettyString()}," +
                   $"\n{nameof(CourseProgress)}: {CourseProgress?.ToPrettyString()}," +
                   $"\n{nameof(ApplyAssignmentGroupWeights)}: {ApplyAssignmentGroupWeights}," +
                   $"\n{nameof(Permissions)}: {Permissions?.ToPrettyString()}," +
                   $"\n{nameof(IsPublic)}: {IsPublic}," +
                   $"\n{nameof(IsPublicToAuthUsers)}: {IsPublicToAuthUsers}," +
                   $"\n{nameof(PublicSyllabus)}: {PublicSyllabus}," +
                   $"\n{nameof(PublicSyllabusToAuth)}: {PublicSyllabusToAuth}," +
                   $"\n{nameof(PublicDescription)}: {PublicDescription}," +
                   $"\n{nameof(StorageQuotaMb)}: {StorageQuotaMb}," +
                   $"\n{nameof(StorageQuotaUsedMb)}: {StorageQuotaUsedMb}," +
                   $"\n{nameof(HideFinalGrades)}: {HideFinalGrades}," +
                   $"\n{nameof(License)}: {License}," +
                   $"\n{nameof(AllowStudentAssignmentEdits)}: {AllowStudentAssignmentEdits}," +
                   $"\n{nameof(AllowWikiComments)}: {AllowWikiComments}," +
                   $"\n{nameof(AllowStudentForumAttachments)}: {AllowStudentForumAttachments}," +
                   $"\n{nameof(OpenEnrollment)}: {OpenEnrollment}," +
                   $"\n{nameof(SelfEnrollment)}: {SelfEnrollment}," +
                   $"\n{nameof(RestrictEnrollmentsToCourseDates)}: {RestrictEnrollmentsToCourseDates}," +
                   $"\n{nameof(CourseFormat)}: {CourseFormat}," +
                   $"\n{nameof(AccessRestrictedByDate)}: {AccessRestrictedByDate}," +
                   $"\n{nameof(TimeZone)}: {TimeZone}," +
                   $"\n{nameof(Blueprint)}: {Blueprint}," +
                   $"\n{nameof(BlueprintRestrictions)}: {BlueprintRestrictions?.ToPrettyString()}," +
                   $"\n{nameof(BlueprintRestrictionsByObjectType)}: {BlueprintRestrictionsByObjectType?.ToPrettyString()}").Indent(4) + 
                   "\n}";
        }
    }
}