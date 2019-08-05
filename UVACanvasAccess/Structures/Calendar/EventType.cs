using JetBrains.Annotations;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Calendar {
    
    [PublicAPI]
    public enum EventType {
        [ApiRepresentation("event")]
        Event, 
        [ApiRepresentation("assignment")]
        Assignment
    }
}
