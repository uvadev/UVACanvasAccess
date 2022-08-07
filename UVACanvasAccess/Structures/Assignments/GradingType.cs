using JetBrains.Annotations;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Assignments {
    
    /// <summary>
    /// Represents the type of grading system used by an assignment.
    /// </summary>
    [PublicAPI]
    public enum GradingType {
        /// <summary>
        /// Point-based grading system.
        /// </summary>
        [ApiRepresentation("points")]
        Points,
        /// <summary>
        /// GPA scale grading system.
        /// </summary>
        [ApiRepresentation("gpa_scale")]
        GpaScale,
        /// <summary>
        /// Letter grading system.
        /// </summary>
        [ApiRepresentation("letter_grade")]
        LetterGrade,
        /// <summary>
        /// Percent-based grading system.
        /// </summary>
        [ApiRepresentation("percent")]
        Percent,
        /// <summary>
        /// Pass/fail grading system.
        /// </summary>
        [ApiRepresentation("pass_fail")]
        PassFail
    }
}