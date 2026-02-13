using JetBrains.Annotations;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.BlueprintCourses {
    
    [PublicAPI]
    public enum BlueprintRestrictionType {
        [ApiRepresentation("content")]
        Content,
        [ApiRepresentation("points")]
        Points,
        [ApiRepresentation("due_dates")]
        DueDates,
        [ApiRepresentation("availability_dates")]
        AvailabilityDates
    }
}
