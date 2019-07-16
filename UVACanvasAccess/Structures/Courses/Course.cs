using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Courses;
using UVACanvasAccess.Structures.Enrollments;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Courses {
    
    [PublicAPI]
    public class Course : IPrettyPrint {
        private readonly Api _api;
        
        public ulong Id { get; }
        
        public string SisCourseId { get; }
        
        public string Uuid { get; }
        
        public string IntegrationId { get; }
        
        public ulong? SisImportId { get; }
        
        public string Name { get; }
        
        public string CourseCode { get; }
        
        public string WorkflowState { get; }
        
        public ulong AccountId { get; }
        
        public ulong RootAccountId { get; }
        
        public ulong EnrollmentTermId { get; }
        
        public ulong? GradingStandardId { get; }
        
        public DateTime CreatedAt { get; }
        
        public DateTime? StartAt { get; }
        
        public DateTime? EndAt { get; }
        
        public string Locale { get; }
        
        public IEnumerable<Enrollment> Enrollments { get; }
        
        public ulong? TotalStudents { get; }
        
        public object Calendar { get; } // todo Calendar structure
        
        public string DefaultView { get; }
        
        public string SyllabusBody { get; }
        
        public uint? NeedsGradingCount { get; }
        
        [CanBeNull]
        public Term Term { get; }
        
        [CanBeNull]
        public CourseProgress CourseProgress { get; }
        
        public bool? ApplyAssignmentGroupWeights { get; }
        
        public Dictionary<string, bool> Permissions { get; }
        
        public bool? IsPublic { get; }
        
        public bool? IsPublicToAuthUsers { get; }
        
        public bool? PublicSyllabus { get; }
        
        public bool? PublicSyllabusToAuth { get; }
        
        [CanBeNull]
        public string PublicDescription { get; }
        
        public ulong StorageQuotaMb { get; }
        
        public ulong StorageQuotaUsedMb { get; }
        
        public bool? HideFinalGrades { get; }
        
        public string License { get; }
        
        public bool? AllowStudentAssignmentEdits { get; }
        
        public bool? AllowWikiComments { get; }
        
        public bool? AllowStudentForumAttachments { get; }
        
        public bool? OpenEnrollment { get; }
        
        public bool? SelfEnrollment { get; }
        
        public bool? RestrictEnrollmentsToCourseDates { get; }
        
        public string CourseFormat { get; }
        
        public bool? AccessRestrictedByDate { get; }
        
        public string TimeZone { get; }
        
        public bool? Blueprint { get; }
        
        [CanBeNull]
        public Dictionary<string, bool> BlueprintRestrictions { get; }

        [CanBeNull]
        public Dictionary<string, Dictionary<string, bool>> BlueprintRestrictionsByObjectType { get; }

        internal Course(Api api, CourseModel model) {
            _api = api;
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
            Calendar = model.Calendar;
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
                   $"\n{nameof(Enrollments)}: {Enrollments.ToPrettyString()}," +
                   $"\n{nameof(TotalStudents)}: {TotalStudents}," +
                   $"\n{nameof(Calendar)}: {Calendar}," +
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