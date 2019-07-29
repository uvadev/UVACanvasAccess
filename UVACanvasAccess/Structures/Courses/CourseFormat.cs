using JetBrains.Annotations;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Courses {
    
    [PublicAPI]
    public enum CourseFormat {
        [ApiRepresentation("on_campus")]
        OnCampus,
        [ApiRepresentation("online")]
        Online,
        [ApiRepresentation("blended")]
        Blended
    }
}
