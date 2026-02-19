using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Quizzes;
using UVACanvasAccess.Structures.Assignments;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Quizzes {
    
    /// <summary>
    /// Represents a quiz.
    /// </summary>
    [PublicAPI]
    public class Quiz : IPrettyPrint {

        private readonly Api api;
        
        /// <summary>
        /// The quiz id.
        /// </summary>
        public ulong? Id { get; }

        /// <summary>
        /// The quiz title.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// The url to the web interface.
        /// </summary>
        public string HtmlUrl { get; }

        /// <summary>
        /// The url to the mobile view of the web interface.
        /// </summary>
        public string MobileUrl { get; }

        /// <summary>
        /// The url to the quiz preview in the web interface.
        /// </summary>
        public string PreviewUrl { get; }

        /// <summary>
        /// The quiz description.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// The quiz type.
        /// </summary>
        public QuizType? QuizType { get; }

        /// <summary>
        /// The assignment group id.
        /// </summary>
        public ulong? AssignmentGroupId { get; }
        
        /// <summary>
        /// The quiz time limit, in minutes.
        /// </summary>
        public decimal? TimeLimit { get; }

        /// <summary>
        /// Whether the order of answer choices is shuffled for students.
        /// </summary>
        public bool? ShuffleAnswers { get; }
        
        /// <summary>
        /// Whether to hide students' results after they complete the quiz.
        /// </summary>
        public HideResults? HideResults { get; }
        
        /// <summary>
        /// Whether the correct answers should be shown to students in the results view.
        /// </summary>
        public bool? ShowCorrectAnswers { get; }

        /// <summary>
        /// Whether the correct answers should be shown to students in the results view when the student
        /// has used all their quiz attempts.
        /// </summary>
        public bool? ShowCorrectAnswersLastAttempt { get; }

        /// <summary>
        /// The date at which to honor <see cref="ShowCorrectAnswers"/>.
        /// </summary>
        public DateTime? ShowCorrectAnswersAt { get; }

        /// <summary>
        /// The date at which to stop honoring <see cref="ShowCorrectAnswers"/>.
        /// </summary>
        public DateTime? HideCorrectAnswersAt { get; }
        
        /// <summary>
        /// If true, results will only be shown to students right after they submit their answers.
        /// </summary>
        public bool? OneTimeResults { get; }
        
        /// <summary>
        /// The quiz scoring policy.
        /// </summary>
        public ScoringPolicy? ScoringPolicy { get; }
        
        /// <summary>
        /// How many attempts are allowed. A value of '-1' indicates unlimited attempts.
        /// </summary>
        public int? AllowedAttempts { get; }

        /// <summary>
        /// Whether one question should be shown at a time.
        /// </summary>
        public bool? OneQuestionAtATime { get; }
        
        /// <summary>
        /// The amount of questions in the quiz.
        /// </summary>
        public uint? QuestionCount { get; }

        /// <summary>
        /// The amount of points possible.
        /// </summary>
        public decimal? PointsPossible { get; }
        
        /// <summary>
        /// Whether answers should be locked in once given. Only applies if <see cref="OneQuestionAtATime"/> is enabled.
        /// </summary>
        public bool? CantGoBack { get; }
        
        /// <summary>
        /// The access code required to access the quiz.
        /// </summary>
        [CanBeNull]
        public string AccessCode { get; }
        
        /// <summary>
        /// The IP address or range the quiz must be accessed from.
        /// </summary>
        [CanBeNull]
        public string IpFilter { get; }
        
        /// <summary>
        /// When the quiz is due.
        /// </summary>
        public DateTime? DueAt { get; }

        /// <summary>
        /// When the quiz locks.
        /// </summary>
        public DateTime? LockAt { get; }

        /// <summary>
        /// When the quiz unlocks.
        /// </summary>
        public DateTime? UnlockAt { get; }
        
        /// <summary>
        /// Whether the quiz is published.
        /// </summary>
        public bool? Published { get; }

        /// <summary>
        /// Whether the quiz is able to be unpublished.
        /// </summary>
        public bool? Unpublishable { get; }
        
        /// <summary>
        /// Whether the quiz is locked for the current user.
        /// </summary>
        public bool? LockedForUser { get; }
        
        /// <summary>
        /// If <see cref="LockedForUser"/> is true, the lock information.
        /// </summary>
        [CanBeNull]
        public LockInfo LockInfo { get; }
        
        /// <summary>
        /// If <see cref="LockedForUser"/> is true, the lock explanation.
        /// </summary>
        [CanBeNull]
        public string LockExplanation { get; }
        
        /// <summary>
        /// The url to the speed grader interface.
        /// </summary>
        [CanBeNull]
        public string SpeedGraderUrl { get; }

        /// <summary>
        /// The url to the web interface page for granting extensions.
        /// </summary>
        public string QuizExtensionsUrl { get; }
        
        /// <summary>
        /// The quiz permissions for the current user.
        /// </summary>
        [CanBeNull]
        public QuizPermissions Permissions { get; }
        
        /// <summary>
        /// All override dates for this quiz, if available.
        /// </summary>
        [CanBeNull]
        public IEnumerable<AssignmentDate> AllDates { get; }
        
        /// <summary>
        /// The version number of the quiz.
        /// </summary>
        public uint? VersionNumber { get; }
        
        /// <summary>
        /// The list of question types in this quiz.
        /// </summary>
        [CanBeNull]
        public IEnumerable<QuizQuestionType> QuestionTypes { get; }
        
        /// <summary>
        /// Whether submissions are anonymous.
        /// </summary>
        public bool? AnonymousSubmissions { get; }

        internal Quiz(Api api, QuizModel model) {
            this.api = api;
            Id = model.Id;
            Title = model.Title;
            HtmlUrl = model.HtmlUrl;
            MobileUrl = model.MobileUrl;
            PreviewUrl = model.PreviewUrl;
            Description = model.Description;
            QuizType = model.QuizType?.ToApiRepresentedEnum<QuizType>();
            AssignmentGroupId = model.AssignmentGroupId;
            TimeLimit = model.TimeLimit;
            ShuffleAnswers = model.ShuffleAnswers;
            HideResults = model.HideResults?.ToApiRepresentedEnum<HideResults>();
            ShowCorrectAnswers = model.ShowCorrectAnswers;
            ShowCorrectAnswersLastAttempt = model.ShowCorrectAnswersLastAttempt;
            ShowCorrectAnswersAt = model.ShowCorrectAnswersAt;
            HideCorrectAnswersAt = model.HideCorrectAnswersAt;
            OneTimeResults = model.OneTimeResults;
            ScoringPolicy = model.ScoringPolicy?.ToApiRepresentedEnum<ScoringPolicy>();
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
            Permissions = model.Permissions.ConvertIfNotNull(m => new QuizPermissions(api, m));
            AllDates = model.AllDates?.Select(m => new AssignmentDate(api, m));
            VersionNumber = model.VersionNumber;
            QuestionTypes = model.QuestionTypes?.SelectNotNullValue(s => s.ToApiRepresentedEnum<QuizQuestionType>());
            AnonymousSubmissions = model.AnonymousSubmissions;
        }

        /// <inheritdoc />
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
                    $"\n{nameof(LockInfo)}: {LockInfo?.ToPrettyString()}," +
                    $"\n{nameof(LockExplanation)}: {LockExplanation}," +
                    $"\n{nameof(SpeedGraderUrl)}: {SpeedGraderUrl}," +
                    $"\n{nameof(QuizExtensionsUrl)}: {QuizExtensionsUrl}," +
                    $"\n{nameof(Permissions)}: {Permissions?.ToPrettyString()}," +
                    $"\n{nameof(AllDates)}: {AllDates?.ToPrettyString()}," +
                    $"\n{nameof(VersionNumber)}: {VersionNumber}," +
                    $"\n{nameof(QuestionTypes)}: {QuestionTypes?.ToPrettyString()}," +
                    $"\n{nameof(AnonymousSubmissions)}: {AnonymousSubmissions}").Indent(4) + 
                   "\n}";
        }
    }

    /// <summary>
    /// Represents the types of <see cref="Quiz"/>.
    /// </summary>
    [PublicAPI]
    public enum QuizType : byte {
        /// <summary>
        /// The quiz is a practice quiz.
        /// </summary>
        [ApiRepresentation("practice_quiz")]
        PracticeQuiz,
        /// <summary>
        /// The quiz is a graded assignment.
        /// </summary>
        [ApiRepresentation("assignment")]
        Assignment,
        /// <summary>
        /// The quiz is a graded survey.
        /// </summary>
        [ApiRepresentation("graded_survey")]
        GradedSurvey,
        /// <summary>
        /// The quiz is an ungraded survey.
        /// </summary>
        [ApiRepresentation("survey")]
        Survey
    }

    /// <summary>
    /// Represents the types of scoring policy for a <see cref="Quiz"/> if multiple attempts are allowed.
    /// </summary>
    [PublicAPI]
    public enum ScoringPolicy : byte {
        /// <summary>
        /// Keep the highest scoring attempt.
        /// </summary>
        [ApiRepresentation("keep_highest")]
        KeepHighest,
        /// <summary>
        /// Keep the most recent attempt.
        /// </summary>
        [ApiRepresentation("keep_latest")]
        KeepLatest
    }

    /// <summary>
    /// Represents the possible values for <see cref="Quiz.HideResults"/> in a <see cref="Quiz"/>.
    /// </summary>
    [PublicAPI]
    public enum HideResults : byte {
        /// <summary>
        /// Do not hide results.
        /// </summary>
        [ApiRepresentation("never")]
        Never,
        /// <summary>
        /// Hide results.
        /// </summary>
        [ApiRepresentation("always")]
        Always,
        /// <summary>
        /// Hide results until the student has used all of their quiz attempts.
        /// </summary>
        [ApiRepresentation("until_after_last_attempt")]
        UntilAfterLastAttempt
    }
}
