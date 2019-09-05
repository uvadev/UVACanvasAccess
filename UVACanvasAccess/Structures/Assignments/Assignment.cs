using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Assignments;
using UVACanvasAccess.Model.Discussions;
using UVACanvasAccess.Model.Submissions;
using UVACanvasAccess.Structures.Discussions;
using UVACanvasAccess.Structures.Submissions;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Assignments {
    
    /// <summary>
    /// Represents an assignment.
    /// </summary>
    [PublicAPI]
    public class Assignment : IPrettyPrint {
        private readonly Api _api;
        
        /// <summary>
        /// The id.
        /// </summary>
        public ulong Id { get; }

        /// <summary>
        /// The name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The HTML-formatted description.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// When this assignment was created.
        /// </summary>
        public DateTime CreatedAt { get; }

        /// <summary>
        /// When this assignment was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; }

        /// <summary>
        /// The due date for this assignment, if present.
        /// </summary>
        /// <remarks>
        /// When an assignment <see cref="AssignmentOverride">override</see> that affects the current user
        /// is present, this field will reflect the override as it affects the current user.
        /// </remarks>
        public DateTime? DueAt { get; }

        /// <summary>
        /// When this assignment will lock itself, if applicable.
        /// </summary>
        /// <remarks>
        /// When an assignment <see cref="AssignmentOverride">override</see> that affects the current user
        /// is present, this field will reflect the override as it affects the current user.
        /// </remarks>
        public DateTime? LockAt { get; }

        /// <summary>
        /// When this assignment will unlock itself, if applicable.
        /// </summary>
        /// <remarks>
        /// When an assignment <see cref="AssignmentOverride">override</see> that affects the current user
        /// is present, this field will reflect the override as it affects the current user.
        /// </remarks>
        public DateTime? UnlockAt { get; }

        /// <summary>
        /// Whether or not this assignment has any <see cref="AssignmentOverride">overrides</see>.
        /// </summary>
        public bool HasOverrides { get; }
        
        /// <summary>
        /// All relevant <see cref="AssignmentDate">assignment dates</see>.
        /// </summary>
        [CanBeNull]
        [OptIn]
        public IEnumerable<AssignmentDate> AllDates { get; }

        /// <summary>
        /// The course this assignment belongs to.
        /// </summary>
        public ulong CourseId { get; }

        /// <summary>
        /// The URL to this assignment's web page.
        /// </summary>
        public string HtmlUrl { get; }

        /// <summary>
        /// The URL to all of this assignment's submissions as a zip.
        /// </summary>
        public string SubmissionsDownloadUrl { get; }

        /// <summary>
        /// The assignment's group.
        /// </summary>
        public ulong AssignmentGroupId { get; }

        /// <summary>
        /// Whether or not a due date must be present for this assignment, reflecting the
        /// <see cref="UVACanvasAccess.Structures.Accounts.Account">account-level</see> setting.
        /// </summary>
        public bool DueDateRequired { get; }
        
        /// <summary>
        /// If this assignment allows online submission, the list of allowed file extensions for submission.
        /// </summary>
        [CanBeNull]
        public IEnumerable<string> AllowedExtensions { get; }

        /// <summary>
        /// The maximum length this assignment's name may be.
        /// </summary>
        public uint MaxNameLength { get; }

        /// <summary>
        /// If the Turnitin plugin is available to the account,
        /// whether or not Turnitin is enabled for this assignment.
        /// </summary>
        public bool? TurnitinEnabled { get; }

        /// <summary>
        /// If the VeriCite plugin is available to the account,
        /// whether or not VeriCite is enabled for this assignment.
        /// </summary>
        public bool? VeriCiteEnabled { get; } 
        
        /// <summary>
        /// If Turnitin is enabled, the relevant settings.
        /// </summary>
        [CanBeNull]
        public TurnitinSettings TurnitinSettings { get; }

        /// <summary>
        /// If this is a group assignment, whether or not students will be graded individually.
        /// </summary>
        public bool? GradeGroupStudentsIndividually { get; }
        
        /// <summary>
        /// If this assignment support external tools, the relevant attributes.
        /// </summary>
        [CanBeNull]
        [OptIn]
        public ExternalToolTagAttributes ExternalToolTagAttributes { get; }

        /// <summary>
        /// Whether or not this assignment requires peer reviews.
        /// </summary>
        public bool PeerReviews { get; }

        /// <summary>
        /// Whether or not peer reviews are assigned automatically.
        /// </summary>
        public bool AutomaticPeerReviews { get; }
        
        /// <summary>
        /// The amount of reviews each user is assigned.
        /// </summary>
        public uint? PeerReviewCount { get; }
        
        /// <summary>
        /// The date when peer reviews are due. Defaults to the assignment's due date if absent.
        /// </summary>
        public DateTime? PeerReviewsAssignAt { get; }

        /// <summary>
        /// Whether or not students are allowed to peer review work from fellow group members.
        /// </summary>
        public bool? IntraGroupPeerReviews { get; }
        
        /// <summary>
        /// If this is a group assignment, the assignment's group set.
        /// </summary>
        public ulong? GroupCategoryId { get; }
        
        /// <summary>
        /// The amount of submissions that need grading.
        /// </summary>
        public uint? NeedsGradingCount { get; }
        
        /// <summary>
        /// The amount of submissions that need grading, organized by section.
        /// </summary>
        [CanBeNull]
        [OptIn]
        public IEnumerable<NeedsGradingCount> NeedsGradingCountBySection { get; }
        
        /// <summary>
        /// The position of this assignment relative to others in the group.
        /// </summary>
        public ulong Position { get; }
        
        public bool? PostToSis { get; }
        
        [CanBeNull]
        public string IntegrationId { get; }
        
        [CanBeNull]
        public object IntegrationData { get; }
        
        /// <summary>
        /// For courses using Old Gradebook, indicates whether the assignment is muted.
        /// For courses using New Gradebook, true if the assignment has any unposted
        /// submissions, otherwise false.
        /// </summary>
        public bool? Muted { get; }
        
        /// <summary>
        /// The maximum amount of points possible for this assignment.
        /// </summary>
        public uint? PointsPossible { get; }
        
        /// <summary>
        /// The submission types supported for this assignment.
        /// </summary>
        public IEnumerable<string> SubmissionTypes { get; }
        
        /// <summary>
        /// Whether or not any submissions have been made yet.
        /// </summary>
        public bool? HasSubmittedSubmissions { get; }

        /// <summary>
        /// The type of grading used by this assignment.
        /// </summary>
        public string GradingType { get; }
        
        /// <summary>
        /// The id of the grading standard.
        /// </summary>
        public ulong? GradingStandardId { get; }
        
        /// <summary>
        /// Whether or not this assignment is published.
        /// </summary>
        public bool Published { get; }
        
        /// <summary>
        /// Whether or not this assignment can be unpublished.
        /// </summary>
        public bool Unpublishable { get; }
        
        /// <summary>
        /// Whether or not this assignment is only visible to overrides.
        /// </summary>
        public bool OnlyVisibleToOverrides { get; }
        
        /// <summary>
        /// Whether or not this assignment is locked for the current user.
        /// </summary>
        public bool LockedForUser { get; }
        
        /// <summary>
        /// If this assignment is locked for the current user, the relevant info.
        /// </summary>
        [CanBeNull]
        public LockInfo LockInfo { get; }
        
        /// <summary>
        /// If this assignment is locked for the current user, the explanation.
        /// </summary>
        [CanBeNull]
        public string LockExplanation { get; }

        /// <summary>
        /// If this assignment is an online quiz, the quiz id associated with it.
        /// </summary>
        public ulong? QuizId { get; }

        /// <summary>
        /// If this assignment is an online quiz, whether or not anonymous submissions are allowed.
        /// </summary>
        public bool? AnonymousSubmissions { get; }
        
        /// <summary>
        /// The associated <see cref="DiscussionTopic"/>, if applicable.
        /// </summary>
        [CanBeNull]
        public DiscussionTopic DiscussionTopic { get; }

        /// <summary>
        /// Whether or not this assignment will be frozen when copied.
        /// </summary>
        public bool? FreezeOnCopy { get; }

        /// <summary>
        /// Whether or not this assignment is frozen for the current user.
        /// </summary>
        public bool? Frozen { get; }
        
        [CanBeNull]
        public IEnumerable<string> FrozenAttributes { get; }
        
        /// <summary>
        /// The current user's submission to this assignment, if present.
        /// </summary>
        [CanBeNull]
        [OptIn]
        public Submission Submission { get; }
        
        /// <summary>
        /// Whether the included rubric is used for grading, or only advisory.
        /// </summary>
        public bool? UseRubricForGrading { get; }
        
        /// <summary>
        /// The rubric settings.
        /// </summary>
        [CanBeNull]
        public object RubricSettings { get; } // again, docs give no concrete type.
        
        /// <summary>
        /// The list of rubric criteria that comprise the rubric.
        /// </summary>
        [CanBeNull]
        public IEnumerable<RubricCriteria> Rubric { get; } 
        
        /// <summary>
        /// The list of users who can see this assignment.
        /// </summary>
        [CanBeNull]
        [OptIn]
        public IEnumerable<ulong> AssignmentVisibility { get; }
        
        /// <summary>
        /// The list of <see cref="AssignmentOverride">overrides</see>, if present.
        /// </summary>
        [CanBeNull]
        [OptIn]
        public IEnumerable<AssignmentOverride> Overrides { get; }
        
        /// <summary>
        /// Whether or not this assignment will be omitted from the final grade.
        /// </summary>
        public bool? OmitFromFinalGrade { get; }
        
        /// <summary>
        /// Whether or not this assignment is moderated.
        /// </summary>
        public bool ModeratedGrading { get; }
        
        /// <summary>
        /// If this assignment is moderated, the maximum amount of provisional graders.
        /// </summary>
        public uint GraderCount { get; }
        
        /// <summary>
        /// If this assignment is moderated, the user responsible for choosing the final grade.
        /// </summary>
        public ulong? FinalGraderId { get; }
        
        /// <summary>
        /// If this assignment is moderated, whether or not graders' comments are visible to other graders.
        /// </summary>
        public bool? GraderCommentsVisibleToGraders { get; }
        
        /// <summary>
        /// If this assignment is moderated, whether or not graders are anonymous to other graders.
        /// </summary>
        public bool? GradersAnonymousToGraders { get; }
        
        /// <summary>
        /// If this assignment is moderated, whether or not grader names are visible to the final grader.
        /// </summary>
        public bool? GraderNamesVisibleToFinalGrader { get; }
        
        /// <summary>
        /// Whether or not students are anonymous to graders.
        /// </summary>
        public bool? AnonymousGrading { get; }

        /// <summary>
        /// The maximum amount of submission attempts allowed. -1 indicates unlimited attempts.
        /// </summary>
        public int AllowedAttempts { get; }

        [SuppressMessage("ReSharper", "ImplicitlyCapturedClosure")]
        internal Assignment(Api api, AssignmentModel model) {
            _api = api;
            Id = model.Id;
            Name = model.Name;
            Description = model.Description;
            CreatedAt = model.CreatedAt;
            UpdatedAt = model.UpdatedAt;
            DueAt = model.DueAt;
            LockAt = model.LockAt;
            UnlockAt = model.UnlockAt;
            HasOverrides = model.HasOverrides;
            AllDates = model.AllDates == null ? null
                                              : from dateModel in model.AllDates
                                                select new AssignmentDate(api, dateModel);
            CourseId = model.CourseId;
            HtmlUrl = model.HtmlUrl;
            SubmissionsDownloadUrl = model.SubmissionsDownloadUrl;
            AssignmentGroupId = model.AssignmentGroupId;
            DueDateRequired = model.DueDateRequired;
            AllowedExtensions = model.AllowedExtensions;
            MaxNameLength = model.MaxNameLength;
            TurnitinEnabled = model.TurnitinEnabled;
            VeriCiteEnabled = model.VeriCiteEnabled;
            TurnitinSettings = model.TurnitinSettings.ConvertIfNotNull(m => new TurnitinSettings(api, m));
            GradeGroupStudentsIndividually = model.GradeGroupStudentsIndividually;
            ExternalToolTagAttributes = model.ExternalToolTagAttributes.ConvertIfNotNull(m => new ExternalToolTagAttributes(api, m));
            PeerReviews = model.PeerReviews;
            AutomaticPeerReviews = model.AutomaticPeerReviews;
            PeerReviewCount = model.PeerReviewCount;
            PeerReviewsAssignAt = model.PeerReviewsAssignAt;
            IntraGroupPeerReviews = model.IntraGroupPeerReviews;
            GroupCategoryId = model.GroupCategoryId;
            NeedsGradingCount = model.NeedsGradingCount;
            NeedsGradingCountBySection = model.NeedsGradingCountBySection.SelectNotNull(m => new NeedsGradingCount(api, m));
            Position = model.Position;
            PostToSis = model.PostToSis;
            IntegrationId = model.IntegrationId;
            IntegrationData = model.IntegrationData;
            Muted = model.Muted;
            PointsPossible = model.PointsPossible;
            SubmissionTypes = model.SubmissionTypes;
            HasSubmittedSubmissions = model.HasSubmittedSubmissions;
            GradingType = model.GradingType;
            GradingStandardId = model.GradingStandardId;
            Published = model.Published;
            Unpublishable = model.Unpublishable;
            OnlyVisibleToOverrides = model.OnlyVisibleToOverrides;
            LockedForUser = model.LockedForUser;
            LockInfo = model.LockInfo.ConvertIfNotNull(m => new LockInfo(api, m));
            LockExplanation = model.LockExplanation;
            QuizId = model.QuizId;
            AnonymousSubmissions = model.AnonymousSubmissions;
            DiscussionTopic = model.DiscussionTopic.ConvertIfNotNull(m => new DiscussionTopic(api, m, DiscussionTopic.DiscussionHome.Course, model.CourseId));
            FreezeOnCopy = model.FreezeOnCopy;
            Frozen = model.Frozen;
            FrozenAttributes = model.FrozenAttributes;
            Submission = model.Submission.ConvertIfNotNull(m => new Submission(api, m));
            UseRubricForGrading = model.UseRubricForGrading;
            RubricSettings = model.RubricSettings;
            Rubric = model.Rubric.SelectNotNull(m => new RubricCriteria(api, m));
            AssignmentVisibility = model.AssignmentVisibility;
            Overrides = model.Overrides.SelectNotNull(m => new AssignmentOverride(api, m));
            OmitFromFinalGrade = model.OmitFromFinalGrade;
            ModeratedGrading = model.ModeratedGrading;
            GraderCount = model.GraderCount;
            FinalGraderId = model.FinalGraderId;
            GraderCommentsVisibleToGraders = model.GraderCommentsVisibleToGraders;
            GradersAnonymousToGraders = model.GradersAnonymousToGraders;
            GraderNamesVisibleToFinalGrader = model.GraderNamesVisibleToFinalGrader;
            AnonymousGrading = model.AnonymousGrading;
            AllowedAttempts = model.AllowedAttempts;
        }

        public string ToPrettyString() {
            return "Assignment {" +
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(Name)}: {Name}," +
                   $"\n{nameof(Description)}: {Description}," +
                   $"\n{nameof(CreatedAt)}: {CreatedAt}," +
                   $"\n{nameof(UpdatedAt)}: {UpdatedAt}," +
                   $"\n{nameof(DueAt)}: {DueAt}," +
                   $"\n{nameof(LockAt)}: {LockAt}," +
                   $"\n{nameof(UnlockAt)}: {UnlockAt}," +
                   $"\n{nameof(HasOverrides)}: {HasOverrides}," +
                   $"\n{nameof(AllDates)}: {AllDates?.ToPrettyString()}," +
                   $"\n{nameof(CourseId)}: {CourseId}," +
                   $"\n{nameof(HtmlUrl)}: {HtmlUrl}," +
                   $"\n{nameof(SubmissionsDownloadUrl)}: {SubmissionsDownloadUrl}," +
                   $"\n{nameof(AssignmentGroupId)}: {AssignmentGroupId}," +
                   $"\n{nameof(DueDateRequired)}: {DueDateRequired}," +
                   $"\n{nameof(AllowedExtensions)}: {AllowedExtensions?.ToPrettyString()}," +
                   $"\n{nameof(MaxNameLength)}: {MaxNameLength}," +
                   $"\n{nameof(TurnitinEnabled)}: {TurnitinEnabled}," +
                   $"\n{nameof(VeriCiteEnabled)}: {VeriCiteEnabled}," +
                   $"\n{nameof(TurnitinSettings)}: {TurnitinSettings}," +
                   $"\n{nameof(GradeGroupStudentsIndividually)}: {GradeGroupStudentsIndividually}," +
                   $"\n{nameof(ExternalToolTagAttributes)}: {ExternalToolTagAttributes}," +
                   $"\n{nameof(PeerReviews)}: {PeerReviews}," +
                   $"\n{nameof(AutomaticPeerReviews)}: {AutomaticPeerReviews}," +
                   $"\n{nameof(PeerReviewCount)}: {PeerReviewCount}," +
                   $"\n{nameof(PeerReviewsAssignAt)}: {PeerReviewsAssignAt}," +
                   $"\n{nameof(IntraGroupPeerReviews)}: {IntraGroupPeerReviews}," +
                   $"\n{nameof(GroupCategoryId)}: {GroupCategoryId}," +
                   $"\n{nameof(NeedsGradingCount)}: {NeedsGradingCount}," +
                   $"\n{nameof(NeedsGradingCountBySection)}: {NeedsGradingCountBySection?.ToPrettyString()}," +
                   $"\n{nameof(Position)}: {Position}," +
                   $"\n{nameof(PostToSis)}: {PostToSis}," +
                   $"\n{nameof(IntegrationId)}: {IntegrationId}," +
                   $"\n{nameof(IntegrationData)}: {IntegrationData}," +
                   $"\n{nameof(Muted)}: {Muted}," +
                   $"\n{nameof(PointsPossible)}: {PointsPossible}," +
                   $"\n{nameof(SubmissionTypes)}: {SubmissionTypes.ToPrettyString()}," +
                   $"\n{nameof(HasSubmittedSubmissions)}: {HasSubmittedSubmissions}," +
                   $"\n{nameof(GradingType)}: {GradingType}," +
                   $"\n{nameof(GradingStandardId)}: {GradingStandardId}," +
                   $"\n{nameof(Published)}: {Published}," +
                   $"\n{nameof(Unpublishable)}: {Unpublishable}," +
                   $"\n{nameof(OnlyVisibleToOverrides)}: {OnlyVisibleToOverrides}," +
                   $"\n{nameof(LockedForUser)}: {LockedForUser}," +
                   $"\n{nameof(LockInfo)}: {LockInfo}," +
                   $"\n{nameof(LockExplanation)}: {LockExplanation}," +
                   $"\n{nameof(QuizId)}: {QuizId}," +
                   $"\n{nameof(AnonymousSubmissions)}: {AnonymousSubmissions}," +
                   $"\n{nameof(DiscussionTopic)}: {DiscussionTopic}," +
                   $"\n{nameof(FreezeOnCopy)}: {FreezeOnCopy}," +
                   $"\n{nameof(Frozen)}: {Frozen}," +
                   $"\n{nameof(FrozenAttributes)}: {FrozenAttributes?.ToPrettyString()}," +
                   $"\n{nameof(Submission)}: {Submission}," +
                   $"\n{nameof(UseRubricForGrading)}: {UseRubricForGrading}," +
                   $"\n{nameof(RubricSettings)}: {RubricSettings}," +
                   $"\n{nameof(Rubric)}: {Rubric?.ToPrettyString()}," +
                   $"\n{nameof(AssignmentVisibility)}: {AssignmentVisibility?.ToPrettyString()}," +
                   $"\n{nameof(Overrides)}: {Overrides?.ToPrettyString()}," +
                   $"\n{nameof(OmitFromFinalGrade)}: {OmitFromFinalGrade}," +
                   $"\n{nameof(ModeratedGrading)}: {ModeratedGrading}," +
                   $"\n{nameof(GraderCount)}: {GraderCount}," +
                   $"\n{nameof(FinalGraderId)}: {FinalGraderId}," +
                   $"\n{nameof(GraderCommentsVisibleToGraders)}: {GraderCommentsVisibleToGraders}," +
                   $"\n{nameof(GradersAnonymousToGraders)}: {GradersAnonymousToGraders}," +
                   $"\n{nameof(GraderNamesVisibleToFinalGrader)}: {GraderNamesVisibleToFinalGrader}," +
                   $"\n{nameof(AnonymousGrading)}: {AnonymousGrading}," +
                   $"\n{nameof(AllowedAttempts)}: {AllowedAttempts}").Indent(4) +
                   "\n}";
        }
    }
}