using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Assignments {
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