using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Assignments {
    
    /// <summary>
    /// Represents the type of grading system used by an assignment.
    /// </summary>
    public enum GradingType {
        [ApiRepresentation("points")]
        Points,
        [ApiRepresentation("gpa_scale")]
        GpaScale,
        [ApiRepresentation("letter_grade")]
        LetterGrade,
        [ApiRepresentation("percent")]
        Percent,
        [ApiRepresentation("pass_fail")]
        PassFail
    }
}