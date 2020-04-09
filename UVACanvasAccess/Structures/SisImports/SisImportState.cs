using JetBrains.Annotations;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.SisImports {
    
    [PublicAPI]
    public enum SisImportState {
        [ApiRepresentation("initializing")]
        Initializing,
        [ApiRepresentation("created")]
        Created,
        [ApiRepresentation("importing")]
        Importing,
        [ApiRepresentation("cleanup_batch")]
        CleanupBatch,
        [ApiRepresentation("imported")]
        Imported,
        [ApiRepresentation("imported_with_messages")]
        ImportedWithMessages,
        [ApiRepresentation("aborted")]
        Aborted,
        [ApiRepresentation("failed_with_messages")]
        FailedWithMessages,
        [ApiRepresentation("failed")]
        Failed,
        [ApiRepresentation("restoring")]
        Restoring,
        [ApiRepresentation("partially_restored")]
        PartiallyRestored,
        [ApiRepresentation("restored")]
        Restored,
        Invalid = -1
    }
}
