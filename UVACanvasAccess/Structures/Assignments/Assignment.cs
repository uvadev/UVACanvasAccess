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
    
    [PublicAPI]
    public class Assignment : IPrettyPrint {
        private readonly Api _api;
        
        public ulong Id { get; }

        public string Name { get; }

        public string Description { get; }

        public DateTime CreatedAt { get; }

        public DateTime? UpdatedAt { get; }

        public DateTime? DueAt { get; }

        public DateTime? LockAt { get; }

        public DateTime? UnlockAt { get; }

        public bool HasOverrides { get; }
        
        [CanBeNull]
        public IEnumerable<AssignmentDate> AllDates { get; }

        public ulong CourseId { get; }

        public string HtmlUrl { get; }

        public string SubmissionsDownloadUrl { get; }

        public ulong AssignmentGroupId { get; }

        public bool DueDateRequired { get; }
        
        [CanBeNull]
        public IEnumerable<string> AllowedExtensions { get; }

        public uint MaxNameLength { get; }

        public bool? TurnitinEnabled { get; }

        public bool? VeriCiteEnabled { get; } 
        
        [CanBeNull]
        public TurnitinSettings TurnitinSettings { get; }

        public bool GradeGroupStudentsIndividually { get; }
        
        [CanBeNull]
        public ExternalToolTagAttributes ExternalToolTagAttributes { get; }

        public bool PeerReviews { get; }

        public bool AutomaticPeerReviews { get; }
        
        public uint? PeerReviewCount { get; }
        
        public DateTime? PeerReviewsAssignAt { get; }

        public bool? IntraGroupPeerReviews { get; }
        
        public ulong? GroupCategoryId { get; }
        
        public uint? NeedsGradingCount { get; }
        
        [CanBeNull]
        public IEnumerable<NeedsGradingCount> NeedsGradingCountBySection { get; }
        
        public ulong Position { get; }
        
        public bool? PostToSis { get; }
        
        [CanBeNull]
        public string IntegrationId { get; }
        
        [CanBeNull]
        public object IntegrationData { get; }
        
        public bool? Muted { get; }
        
        public uint PointsPossible { get; }
        
        public IEnumerable<string> SubmissionTypes { get; }
        
        public bool HasSubmittedSubmissions { get; }

        public string GradingType { get; }
        
        public ulong? GradingStandardId { get; }
        
        public bool Published { get; }
        
        public bool Unpublishable { get; }
        
        public bool OnlyVisibleToOverrides { get; }
        
        public bool LockedForUser { get; }
        
        [CanBeNull]
        public LockInfo LockInfo { get; }
        
        [CanBeNull]
        public string LockExplanation { get; }

        public ulong? QuizId { get; }

        public bool? AnonymousSubmissions { get; }
        
        [CanBeNull]
        public DiscussionTopic DiscussionTopic { get; }

        public bool? FreezeOnCopy { get; }

        public bool? Frozen { get; }
        
        [CanBeNull]
        public IEnumerable<string> FrozenAttributes { get; }
        
        [CanBeNull]
        public Submission Submission { get; }
        
        public bool? UseRubricForGrading { get; }
        
        [CanBeNull]
        public object RubricSettings { get; } // again, docs give no concrete type.
        
        [CanBeNull]
        public IEnumerable<RubricCriteria> Rubric { get; } 
        
        [CanBeNull]
        public IEnumerable<ulong> AssignmentVisibility { get; }
        
        [CanBeNull]
        public IEnumerable<AssignmentOverride> Overrides { get; }
        
        public bool? OmitFromFinalGrade { get; }
        
        public bool ModeratedGrading { get; }
        
        public uint GraderCount { get; }
        
        public ulong? FinalGraderId { get; }
        
        public bool? GraderCommentsVisibleToGraders { get; }
        
        public bool? GradersAnonymousToGraders { get; }
        
        public bool? GraderNamesVisibleToFinalGrader { get; }
        
        public bool? AnonymousGrading { get; }

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