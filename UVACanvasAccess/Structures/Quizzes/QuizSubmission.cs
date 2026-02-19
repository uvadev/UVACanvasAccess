using System;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Quizzes;
using UVACanvasAccess.Structures.Submissions;
using UVACanvasAccess.Structures.Users;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Quizzes {
    
    /// <summary>
    /// Represents a quiz submission.
    /// </summary>
    [PublicAPI]
    public class QuizSubmission : IPrettyPrint {
        private readonly Api api;
        
        public ulong? Id { get; }
        
        public ulong? QuizId { get; }
        
        public ulong? UserId { get; }
        
        public ulong? SubmissionId { get; }
        
        public decimal? Score { get; }
        
        public decimal? KeptScore { get; }
        
        public DateTime? StartedAt { get; }
        
        public DateTime? FinishedAt { get; }
        
        public DateTime? EndAt { get; }
        
        public uint? Attempt { get; }
        
        public uint? ExtraAttempts { get; }
        
        public uint? ExtraTime { get; }
        
        public bool? ManuallyUnlocked { get; }
        
        public uint? TimeSpent { get; }
        
        public decimal? ScoreBeforeRegrade { get; }
        
        public uint? TimesGraded { get; }
        
        public string WorkflowState { get; }
        
        public string ValidationToken { get; }
        
        public uint? QuizVersion { get; }
        
        public decimal? QuizPointsPossible { get; }
        
        public DateTime? QuizSubmittedAt { get; }
        
        [CanBeNull]
        public UserDisplay User { get; }
        
        [CanBeNull]
        public Quiz Quiz { get; }
        
        [CanBeNull]
        public Submission Submission { get; }
        
        public string HtmlUrl { get; }

        internal QuizSubmission(Api api, QuizSubmissionModel model) {
            this.api = api;
            Id = model.Id;
            QuizId = model.QuizId;
            UserId = model.UserId;
            SubmissionId = model.SubmissionId;
            Score = model.Score;
            KeptScore = model.KeptScore;
            StartedAt = model.StartedAt;
            FinishedAt = model.FinishedAt;
            EndAt = model.EndAt;
            Attempt = model.Attempt;
            ExtraAttempts = model.ExtraAttempts;
            ExtraTime = model.ExtraTime;
            ManuallyUnlocked = model.ManuallyUnlocked;
            TimeSpent = model.TimeSpent;
            ScoreBeforeRegrade = model.ScoreBeforeRegrade;
            TimesGraded = model.TimesGraded;
            WorkflowState = model.WorkflowState;
            ValidationToken = model.ValidationToken;
            QuizVersion = model.QuizVersion;
            QuizPointsPossible = model.QuizPointsPossible;
            QuizSubmittedAt = model.QuizSubmittedAt;
            User = model.User.ConvertIfNotNull(m => new UserDisplay(api, m));
            Quiz = model.Quiz.ConvertIfNotNull(m => new Quiz(api, m));
            Submission = model.Submission.ConvertIfNotNull(m => new Submission(api, m));
            HtmlUrl = model.HtmlUrl;
        }

        public string ToPrettyString() {
            return "QuizSubmission {" +
                   ($"\n{nameof(Id)}: {Id}," +
                    $"\n{nameof(QuizId)}: {QuizId}," +
                    $"\n{nameof(UserId)}: {UserId}," +
                    $"\n{nameof(SubmissionId)}: {SubmissionId}," +
                    $"\n{nameof(Score)}: {Score}," +
                    $"\n{nameof(KeptScore)}: {KeptScore}," +
                    $"\n{nameof(StartedAt)}: {StartedAt}," +
                    $"\n{nameof(FinishedAt)}: {FinishedAt}," +
                    $"\n{nameof(EndAt)}: {EndAt}," +
                    $"\n{nameof(Attempt)}: {Attempt}," +
                    $"\n{nameof(ExtraAttempts)}: {ExtraAttempts}," +
                    $"\n{nameof(ExtraTime)}: {ExtraTime}," +
                    $"\n{nameof(ManuallyUnlocked)}: {ManuallyUnlocked}," +
                    $"\n{nameof(TimeSpent)}: {TimeSpent}," +
                    $"\n{nameof(ScoreBeforeRegrade)}: {ScoreBeforeRegrade}," +
                    $"\n{nameof(TimesGraded)}: {TimesGraded}," +
                    $"\n{nameof(WorkflowState)}: {WorkflowState}," +
                    $"\n{nameof(ValidationToken)}: {ValidationToken}," +
                    $"\n{nameof(QuizVersion)}: {QuizVersion}," +
                    $"\n{nameof(QuizPointsPossible)}: {QuizPointsPossible}," +
                    $"\n{nameof(QuizSubmittedAt)}: {QuizSubmittedAt}," +
                    $"\n{nameof(User)}: {User?.ToPrettyString()}," +
                    $"\n{nameof(Quiz)}: {Quiz?.ToPrettyString()}," +
                    $"\n{nameof(Submission)}: {Submission?.ToPrettyString()}," +
                    $"\n{nameof(HtmlUrl)}: {HtmlUrl}").Indent(4) +
                   "\n}";
        }
    }
}
