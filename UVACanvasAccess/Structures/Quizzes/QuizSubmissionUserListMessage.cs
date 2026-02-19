using JetBrains.Annotations;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Quizzes {
    
    /// <summary>
    /// Defines a message to users in a quiz submission user list.
    /// </summary>
    [PublicAPI]
    public class QuizSubmissionUserListMessage {
        
        public string Subject { get; }
        
        public string Body { get; }
        
        public QuizSubmissionUserListRecipients Recipients { get; }

        public QuizSubmissionUserListMessage(string subject, string body, QuizSubmissionUserListRecipients recipients) {
            Subject = subject;
            Body = body;
            Recipients = recipients;
        }
    }

    /// <summary>
    /// Which users to message via the submission user list.
    /// </summary>
    [PublicAPI]
    public enum QuizSubmissionUserListRecipients {
        [ApiRepresentation("submitted")]
        Submitted,
        [ApiRepresentation("unsubmitted")]
        Unsubmitted
    }
}
