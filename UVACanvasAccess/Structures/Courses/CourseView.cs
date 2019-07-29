using JetBrains.Annotations;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Courses {
    
    [PublicAPI]
    public enum CourseView : byte {
        [ApiRepresentation("feed")]
        Feed,
        [ApiRepresentation("wiki")]
        Wiki,
        [ApiRepresentation("modules")]
        Modules,
        [ApiRepresentation("syllabus")]
        Syllabus,
        [ApiRepresentation("assignments")]
        Assignments
    }
}
