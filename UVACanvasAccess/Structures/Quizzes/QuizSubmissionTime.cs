using System;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Quizzes;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Quizzes {
    
    /// <summary>
    /// Represents timing info for a quiz submission.
    /// </summary>
    [PublicAPI]
    public class QuizSubmissionTime : IPrettyPrint {
        private readonly Api api;
        
        public DateTime? EndAt { get; }
        
        public int? TimeLeft { get; }

        internal QuizSubmissionTime(Api api, QuizSubmissionTimeModel model) {
            this.api = api;
            EndAt = model.EndAt;
            TimeLeft = model.TimeLeft;
        }

        public string ToPrettyString() {
            return "QuizSubmissionTime {" +
                   ($"\n{nameof(EndAt)}: {EndAt}," +
                    $"\n{nameof(TimeLeft)}: {TimeLeft}").Indent(4) +
                   "\n}";
        }
    }
}
