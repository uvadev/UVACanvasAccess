using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Quizzes;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Quizzes {
    
    /// <summary>
    /// Represents a formatted quiz submission answer.
    /// </summary>
    [PublicAPI]
    public class QuizSubmissionFormattedAnswer : IPrettyPrint {
        private readonly Api api;
        
        public string FormattedAnswer { get; }

        internal QuizSubmissionFormattedAnswer(Api api, QuizSubmissionFormattedAnswerModel model) {
            this.api = api;
            FormattedAnswer = model.FormattedAnswer;
        }

        public string ToPrettyString() {
            return "QuizSubmissionFormattedAnswer {" +
                   ($"\n{nameof(FormattedAnswer)}: {FormattedAnswer}").Indent(4) +
                   "\n}";
        }
    }
}
