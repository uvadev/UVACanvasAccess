using System;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Exceptions;
using UVACanvasAccess.Model.Quizzes;
using UVACanvasAccess.Structures.Assignments;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Quizzes {

    [PublicAPI]
    public enum QuizType : byte {
        [ApiRepresentation("practice_quiz")]
        PracticeQuiz,
        [ApiRepresentation("assignment")]
        Assignment,
        [ApiRepresentation("graded_server")]
        GradedSurvey,
        [ApiRepresentation("survey")]
        Survey
    }

    [PublicAPI]
    public enum ScoringPolicy : byte {
        [ApiRepresentation("keep_highest")]
        KeepHighest,
        [ApiRepresentation("keep_latest")]
        KeepLatest
    }

    [PublicAPI]
    public enum HideResults : byte {
        [ApiRepresentation("never")]
        Never,
        [ApiRepresentation("always")]
        Always,
        [ApiRepresentation("until_after_last_attempt")]
        UntilAfterLastAttempt
    }
    
    [PublicAPI]
    public class Quiz : IPrettyPrint {

        private readonly Api _api;
        
        public ulong Id { get; }

        public string Title { get; }

        public string HtmlUrl { get; }

        public string MobileUrl { get; }

        public string PreviewUrl { get; }

        public string Description { get; }

        public QuizType QuizType { get; }

        public ulong AssignmentGroupId { get; }
        
        public decimal? TimeLimit { get; }

        public bool? ShuffleAnswers { get; }
        
        public HideResults HideResults { get; }
        
        public bool? ShowCorrectAnswers { get; }

        public bool? ShowCorrectAnswersLastAttempt { get; }

        public DateTime? ShowCorrectAnswersAt { get; }

        public DateTime? HideCorrectAnswersAt { get; }
        
        public bool? OneTimeResults { get; }
        
        public ScoringPolicy? ScoringPolicy { get; }
        
        public int AllowedAttempts { get; }

        public bool? OneQuestionAtATime { get; }
        
        public uint? QuestionCount { get; }

        public decimal? PointsPossible { get; }
        
        public bool? CantGoBack { get; }
        
        [CanBeNull]
        public string AccessCode { get; }
        
        [CanBeNull]
        public string IpFilter { get; }
        
        public DateTime? DueAt { get; }

        public DateTime? LockAt { get; }

        public DateTime? UnlockAt { get; }
        
        public bool? Published { get; }

        public bool? Unpublishable { get; }
        
        public bool? LockedForUser { get; }
        
        [CanBeNull]
        public LockInfo LockInfo { get; }
        
        [CanBeNull]
        public string LockExplanation { get; }
        
        [CanBeNull]
        public string SpeedGraderUrl { get; }

        public string QuizExtensionsUrl { get; }

        internal Quiz(Api api, QuizModel model) {
            _api = api;
            Id = model.Id;
            Title = model.Title;
            HtmlUrl = model.HtmlUrl;
            MobileUrl = model.MobileUrl;
            PreviewUrl = model.PreviewUrl;
            Description = model.Description;
            QuizType = model.QuizType.ToApiRepresentedEnum<QuizType>()
                                     .Expect(() => new BadApiStateException($"Quiz.QuizType was an unexpected value: {model.QuizType}"));
            AssignmentGroupId = model.AssignmentGroupId;
            TimeLimit = model.TimeLimit;
            ShuffleAnswers = model.ShuffleAnswers;
            HideResults = (model.HideResults ?? "never").ToApiRepresentedEnum<HideResults>()
                                                        .Expect(() => new BadApiStateException($"Quiz.HideResults was an unexpected value: {model.HideResults}"));
            ShowCorrectAnswers = model.ShowCorrectAnswers;
            ShowCorrectAnswersLastAttempt = model.ShowCorrectAnswersLastAttempt;
            ShowCorrectAnswersAt = model.ShowCorrectAnswersAt;
            HideCorrectAnswersAt = model.HideCorrectAnswersAt;
            OneTimeResults = model.OneTimeResults;
            ScoringPolicy = model.ScoringPolicy?.ToApiRepresentedEnum<ScoringPolicy>()
                                                .Expect(() => new BadApiStateException($"Quiz.ScoringPolicy was an unexpected value: {model.ScoringPolicy}"));
            AllowedAttempts = model.AllowedAttempts;
            OneQuestionAtATime = model.OneQuestionAtATime;
            QuestionCount = model.QuestionCount;
            PointsPossible = model.PointsPossible;
            CantGoBack = model.CantGoBack;
            AccessCode = model.AccessCode;
            IpFilter = model.IpFilter;
            DueAt = model.DueAt;
            LockAt = model.LockAt;
            UnlockAt = model.UnlockAt;
            Published = model.Published;
            Unpublishable = model.Unpublishable;
            LockedForUser = model.LockedForUser;
            LockInfo = model.LockInfo.ConvertIfNotNull(m => new LockInfo(api, m));
            LockExplanation = model.LockExplanation;
            SpeedGraderUrl = model.SpeedGraderUrl;
            QuizExtensionsUrl = model.QuizExtensionsUrl;
        }

        public string ToPrettyString() {
            return "Quiz {" + 
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(Title)}: {Title}," +
                   $"\n{nameof(HtmlUrl)}: {HtmlUrl}," +
                   $"\n{nameof(MobileUrl)}: {MobileUrl}," +
                   $"\n{nameof(PreviewUrl)}: {PreviewUrl}," +
                   $"\n{nameof(Description)}: {Description}," +
                   $"\n{nameof(QuizType)}: {QuizType}," +
                   $"\n{nameof(AssignmentGroupId)}: {AssignmentGroupId}," +
                   $"\n{nameof(TimeLimit)}: {TimeLimit}," +
                   $"\n{nameof(ShuffleAnswers)}: {ShuffleAnswers}," +
                   $"\n{nameof(HideResults)}: {HideResults}," +
                   $"\n{nameof(ShowCorrectAnswers)}: {ShowCorrectAnswers}," +
                   $"\n{nameof(ShowCorrectAnswersLastAttempt)}: {ShowCorrectAnswersLastAttempt}," +
                   $"\n{nameof(ShowCorrectAnswersAt)}: {ShowCorrectAnswersAt}," +
                   $"\n{nameof(HideCorrectAnswersAt)}: {HideCorrectAnswersAt}," +
                   $"\n{nameof(OneTimeResults)}: {OneTimeResults}," +
                   $"\n{nameof(ScoringPolicy)}: {ScoringPolicy}," +
                   $"\n{nameof(AllowedAttempts)}: {AllowedAttempts}," +
                   $"\n{nameof(OneQuestionAtATime)}: {OneQuestionAtATime}," +
                   $"\n{nameof(QuestionCount)}: {QuestionCount}," +
                   $"\n{nameof(PointsPossible)}: {PointsPossible}," +
                   $"\n{nameof(CantGoBack)}: {CantGoBack}," +
                   $"\n{nameof(AccessCode)}: {AccessCode}," +
                   $"\n{nameof(IpFilter)}: {IpFilter}," +
                   $"\n{nameof(DueAt)}: {DueAt}," +
                   $"\n{nameof(LockAt)}: {LockAt}," +
                   $"\n{nameof(UnlockAt)}: {UnlockAt}," +
                   $"\n{nameof(Published)}: {Published}," +
                   $"\n{nameof(Unpublishable)}: {Unpublishable}," +
                   $"\n{nameof(LockedForUser)}: {LockedForUser}," +
                   $"\n{nameof(LockInfo)}: {LockInfo}," +
                   $"\n{nameof(LockExplanation)}: {LockExplanation}," +
                   $"\n{nameof(SpeedGraderUrl)}: {SpeedGraderUrl}," +
                   $"\n{nameof(QuizExtensionsUrl)}: {QuizExtensionsUrl}").Indent(4) + 
                   "\n}";
        }
    }
}
