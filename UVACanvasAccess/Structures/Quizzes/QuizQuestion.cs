using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Quizzes;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Quizzes {
    
    /// <summary>
    /// Represents a quiz question.
    /// </summary>
    [PublicAPI]
    public abstract class QuizQuestion : IPrettyPrint {
        protected readonly Api Api;
        
        public ulong Id { get; }
        
        public ulong QuizId { get; }
        
        public ulong? QuizGroupId { get; }
        
        public uint? Position { get; }
        
        public string QuestionName { get; }
        
        public QuizQuestionType? QuestionType { get; }
        
        public string QuestionText { get; }
        
        public decimal? PointsPossible { get; }
        
        public string CorrectComments { get; }
        
        public string IncorrectComments { get; }
        
        public string NeutralComments { get; }
        
        public string TextAfterAnswers { get; }
        
        [CanBeNull]
        public IEnumerable<QuizQuestionAnswer> Answers { get; }

        internal QuizQuestion(Api api, QuizQuestionModel model, QuizQuestionType? questionType) {
            Api = api;
            Id = model.Id;
            QuizId = model.QuizId;
            QuizGroupId = model.QuizGroupId;
            Position = model.Position;
            QuestionName = model.QuestionName;
            QuestionType = questionType;
            QuestionText = model.QuestionText;
            PointsPossible = model.PointsPossible;
            CorrectComments = model.CorrectComments;
            IncorrectComments = model.IncorrectComments;
            NeutralComments = model.NeutralComments;
            TextAfterAnswers = model.TextAfterAnswers;
            Answers = model.Answers?.Select(a => QuizQuestionAnswer.Create(api, a, QuestionType));
        }

        internal static QuizQuestion Create(Api api, QuizQuestionModel model) {
            var questionType = model.QuestionType?.ToApiRepresentedEnum<QuizQuestionType>();
            
            switch (questionType) {
                case QuizQuestionType.CalculatedQuestion:
                    return new CalculatedQuizQuestion(api, model, questionType);
                case QuizQuestionType.EssayQuestion:
                    return new EssayQuizQuestion(api, model, questionType);
                case QuizQuestionType.FileUploadQuestion:
                    return new FileUploadQuizQuestion(api, model, questionType);
                case QuizQuestionType.FillInMultipleBlanksQuestion:
                    return new FillInMultipleBlanksQuizQuestion(api, model, questionType);
                case QuizQuestionType.MatchingQuestion:
                    return new MatchingQuizQuestion(api, model, questionType);
                case QuizQuestionType.MultipleAnswersQuestion:
                    return new MultipleAnswersQuizQuestion(api, model, questionType);
                case QuizQuestionType.MultipleChoiceQuestion:
                    return new MultipleChoiceQuizQuestion(api, model, questionType);
                case QuizQuestionType.MultipleDropdownsQuestion:
                    return new MultipleDropdownsQuizQuestion(api, model, questionType);
                case QuizQuestionType.NumericalQuestion:
                    return new NumericalQuizQuestion(api, model, questionType);
                case QuizQuestionType.ShortAnswerQuestion:
                    return new ShortAnswerQuizQuestion(api, model, questionType);
                case QuizQuestionType.TextOnlyQuestion:
                    return new TextOnlyQuizQuestion(api, model, questionType);
                case QuizQuestionType.TrueFalseQuestion:
                    return new TrueFalseQuizQuestion(api, model, questionType);
                default:
                    return new GenericQuizQuestion(api, model, questionType);
            }
        }

        public virtual string ToPrettyString() {
            return $"{GetType().Name} {{" +
                   ($"\n{nameof(Id)}: {Id}," +
                    $"\n{nameof(QuizId)}: {QuizId}," +
                    $"\n{nameof(QuizGroupId)}: {QuizGroupId}," +
                    $"\n{nameof(Position)}: {Position}," +
                    $"\n{nameof(QuestionName)}: {QuestionName}," +
                    $"\n{nameof(QuestionType)}: {QuestionType}," +
                    $"\n{nameof(QuestionText)}: {QuestionText}," +
                    $"\n{nameof(PointsPossible)}: {PointsPossible}," +
                    $"\n{nameof(CorrectComments)}: {CorrectComments}," +
                    $"\n{nameof(IncorrectComments)}: {IncorrectComments}," +
                    $"\n{nameof(NeutralComments)}: {NeutralComments}," +
                    $"\n{nameof(TextAfterAnswers)}: {TextAfterAnswers}," +
                    $"\n{nameof(Answers)}: {Answers?.ToPrettyString()}").Indent(4) +
                   "\n}";
        }
    }

    [PublicAPI]
    public class CalculatedQuizQuestion : QuizQuestion {
        internal CalculatedQuizQuestion(Api api, QuizQuestionModel model, QuizQuestionType? questionType) 
            : base(api, model, questionType) { }
    }

    [PublicAPI]
    public class EssayQuizQuestion : QuizQuestion {
        internal EssayQuizQuestion(Api api, QuizQuestionModel model, QuizQuestionType? questionType) 
            : base(api, model, questionType) { }
    }

    [PublicAPI]
    public class FileUploadQuizQuestion : QuizQuestion {
        internal FileUploadQuizQuestion(Api api, QuizQuestionModel model, QuizQuestionType? questionType) 
            : base(api, model, questionType) { }
    }

    [PublicAPI]
    public class FillInMultipleBlanksQuizQuestion : QuizQuestion {
        internal FillInMultipleBlanksQuizQuestion(Api api, QuizQuestionModel model, QuizQuestionType? questionType) 
            : base(api, model, questionType) { }
    }

    [PublicAPI]
    public class MatchingQuizQuestion : QuizQuestion {
        internal MatchingQuizQuestion(Api api, QuizQuestionModel model, QuizQuestionType? questionType) 
            : base(api, model, questionType) { }
    }

    [PublicAPI]
    public class MultipleAnswersQuizQuestion : QuizQuestion {
        internal MultipleAnswersQuizQuestion(Api api, QuizQuestionModel model, QuizQuestionType? questionType) 
            : base(api, model, questionType) { }
    }

    [PublicAPI]
    public class MultipleChoiceQuizQuestion : QuizQuestion {
        internal MultipleChoiceQuizQuestion(Api api, QuizQuestionModel model, QuizQuestionType? questionType) 
            : base(api, model, questionType) { }
    }

    [PublicAPI]
    public class MultipleDropdownsQuizQuestion : QuizQuestion {
        internal MultipleDropdownsQuizQuestion(Api api, QuizQuestionModel model, QuizQuestionType? questionType) 
            : base(api, model, questionType) { }
    }

    [PublicAPI]
    public class NumericalQuizQuestion : QuizQuestion {
        internal NumericalQuizQuestion(Api api, QuizQuestionModel model, QuizQuestionType? questionType) 
            : base(api, model, questionType) { }
    }

    [PublicAPI]
    public class ShortAnswerQuizQuestion : QuizQuestion {
        internal ShortAnswerQuizQuestion(Api api, QuizQuestionModel model, QuizQuestionType? questionType) 
            : base(api, model, questionType) { }
    }

    [PublicAPI]
    public class TextOnlyQuizQuestion : QuizQuestion {
        internal TextOnlyQuizQuestion(Api api, QuizQuestionModel model, QuizQuestionType? questionType) 
            : base(api, model, questionType) { }
    }

    [PublicAPI]
    public class TrueFalseQuizQuestion : QuizQuestion {
        internal TrueFalseQuizQuestion(Api api, QuizQuestionModel model, QuizQuestionType? questionType) 
            : base(api, model, questionType) { }
    }

    [PublicAPI]
    public class GenericQuizQuestion : QuizQuestion {
        internal GenericQuizQuestion(Api api, QuizQuestionModel model, QuizQuestionType? questionType) 
            : base(api, model, questionType) { }
    }

    /// <summary>
    /// The quiz question types.
    /// </summary>
    [PublicAPI]
    public enum QuizQuestionType : byte {
        [ApiRepresentation("calculated_question")]
        CalculatedQuestion,
        [ApiRepresentation("essay_question")]
        EssayQuestion,
        [ApiRepresentation("file_upload_question")]
        FileUploadQuestion,
        [ApiRepresentation("fill_in_multiple_blanks_question")]
        FillInMultipleBlanksQuestion,
        [ApiRepresentation("matching_question")]
        MatchingQuestion,
        [ApiRepresentation("multiple_answers_question")]
        MultipleAnswersQuestion,
        [ApiRepresentation("multiple_choice_question")]
        MultipleChoiceQuestion,
        [ApiRepresentation("multiple_dropdowns_question")]
        MultipleDropdownsQuestion,
        [ApiRepresentation("numerical_question")]
        NumericalQuestion,
        [ApiRepresentation("short_answer_question")]
        ShortAnswerQuestion,
        [ApiRepresentation("text_only_question")]
        TextOnlyQuestion,
        [ApiRepresentation("true_false_question")]
        TrueFalseQuestion
    }
}
