using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Quizzes;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Quizzes {
    
    /// <summary>
    /// Represents a submission question for a quiz submission.
    /// </summary>
    [PublicAPI]
    public class QuizSubmissionQuestion : IPrettyPrint {
        private readonly Api api;
        
        public ulong Id { get; }
        
        public bool? Flagged { get; }
        
        [CanBeNull]
        public QuizSubmissionQuestionAnswerValue Answer { get; }
        
        [CanBeNull]
        public IEnumerable<QuizQuestionAnswer> Answers { get; }
        
        [CanBeNull]
        public QuizQuestion QuizQuestion { get; }

        internal QuizSubmissionQuestion(Api api, QuizSubmissionQuestionModel model) {
            this.api = api;
            Id = model.Id;
            Flagged = model.Flagged;
            QuizQuestion = model.QuizQuestion.ConvertIfNotNull(m => QuizQuestion.Create(api, m));
            Answer = QuizSubmissionQuestionAnswerValue.Create(model.Answer, QuizQuestion?.QuestionType);
            Answers = model.Answers?.Select(a => QuizQuestionAnswer.Create(api, a, QuizQuestion?.QuestionType));
        }

        public string ToPrettyString() {
            return "QuizSubmissionQuestion {" +
                   ($"\n{nameof(Id)}: {Id}," +
                    $"\n{nameof(Flagged)}: {Flagged}," +
                    $"\n{nameof(Answer)}: {Answer?.ToPrettyString()}," +
                    $"\n{nameof(Answers)}: {Answers?.ToPrettyString()}," +
                    $"\n{nameof(QuizQuestion)}: {QuizQuestion?.ToPrettyString()}").Indent(4) +
                   "\n}";
        }
    }

    /// <summary>
    /// An answer definition for updating quiz submission questions.
    /// </summary>
    [PublicAPI]
    public class QuizSubmissionQuestionAnswer {
        
        public ulong QuestionId { get; }
        
        [NotNull]
        public QuizSubmissionQuestionAnswerValue Answer { get; }

        public QuizSubmissionQuestionAnswer(ulong questionId, [NotNull] QuizSubmissionQuestionAnswerValue answer) {
            QuestionId = questionId;
            Answer = answer;
        }
    }

    /// <summary>
    /// Additional data to include when retrieving submission questions.
    /// </summary>
    [PublicAPI]
    [System.Flags]
    public enum QuizSubmissionQuestionInclude {
        [ApiRepresentation("quiz_question")]
        QuizQuestion = 1 << 0
    }
}
