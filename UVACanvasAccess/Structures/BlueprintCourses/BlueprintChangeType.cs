using JetBrains.Annotations;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.BlueprintCourses {
    
    [PublicAPI]
    public enum BlueprintChangeType {
        [ApiRepresentation("created")]
        Created,
        [ApiRepresentation("updated")]
        Updated,
        [ApiRepresentation("deleted")]
        Deleted
    }
}
