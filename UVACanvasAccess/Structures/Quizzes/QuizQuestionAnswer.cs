using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Quizzes;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Quizzes {
    
    /// <summary>
    /// Represents an answer option for a quiz question.
    /// </summary>
    [PublicAPI]
    public abstract class QuizQuestionAnswer : IPrettyPrint {
        protected readonly Api Api;
        
        public ulong Id { get; }
        
        public string AnswerText { get; }
        
        public decimal? AnswerWeight { get; }
        
        public string AnswerComments { get; }
        
        public string TextAfterAnswers { get; }
        
        internal QuizQuestionAnswer(Api api, QuizQuestionAnswerModel model) {
            Api = api;
            Id = model.Id;
            AnswerText = model.AnswerText ?? model.Text;
            AnswerWeight = model.AnswerWeight;
            AnswerComments = model.AnswerComments;
            TextAfterAnswers = model.TextAfterAnswers;
        }

        internal static QuizQuestionAnswer Create(Api api, QuizQuestionAnswerModel model, QuizQuestionType? questionType) {
            switch (questionType) {
                case QuizQuestionType.MatchingQuestion:
                    return new MatchingQuizQuestionAnswer(api, model);
                case QuizQuestionType.NumericalQuestion:
                    return new NumericalQuizQuestionAnswer(api, model);
                case QuizQuestionType.FillInMultipleBlanksQuestion:
                case QuizQuestionType.MultipleDropdownsQuestion:
                    return new BlankQuizQuestionAnswer(api, model);
                default:
                    return new ChoiceQuizQuestionAnswer(api, model);
            }
        }

        public abstract string ToPrettyString();
    }

    /// <summary>
    /// An answer for choice-like question types.
    /// </summary>
    [PublicAPI]
    public class ChoiceQuizQuestionAnswer : QuizQuestionAnswer {
        internal ChoiceQuizQuestionAnswer(Api api, QuizQuestionAnswerModel model) : base(api, model) { }

        public override string ToPrettyString() {
            return "ChoiceQuizQuestionAnswer {" +
                   ($"\n{nameof(Id)}: {Id}," +
                    $"\n{nameof(AnswerText)}: {AnswerText}," +
                    $"\n{nameof(AnswerWeight)}: {AnswerWeight}," +
                    $"\n{nameof(AnswerComments)}: {AnswerComments}," +
                    $"\n{nameof(TextAfterAnswers)}: {TextAfterAnswers}").Indent(4) +
                   "\n}";
        }
    }

    /// <summary>
    /// An answer for matching questions.
    /// </summary>
    [PublicAPI]
    public class MatchingQuizQuestionAnswer : QuizQuestionAnswer {
        public string AnswerMatchLeft { get; }
        
        public string AnswerMatchRight { get; }
        
        public string MatchingAnswerIncorrectMatches { get; }

        internal MatchingQuizQuestionAnswer(Api api, QuizQuestionAnswerModel model) : base(api, model) {
            AnswerMatchLeft = model.AnswerMatchLeft;
            AnswerMatchRight = model.AnswerMatchRight;
            MatchingAnswerIncorrectMatches = model.MatchingAnswerIncorrectMatches;
        }

        public override string ToPrettyString() {
            return "MatchingQuizQuestionAnswer {" +
                   ($"\n{nameof(Id)}: {Id}," +
                    $"\n{nameof(AnswerMatchLeft)}: {AnswerMatchLeft}," +
                    $"\n{nameof(AnswerMatchRight)}: {AnswerMatchRight}," +
                    $"\n{nameof(MatchingAnswerIncorrectMatches)}: {MatchingAnswerIncorrectMatches}," +
                    $"\n{nameof(AnswerWeight)}: {AnswerWeight}," +
                    $"\n{nameof(AnswerComments)}: {AnswerComments}," +
                    $"\n{nameof(TextAfterAnswers)}: {TextAfterAnswers}").Indent(4) +
                   "\n}";
        }
    }

    /// <summary>
    /// An answer for numerical questions.
    /// </summary>
    [PublicAPI]
    public class NumericalQuizQuestionAnswer : QuizQuestionAnswer {
        public QuizNumericalAnswerType? NumericalAnswerType { get; }
        
        public decimal? Exact { get; }
        
        public decimal? Margin { get; }
        
        public decimal? Approximate { get; }
        
        public decimal? Precision { get; }
        
        public decimal? Start { get; }
        
        public decimal? End { get; }

        internal NumericalQuizQuestionAnswer(Api api, QuizQuestionAnswerModel model) : base(api, model) {
            NumericalAnswerType = model.NumericalAnswerType?.ToApiRepresentedEnum<QuizNumericalAnswerType>();
            Exact = model.Exact;
            Margin = model.Margin;
            Approximate = model.Approximate;
            Precision = model.Precision;
            Start = model.Start;
            End = model.End;
        }

        public override string ToPrettyString() {
            return "NumericalQuizQuestionAnswer {" +
                   ($"\n{nameof(Id)}: {Id}," +
                    $"\n{nameof(NumericalAnswerType)}: {NumericalAnswerType}," +
                    $"\n{nameof(Exact)}: {Exact}," +
                    $"\n{nameof(Margin)}: {Margin}," +
                    $"\n{nameof(Approximate)}: {Approximate}," +
                    $"\n{nameof(Precision)}: {Precision}," +
                    $"\n{nameof(Start)}: {Start}," +
                    $"\n{nameof(End)}: {End}," +
                    $"\n{nameof(AnswerWeight)}: {AnswerWeight}," +
                    $"\n{nameof(AnswerComments)}: {AnswerComments}," +
                    $"\n{nameof(TextAfterAnswers)}: {TextAfterAnswers}").Indent(4) +
                   "\n}";
        }
    }

    /// <summary>
    /// An answer for fill-in-the-blank and dropdown questions.
    /// </summary>
    [PublicAPI]
    public class BlankQuizQuestionAnswer : QuizQuestionAnswer {
        public ulong? BlankId { get; }

        internal BlankQuizQuestionAnswer(Api api, QuizQuestionAnswerModel model) : base(api, model) {
            BlankId = model.BlankId;
        }

        public override string ToPrettyString() {
            return "BlankQuizQuestionAnswer {" +
                   ($"\n{nameof(Id)}: {Id}," +
                    $"\n{nameof(BlankId)}: {BlankId}," +
                    $"\n{nameof(AnswerText)}: {AnswerText}," +
                    $"\n{nameof(AnswerWeight)}: {AnswerWeight}," +
                    $"\n{nameof(AnswerComments)}: {AnswerComments}," +
                    $"\n{nameof(TextAfterAnswers)}: {TextAfterAnswers}").Indent(4) +
                   "\n}";
        }
    }
    
    /// <summary>
    /// Types of numerical answers for numerical questions.
    /// </summary>
    [PublicAPI]
    public enum QuizNumericalAnswerType : byte {
        [ApiRepresentation("exact_answer")]
        ExactAnswer,
        [ApiRepresentation("range_answer")]
        RangeAnswer,
        [ApiRepresentation("precision_answer")]
        PrecisionAnswer
    }
}
