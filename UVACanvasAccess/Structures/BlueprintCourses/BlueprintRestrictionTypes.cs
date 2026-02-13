using System;
using JetBrains.Annotations;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.BlueprintCourses {
    
    [PublicAPI]
    [Flags]
    public enum BlueprintRestrictionTypes {
        None = 0,
        [ApiRepresentation("content")]
        Content = 1 << 0,
        [ApiRepresentation("points")]
        Points = 1 << 1,
        [ApiRepresentation("due_dates")]
        DueDates = 1 << 2,
        [ApiRepresentation("availability_dates")]
        AvailabilityDates = 1 << 3
    }
}
