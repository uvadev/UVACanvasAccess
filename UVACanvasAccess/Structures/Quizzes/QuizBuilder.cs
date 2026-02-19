using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Quizzes {
    
    /// <summary>
    /// Builder for quiz creation and updates.
    /// </summary>
    [PublicAPI]
    public class QuizBuilder {
        private readonly Api api;
        private readonly bool isEditing;
        private readonly ulong courseId;
        private readonly ulong? quizId; // Iff editing
        
        private string title;
        private string description;
        private QuizType? quizType;
        private ulong? assignmentGroupId;
        private int? timeLimit;
        private bool? shuffleAnswers;
        private HideResults? hideResults;
        private bool? showCorrectAnswers;
        private bool? showCorrectAnswersLastAttempt;
        private DateTime? showCorrectAnswersAt;
        private DateTime? hideCorrectAnswersAt;
        private int? allowedAttempts;
        private ScoringPolicy? scoringPolicy;
        private bool? oneTimeResults;
        private bool? oneQuestionAtATime;
        private bool? cantGoBack;
        private string accessCode;
        private string ipFilter;
        private DateTime? dueAt;
        private DateTime? lockAt;
        private DateTime? unlockAt;
        private bool? published;
        private bool? onlyVisibleToOverrides;
        private bool? notifyOfUpdate;
        
        internal QuizBuilder(Api api, bool isEditing, ulong courseId, ulong? quizId = null) {
            this.api = api;
            this.isEditing = isEditing;
            this.courseId = courseId;
            this.quizId = quizId;
        }

        /// <summary>
        /// The quiz title.
        /// </summary>
        public QuizBuilder WithTitle([NotNull] string title) {
            this.title = title;
            return this;
        }

        /// <summary>
        /// The quiz description (HTML).
        /// </summary>
        public QuizBuilder WithDescription([CanBeNull] string description) {
            this.description = description;
            return this;
        }
        
        /// <summary>
        /// The quiz type.
        /// </summary>
        public QuizBuilder WithQuizType(QuizType quizType) {
            this.quizType = quizType;
            return this;
        }
        
        /// <summary>
        /// The assignment group id.
        /// </summary>
        public QuizBuilder WithAssignmentGroupId(ulong assignmentGroupId) {
            this.assignmentGroupId = assignmentGroupId;
            return this;
        }
        
        /// <summary>
        /// The quiz time limit, in minutes.
        /// </summary>
        public QuizBuilder WithTimeLimit(int timeLimit) {
            this.timeLimit = timeLimit;
            return this;
        }
        
        /// <summary>
        /// Whether to shuffle answers.
        /// </summary>
        public QuizBuilder WithShuffleAnswers(bool shuffleAnswers = true) {
            this.shuffleAnswers = shuffleAnswers;
            return this;
        }
        
        /// <summary>
        /// Whether to hide student results.
        /// </summary>
        /// <remarks>
        /// A null value omits the parameter, which indicates that results should not be hidden.
        /// </remarks>
        public QuizBuilder WithHideResults(HideResults? hideResults) {
            this.hideResults = hideResults;
            return this;
        }
        
        /// <summary>
        /// Whether to show correct answers in results.
        /// </summary>
        public QuizBuilder WithShowCorrectAnswers(bool showCorrectAnswers = true) {
            this.showCorrectAnswers = showCorrectAnswers;
            return this;
        }
        
        /// <summary>
        /// Whether to show correct answers only after the last attempt.
        /// </summary>
        public QuizBuilder WithShowCorrectAnswersLastAttempt(bool showCorrectAnswersLastAttempt = true) {
            this.showCorrectAnswersLastAttempt = showCorrectAnswersLastAttempt;
            return this;
        }
        
        /// <summary>
        /// When to begin showing correct answers.
        /// </summary>
        public QuizBuilder WithShowCorrectAnswersAt(DateTime showCorrectAnswersAt) {
            this.showCorrectAnswersAt = showCorrectAnswersAt;
            return this;
        }
        
        /// <summary>
        /// When to stop showing correct answers.
        /// </summary>
        public QuizBuilder WithHideCorrectAnswersAt(DateTime hideCorrectAnswersAt) {
            this.hideCorrectAnswersAt = hideCorrectAnswersAt;
            return this;
        }
        
        /// <summary>
        /// The number of allowed attempts. A value of -1 indicates unlimited attempts.
        /// </summary>
        public QuizBuilder WithAllowedAttempts(int allowedAttempts) {
            this.allowedAttempts = allowedAttempts;
            return this;
        }
        
        /// <summary>
        /// The scoring policy to use if multiple attempts are allowed.
        /// </summary>
        public QuizBuilder WithScoringPolicy(ScoringPolicy scoringPolicy) {
            this.scoringPolicy = scoringPolicy;
            return this;
        }
        
        /// <summary>
        /// If true, results will only be shown to students right after they submit their answers.
        /// </summary>
        public QuizBuilder WithOneTimeResults(bool oneTimeResults = true) {
            this.oneTimeResults = oneTimeResults;
            return this;
        }
        
        /// <summary>
        /// Whether one question should be shown at a time.
        /// </summary>
        public QuizBuilder WithOneQuestionAtATime(bool oneQuestionAtATime = true) {
            this.oneQuestionAtATime = oneQuestionAtATime;
            return this;
        }
        
        /// <summary>
        /// Whether students can go back to previous questions when <see cref="WithOneQuestionAtATime"/> is enabled.
        /// </summary>
        public QuizBuilder WithCantGoBack(bool cantGoBack = true) {
            this.cantGoBack = cantGoBack;
            return this;
        }
        
        /// <summary>
        /// The access code required to take the quiz.
        /// </summary>
        public QuizBuilder WithAccessCode([CanBeNull] string accessCode) {
            this.accessCode = accessCode;
            return this;
        }
        
        /// <summary>
        /// The IP address or range the quiz must be accessed from.
        /// </summary>
        public QuizBuilder WithIpFilter([CanBeNull] string ipFilter) {
            this.ipFilter = ipFilter;
            return this;
        }
        
        /// <summary>
        /// The quiz due date.
        /// </summary>
        public QuizBuilder WithDueAt(DateTime dueAt) {
            this.dueAt = dueAt;
            return this;
        }
        
        /// <summary>
        /// When the quiz locks.
        /// </summary>
        public QuizBuilder WithLockAt(DateTime lockAt) {
            this.lockAt = lockAt;
            return this;
        }
        
        /// <summary>
        /// When the quiz unlocks.
        /// </summary>
        public QuizBuilder WithUnlockAt(DateTime unlockAt) {
            this.unlockAt = unlockAt;
            return this;
        }
        
        /// <summary>
        /// Whether the quiz is published.
        /// </summary>
        public QuizBuilder WithPublished(bool published = true) {
            this.published = published;
            return this;
        }
        
        /// <summary>
        /// Whether the quiz is only visible to overrides.
        /// </summary>
        public QuizBuilder WithOnlyVisibleToOverrides(bool onlyVisibleToOverrides = true) {
            this.onlyVisibleToOverrides = onlyVisibleToOverrides;
            return this;
        }
        
        /// <summary>
        /// Whether to notify users that the quiz has changed.
        /// </summary>
        /// <remarks>
        /// Only applicable when updating an existing quiz.
        /// </remarks>
        public QuizBuilder WithNotifyOfUpdate(bool notifyOfUpdate = true) {
            this.notifyOfUpdate = notifyOfUpdate;
            return this;
        }
        
        internal IEnumerable<(string, string)> ToParams(bool includeNotifyOfUpdate) {
            var args = new List<(string, string)>();
            
            if (title != null) {
                args.Add(("quiz[title]", title));
            }
            
            if (description != null) {
                args.Add(("quiz[description]", description));
            }
            
            if (quizType != null) {
                args.Add(("quiz[quiz_type]", quizType.Value.GetApiRepresentation()));
            }
            
            if (assignmentGroupId != null) {
                args.Add(("quiz[assignment_group_id]", assignmentGroupId.ToString()));
            }
            
            if (timeLimit != null) {
                args.Add(("quiz[time_limit]", timeLimit.ToString()));
            }
            
            if (shuffleAnswers != null) {
                args.Add(("quiz[shuffle_answers]", shuffleAnswers.Value.ToShortString()));
            }
            
            if (hideResults != null) {
                args.Add(("quiz[hide_results]", hideResults.Value.GetApiRepresentation()));
            }
            
            if (showCorrectAnswers != null) {
                args.Add(("quiz[show_correct_answers]", showCorrectAnswers.Value.ToShortString()));
            }
            
            if (showCorrectAnswersLastAttempt != null) {
                args.Add(("quiz[show_correct_answers_last_attempt]", showCorrectAnswersLastAttempt.Value.ToShortString()));
            }
            
            if (showCorrectAnswersAt != null) {
                args.Add(("quiz[show_correct_answers_at]", showCorrectAnswersAt.ToString()));
            }
            
            if (hideCorrectAnswersAt != null) {
                args.Add(("quiz[hide_correct_answers_at]", hideCorrectAnswersAt.ToString()));
            }
            
            if (allowedAttempts != null) {
                args.Add(("quiz[allowed_attempts]", allowedAttempts.ToString()));
            }
            
            if (scoringPolicy != null) {
                args.Add(("quiz[scoring_policy]", scoringPolicy.Value.GetApiRepresentation()));
            }
            
            if (oneTimeResults != null) {
                args.Add(("quiz[one_time_results]", oneTimeResults.Value.ToShortString()));
            }
            
            if (oneQuestionAtATime != null) {
                args.Add(("quiz[one_question_at_a_time]", oneQuestionAtATime.Value.ToShortString()));
            }
            
            if (cantGoBack != null) {
                args.Add(("quiz[cant_go_back]", cantGoBack.Value.ToShortString()));
            }
            
            if (accessCode != null) {
                args.Add(("quiz[access_code]", accessCode));
            }
            
            if (ipFilter != null) {
                args.Add(("quiz[ip_filter]", ipFilter));
            }
            
            if (dueAt != null) {
                args.Add(("quiz[due_at]", dueAt.ToString()));
            }
            
            if (lockAt != null) {
                args.Add(("quiz[lock_at]", lockAt.ToString()));
            }
            
            if (unlockAt != null) {
                args.Add(("quiz[unlock_at]", unlockAt.ToString()));
            }
            
            if (published != null) {
                args.Add(("quiz[published]", published.Value.ToShortString()));
            }
            
            if (onlyVisibleToOverrides != null) {
                args.Add(("quiz[only_visible_to_overrides]", onlyVisibleToOverrides.Value.ToShortString()));
            }
            
            if (includeNotifyOfUpdate && notifyOfUpdate != null) {
                args.Add(("quiz[notify_of_update]", notifyOfUpdate.Value.ToShortString()));
            }
            
            return args;
        }

        /// <summary>
        /// Performs the operation using the fields in this builder.
        /// </summary>
        /// <returns>The created or updated quiz.</returns>
        public Task<Quiz> Post() {
            if (!isEditing) {
                return api.PostCreateQuiz(courseId, this);
            }
            
            Debug.Assert(quizId != null, nameof(quizId) + " != null");
            return api.PutUpdateQuiz(courseId, (ulong) quizId, this);
        }
    }
}
