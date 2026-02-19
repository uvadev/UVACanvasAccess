using JetBrains.Annotations;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Quizzes {
    
    /// <summary>
    /// An item to reorder within a quiz (either a question or a group).
    /// </summary>
    [PublicAPI]
    public class QuizReorderItem {
        
        /// <summary>
        /// The question or question group id.
        /// </summary>
        public ulong Id { get; }
        
        /// <summary>
        /// The type of the item being reordered.
        /// </summary>
        public QuizReorderItemType Type { get; }

        public QuizReorderItem(ulong id, QuizReorderItemType type) {
            Id = id;
            Type = type;
        }
    }

    /// <summary>
    /// The type of item being reordered in a quiz.
    /// </summary>
    [PublicAPI]
    public enum QuizReorderItemType : byte {
        /// <summary>
        /// A quiz question.
        /// </summary>
        [ApiRepresentation("question")]
        Question,
        /// <summary>
        /// A quiz question group.
        /// </summary>
        [ApiRepresentation("group")]
        Group
    }
}
