using JetBrains.Annotations;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.BlueprintCourses {
    
    [PublicAPI]
    public enum BlueprintMigrationState {
        [ApiRepresentation("queued")]
        Queued,
        [ApiRepresentation("exporting")]
        Exporting,
        [ApiRepresentation("running")]
        Running,
        [ApiRepresentation("imports_queued")]
        ImportsQueued,
        [ApiRepresentation("completed")]
        Completed,
        [ApiRepresentation("exports_failed")]
        ExportsFailed,
        [ApiRepresentation("imports_failed")]
        ImportsFailed
    }
}
