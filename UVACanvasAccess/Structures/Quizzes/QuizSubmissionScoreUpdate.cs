using JetBrains.Annotations;

namespace UVACanvasAccess.Structures.Quizzes {
    
    /// <summary>
    /// Represents a score update for a quiz submission question.
    /// </summary>
    [PublicAPI]
    public class QuizSubmissionScoreUpdate {
        
        public ulong QuestionId { get; }
        
        public decimal? Score { get; }
        
        public string Comment { get; }

        public QuizSubmissionScoreUpdate(ulong questionId, decimal? score = null, string comment = null) {
            QuestionId = questionId;
            Score = score;
            Comment = comment;
        }
    }
}
