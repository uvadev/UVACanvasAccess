using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Quizzes {
    
    /// <summary>
    /// Builder for quiz questions.
    /// </summary>
    [PublicAPI]
    public abstract class QuizQuestionBuilder {
        private readonly Api api;
        private readonly bool isEditing;
        
        protected readonly ulong CourseId;
        protected readonly ulong QuizId;
        protected readonly ulong? QuestionId;
        protected readonly QuizQuestionType QuestionType;
        protected readonly string QuestionName;
        protected readonly string QuestionText;
        
        private decimal? pointsPossible;
        private string correctComments;
        private string incorrectComments;
        private string neutralComments;
        private string textAfterAnswers;
        private readonly List<QuizQuestionAnswerInput> answers = new List<QuizQuestionAnswerInput>();

        protected QuizQuestionBuilder(Api api,
                                      bool isEditing,
                                      ulong courseId,
                                      ulong quizId,
                                      ulong? questionId,
                                      QuizQuestionType questionType,
                                      [NotNull] string questionName,
                                      [NotNull] string questionText) {
            this.api = api;
            this.isEditing = isEditing;
            CourseId = courseId;
            QuizId = quizId;
            QuestionId = questionId;
            QuestionType = questionType;
            QuestionName = questionName;
            QuestionText = questionText;
        }
        
        public QuizQuestionBuilder WithPointsPossible(decimal points) {
            pointsPossible = points;
            return this;
        }
        
        public QuizQuestionBuilder WithCorrectComments([NotNull] string comments) {
            correctComments = comments;
            return this;
        }
        
        public QuizQuestionBuilder WithIncorrectComments([NotNull] string comments) {
            incorrectComments = comments;
            return this;
        }
        
        public QuizQuestionBuilder WithNeutralComments([NotNull] string comments) {
            neutralComments = comments;
            return this;
        }
        
        public QuizQuestionBuilder WithTextAfterAnswers([NotNull] string text) {
            textAfterAnswers = text;
            return this;
        }
        
        protected QuizQuestionBuilder AddAnswer([NotNull] QuizQuestionAnswerInput answer) {
            answers.Add(answer);
            return this;
        }
        
        protected QuizQuestionBuilder AddAnswers([NotNull] IEnumerable<QuizQuestionAnswerInput> answerList) {
            answers.AddRange(answerList);
            return this;
        }
        
        internal JObject ToJson() {
            var question = new JObject {
                ["question_name"] = QuestionName,
                ["question_type"] = QuestionType.GetApiRepresentation(),
                ["question_text"] = QuestionText
            };
            
            if (pointsPossible != null) {
                question["points_possible"] = pointsPossible;
            }
            
            if (correctComments != null) {
                question["correct_comments"] = correctComments;
            }
            
            if (incorrectComments != null) {
                question["incorrect_comments"] = incorrectComments;
            }
            
            if (neutralComments != null) {
                question["neutral_comments"] = neutralComments;
            }
            
            if (textAfterAnswers != null) {
                question["text_after_answers"] = textAfterAnswers;
            }
            
            if (answers.Any()) {
                question["answers"] = new JArray(answers.Select(a => a.ToJson()));
            }
            
            return new JObject { ["question"] = question };
        }
        
        /// <summary>
        /// Performs the operation using the fields in this builder.
        /// </summary>
        /// <returns>The created or updated quiz question.</returns>
        public Task<QuizQuestion> Post() {
            if (!isEditing) {
                return api.PostCreateQuizQuestion(CourseId, QuizId, this);
            }
            
            Debug.Assert(QuestionId != null, nameof(QuestionId) + " != null");
            return api.PutUpdateQuizQuestion(CourseId, QuizId, (ulong) QuestionId, this);
        }

        internal static QuizQuestionBuilder Create(Api api,
                                                   bool isEditing,
                                                   ulong courseId,
                                                   ulong quizId,
                                                   ulong? questionId,
                                                   QuizQuestionType questionType,
                                                   [NotNull] string questionName,
                                                   [NotNull] string questionText) {
            switch (questionType) {
                case QuizQuestionType.CalculatedQuestion:
                    return new CalculatedQuizQuestionBuilder(api, isEditing, courseId, quizId, questionId, questionName, questionText);
                case QuizQuestionType.EssayQuestion:
                    return new EssayQuizQuestionBuilder(api, isEditing, courseId, quizId, questionId, questionName, questionText);
                case QuizQuestionType.FileUploadQuestion:
                    return new FileUploadQuizQuestionBuilder(api, isEditing, courseId, quizId, questionId, questionName, questionText);
                case QuizQuestionType.FillInMultipleBlanksQuestion:
                    return new FillInMultipleBlanksQuizQuestionBuilder(api, isEditing, courseId, quizId, questionId, questionName, questionText);
                case QuizQuestionType.MatchingQuestion:
                    return new MatchingQuizQuestionBuilder(api, isEditing, courseId, quizId, questionId, questionName, questionText);
                case QuizQuestionType.MultipleAnswersQuestion:
                    return new MultipleAnswersQuizQuestionBuilder(api, isEditing, courseId, quizId, questionId, questionName, questionText);
                case QuizQuestionType.MultipleChoiceQuestion:
                    return new MultipleChoiceQuizQuestionBuilder(api, isEditing, courseId, quizId, questionId, questionName, questionText);
                case QuizQuestionType.MultipleDropdownsQuestion:
                    return new MultipleDropdownsQuizQuestionBuilder(api, isEditing, courseId, quizId, questionId, questionName, questionText);
                case QuizQuestionType.NumericalQuestion:
                    return new NumericalQuizQuestionBuilder(api, isEditing, courseId, quizId, questionId, questionName, questionText);
                case QuizQuestionType.ShortAnswerQuestion:
                    return new ShortAnswerQuizQuestionBuilder(api, isEditing, courseId, quizId, questionId, questionName, questionText);
                case QuizQuestionType.TextOnlyQuestion:
                    return new TextOnlyQuizQuestionBuilder(api, isEditing, courseId, quizId, questionId, questionName, questionText);
                case QuizQuestionType.TrueFalseQuestion:
                    return new TrueFalseQuizQuestionBuilder(api, isEditing, courseId, quizId, questionId, questionName, questionText);
                default:
                    return new GenericQuizQuestionBuilder(api, isEditing, courseId, quizId, questionId, questionName, questionText);
            }
        }
    }

    [PublicAPI]
    public class CalculatedQuizQuestionBuilder : QuizQuestionBuilder {
        public CalculatedQuizQuestionBuilder(Api api, bool isEditing, ulong courseId, ulong quizId, ulong? questionId, string questionName, string questionText)
            : base(api, isEditing, courseId, quizId, questionId, QuizQuestionType.CalculatedQuestion, questionName, questionText) { }
        
        public CalculatedQuizQuestionBuilder WithAnswer([NotNull] NumericalQuizQuestionAnswerInput answer) {
            AddAnswer(answer);
            return this;
        }
        
        public CalculatedQuizQuestionBuilder WithAnswers([NotNull] IEnumerable<NumericalQuizQuestionAnswerInput> answers) {
            AddAnswers(answers);
            return this;
        }
    }

    [PublicAPI]
    public class EssayQuizQuestionBuilder : QuizQuestionBuilder {
        public EssayQuizQuestionBuilder(Api api, bool isEditing, ulong courseId, ulong quizId, ulong? questionId, string questionName, string questionText)
            : base(api, isEditing, courseId, quizId, questionId, QuizQuestionType.EssayQuestion, questionName, questionText) { }
    }

    [PublicAPI]
    public class FileUploadQuizQuestionBuilder : QuizQuestionBuilder {
        public FileUploadQuizQuestionBuilder(Api api, bool isEditing, ulong courseId, ulong quizId, ulong? questionId, string questionName, string questionText)
            : base(api, isEditing, courseId, quizId, questionId, QuizQuestionType.FileUploadQuestion, questionName, questionText) { }
    }

    [PublicAPI]
    public class FillInMultipleBlanksQuizQuestionBuilder : QuizQuestionBuilder {
        public FillInMultipleBlanksQuizQuestionBuilder(Api api, bool isEditing, ulong courseId, ulong quizId, ulong? questionId, string questionName, string questionText)
            : base(api, isEditing, courseId, quizId, questionId, QuizQuestionType.FillInMultipleBlanksQuestion, questionName, questionText) { }
        
        public FillInMultipleBlanksQuizQuestionBuilder WithAnswer([NotNull] BlankQuizQuestionAnswerInput answer) {
            AddAnswer(answer);
            return this;
        }
        
        public FillInMultipleBlanksQuizQuestionBuilder WithAnswers([NotNull] IEnumerable<BlankQuizQuestionAnswerInput> answers) {
            AddAnswers(answers);
            return this;
        }
    }

    [PublicAPI]
    public class MatchingQuizQuestionBuilder : QuizQuestionBuilder {
        public MatchingQuizQuestionBuilder(Api api, bool isEditing, ulong courseId, ulong quizId, ulong? questionId, string questionName, string questionText)
            : base(api, isEditing, courseId, quizId, questionId, QuizQuestionType.MatchingQuestion, questionName, questionText) { }
        
        public MatchingQuizQuestionBuilder WithAnswer([NotNull] MatchingQuizQuestionAnswerInput answer) {
            AddAnswer(answer);
            return this;
        }
        
        public MatchingQuizQuestionBuilder WithAnswers([NotNull] IEnumerable<MatchingQuizQuestionAnswerInput> answers) {
            AddAnswers(answers);
            return this;
        }
    }

    [PublicAPI]
    public class MultipleAnswersQuizQuestionBuilder : QuizQuestionBuilder {
        public MultipleAnswersQuizQuestionBuilder(Api api, bool isEditing, ulong courseId, ulong quizId, ulong? questionId, string questionName, string questionText)
            : base(api, isEditing, courseId, quizId, questionId, QuizQuestionType.MultipleAnswersQuestion, questionName, questionText) { }
        
        public MultipleAnswersQuizQuestionBuilder WithAnswer([NotNull] ChoiceQuizQuestionAnswerInput answer) {
            AddAnswer(answer);
            return this;
        }
        
        public MultipleAnswersQuizQuestionBuilder WithAnswers([NotNull] IEnumerable<ChoiceQuizQuestionAnswerInput> answers) {
            AddAnswers(answers);
            return this;
        }
    }

    [PublicAPI]
    public class MultipleChoiceQuizQuestionBuilder : QuizQuestionBuilder {
        public MultipleChoiceQuizQuestionBuilder(Api api, bool isEditing, ulong courseId, ulong quizId, ulong? questionId, string questionName, string questionText)
            : base(api, isEditing, courseId, quizId, questionId, QuizQuestionType.MultipleChoiceQuestion, questionName, questionText) { }
        
        public MultipleChoiceQuizQuestionBuilder WithAnswer([NotNull] ChoiceQuizQuestionAnswerInput answer) {
            AddAnswer(answer);
            return this;
        }
        
        public MultipleChoiceQuizQuestionBuilder WithAnswers([NotNull] IEnumerable<ChoiceQuizQuestionAnswerInput> answers) {
            AddAnswers(answers);
            return this;
        }
    }

    [PublicAPI]
    public class MultipleDropdownsQuizQuestionBuilder : QuizQuestionBuilder {
        public MultipleDropdownsQuizQuestionBuilder(Api api, bool isEditing, ulong courseId, ulong quizId, ulong? questionId, string questionName, string questionText)
            : base(api, isEditing, courseId, quizId, questionId, QuizQuestionType.MultipleDropdownsQuestion, questionName, questionText) { }
        
        public MultipleDropdownsQuizQuestionBuilder WithAnswer([NotNull] BlankQuizQuestionAnswerInput answer) {
            AddAnswer(answer);
            return this;
        }
        
        public MultipleDropdownsQuizQuestionBuilder WithAnswers([NotNull] IEnumerable<BlankQuizQuestionAnswerInput> answers) {
            AddAnswers(answers);
            return this;
        }
    }

    [PublicAPI]
    public class NumericalQuizQuestionBuilder : QuizQuestionBuilder {
        public NumericalQuizQuestionBuilder(Api api, bool isEditing, ulong courseId, ulong quizId, ulong? questionId, string questionName, string questionText)
            : base(api, isEditing, courseId, quizId, questionId, QuizQuestionType.NumericalQuestion, questionName, questionText) { }
        
        public NumericalQuizQuestionBuilder WithAnswer([NotNull] NumericalQuizQuestionAnswerInput answer) {
            AddAnswer(answer);
            return this;
        }
        
        public NumericalQuizQuestionBuilder WithAnswers([NotNull] IEnumerable<NumericalQuizQuestionAnswerInput> answers) {
            AddAnswers(answers);
            return this;
        }
    }

    [PublicAPI]
    public class ShortAnswerQuizQuestionBuilder : QuizQuestionBuilder {
        public ShortAnswerQuizQuestionBuilder(Api api, bool isEditing, ulong courseId, ulong quizId, ulong? questionId, string questionName, string questionText)
            : base(api, isEditing, courseId, quizId, questionId, QuizQuestionType.ShortAnswerQuestion, questionName, questionText) { }
        
        public ShortAnswerQuizQuestionBuilder WithAnswer([NotNull] ChoiceQuizQuestionAnswerInput answer) {
            AddAnswer(answer);
            return this;
        }
        
        public ShortAnswerQuizQuestionBuilder WithAnswers([NotNull] IEnumerable<ChoiceQuizQuestionAnswerInput> answers) {
            AddAnswers(answers);
            return this;
        }
    }

    [PublicAPI]
    public class TextOnlyQuizQuestionBuilder : QuizQuestionBuilder {
        public TextOnlyQuizQuestionBuilder(Api api, bool isEditing, ulong courseId, ulong quizId, ulong? questionId, string questionName, string questionText)
            : base(api, isEditing, courseId, quizId, questionId, QuizQuestionType.TextOnlyQuestion, questionName, questionText) { }
    }

    [PublicAPI]
    public class TrueFalseQuizQuestionBuilder : QuizQuestionBuilder {
        public TrueFalseQuizQuestionBuilder(Api api, bool isEditing, ulong courseId, ulong quizId, ulong? questionId, string questionName, string questionText)
            : base(api, isEditing, courseId, quizId, questionId, QuizQuestionType.TrueFalseQuestion, questionName, questionText) { }
        
        public TrueFalseQuizQuestionBuilder WithAnswer([NotNull] ChoiceQuizQuestionAnswerInput answer) {
            AddAnswer(answer);
            return this;
        }
        
        public TrueFalseQuizQuestionBuilder WithAnswers([NotNull] IEnumerable<ChoiceQuizQuestionAnswerInput> answers) {
            AddAnswers(answers);
            return this;
        }
    }

    [PublicAPI]
    public class GenericQuizQuestionBuilder : QuizQuestionBuilder {
        public GenericQuizQuestionBuilder(Api api, bool isEditing, ulong courseId, ulong quizId, ulong? questionId, string questionName, string questionText)
            : base(api, isEditing, courseId, quizId, questionId, QuizQuestionType.TextOnlyQuestion, questionName, questionText) { }
    }
}
