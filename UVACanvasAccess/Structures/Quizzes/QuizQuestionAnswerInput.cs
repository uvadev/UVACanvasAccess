using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Quizzes {
    
    /// <summary>
    /// Base class for quiz question answer inputs.
    /// </summary>
    [PublicAPI]
    public abstract class QuizQuestionAnswerInput {
        internal abstract JObject ToJson();
    }

    /// <summary>
    /// Answer input for choice-like questions.
    /// </summary>
    [PublicAPI]
    public class ChoiceQuizQuestionAnswerInput : QuizQuestionAnswerInput {
        private readonly string answerText;
        private decimal? answerWeight;
        private string answerComments;

        public ChoiceQuizQuestionAnswerInput([NotNull] string answerText) {
            this.answerText = answerText;
        }

        public ChoiceQuizQuestionAnswerInput WithAnswerWeight(decimal weight) {
            answerWeight = weight;
            return this;
        }

        public ChoiceQuizQuestionAnswerInput WithAnswerComments([NotNull] string comments) {
            answerComments = comments;
            return this;
        }

        internal override JObject ToJson() {
            var answer = new JObject {
                ["answer_text"] = answerText
            };

            if (answerWeight != null) {
                answer["answer_weight"] = answerWeight;
            }

            if (answerComments != null) {
                answer["answer_comments"] = answerComments;
            }

            return answer;
        }
    }

    /// <summary>
    /// Answer input for matching questions.
    /// </summary>
    [PublicAPI]
    public class MatchingQuizQuestionAnswerInput : QuizQuestionAnswerInput {
        private readonly string answerMatchLeft;
        private readonly string answerMatchRight;
        private string matchingAnswerIncorrectMatches;
        private string answerComments;
        private decimal? answerWeight;

        public MatchingQuizQuestionAnswerInput([NotNull] string answerMatchLeft, [NotNull] string answerMatchRight) {
            this.answerMatchLeft = answerMatchLeft;
            this.answerMatchRight = answerMatchRight;
        }

        public MatchingQuizQuestionAnswerInput WithMatchingAnswerIncorrectMatches([NotNull] string value) {
            matchingAnswerIncorrectMatches = value;
            return this;
        }

        public MatchingQuizQuestionAnswerInput WithAnswerComments([NotNull] string comments) {
            answerComments = comments;
            return this;
        }

        public MatchingQuizQuestionAnswerInput WithAnswerWeight(decimal weight) {
            answerWeight = weight;
            return this;
        }

        internal override JObject ToJson() {
            var answer = new JObject {
                ["answer_match_left"] = answerMatchLeft,
                ["answer_match_right"] = answerMatchRight
            };

            if (matchingAnswerIncorrectMatches != null) {
                answer["matching_answer_incorrect_matches"] = matchingAnswerIncorrectMatches;
            }

            if (answerComments != null) {
                answer["answer_comments"] = answerComments;
            }

            if (answerWeight != null) {
                answer["answer_weight"] = answerWeight;
            }

            return answer;
        }
    }

    /// <summary>
    /// Base class for numerical question answer inputs.
    /// </summary>
    [PublicAPI]
    public abstract class NumericalQuizQuestionAnswerInput : QuizQuestionAnswerInput {
        private readonly QuizNumericalAnswerType numericalAnswerType;
        private string answerComments;
        private decimal? answerWeight;

        protected NumericalQuizQuestionAnswerInput(QuizNumericalAnswerType numericalAnswerType) {
            this.numericalAnswerType = numericalAnswerType;
        }

        public NumericalQuizQuestionAnswerInput WithAnswerComments([NotNull] string comments) {
            answerComments = comments;
            return this;
        }

        public NumericalQuizQuestionAnswerInput WithAnswerWeight(decimal weight) {
            answerWeight = weight;
            return this;
        }

        protected abstract void AddNumericalFields(JObject answer);

        internal override JObject ToJson() {
            var answer = new JObject {
                ["numerical_answer_type"] = numericalAnswerType.GetApiRepresentation()
            };

            AddNumericalFields(answer);

            if (answerComments != null) {
                answer["answer_comments"] = answerComments;
            }

            if (answerWeight != null) {
                answer["answer_weight"] = answerWeight;
            }

            return answer;
        }
    }

    /// <summary>
    /// Numerical answer input for exact answers.
    /// </summary>
    [PublicAPI]
    public class ExactNumericalQuizQuestionAnswerInput : NumericalQuizQuestionAnswerInput {
        private readonly decimal exact;
        private readonly decimal? margin;

        public ExactNumericalQuizQuestionAnswerInput(decimal exact, decimal? margin = null) : base(QuizNumericalAnswerType.ExactAnswer) {
            this.exact = exact;
            this.margin = margin;
        }

        protected override void AddNumericalFields(JObject answer) {
            answer["exact"] = exact;
            if (margin != null) {
                answer["margin"] = margin;
            }
        }
    }

    /// <summary>
    /// Numerical answer input for precision answers.
    /// </summary>
    [PublicAPI]
    public class PrecisionNumericalQuizQuestionAnswerInput : NumericalQuizQuestionAnswerInput {
        private readonly decimal approximate;
        private readonly decimal? precision;

        public PrecisionNumericalQuizQuestionAnswerInput(decimal approximate, decimal? precision = null) : base(QuizNumericalAnswerType.PrecisionAnswer) {
            this.approximate = approximate;
            this.precision = precision;
        }

        protected override void AddNumericalFields(JObject answer) {
            answer["approximate"] = approximate;
            if (precision != null) {
                answer["precision"] = precision;
            }
        }
    }

    /// <summary>
    /// Numerical answer input for range answers.
    /// </summary>
    [PublicAPI]
    public class RangeNumericalQuizQuestionAnswerInput : NumericalQuizQuestionAnswerInput {
        private readonly decimal start;
        private readonly decimal end;

        public RangeNumericalQuizQuestionAnswerInput(decimal start, decimal end) : base(QuizNumericalAnswerType.RangeAnswer) {
            this.start = start;
            this.end = end;
        }

        protected override void AddNumericalFields(JObject answer) {
            answer["start"] = start;
            answer["end"] = end;
        }
    }

    /// <summary>
    /// Answer input for fill-in-the-blank and dropdown questions.
    /// </summary>
    [PublicAPI]
    public class BlankQuizQuestionAnswerInput : QuizQuestionAnswerInput {
        private readonly ulong blankId;
        private readonly string answerText;
        private string answerComments;
        private decimal? answerWeight;

        public BlankQuizQuestionAnswerInput(ulong blankId, [NotNull] string answerText) {
            this.blankId = blankId;
            this.answerText = answerText;
        }

        public BlankQuizQuestionAnswerInput WithAnswerComments([NotNull] string comments) {
            answerComments = comments;
            return this;
        }

        public BlankQuizQuestionAnswerInput WithAnswerWeight(decimal weight) {
            answerWeight = weight;
            return this;
        }

        internal override JObject ToJson() {
            var answer = new JObject {
                ["blank_id"] = blankId,
                ["answer_text"] = answerText
            };

            if (answerComments != null) {
                answer["answer_comments"] = answerComments;
            }

            if (answerWeight != null) {
                answer["answer_weight"] = answerWeight;
            }

            return answer;
        }
    }
}
