using JetBrains.Annotations;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.SisImports {
    
    /// <summary>
    /// The states that a <see cref="SisImport">SIS import</see> can take.
    /// </summary>
    [PublicAPI]
    public enum SisImportState {
        /// <summary>
        /// The import is initializing.
        /// </summary>
        [ApiRepresentation("initializing")]
        Initializing,
        /// <summary>
        /// The import has been created, but is not processing.
        /// </summary>
        [ApiRepresentation("created")]
        Created,
        /// <summary>
        /// The import is processing.
        /// </summary>
        [ApiRepresentation("importing")]
        Importing,
        /// <summary>
        /// The import is deleting items absent from a batch in batch mode.
        /// </summary>
        [ApiRepresentation("cleanup_batch")]
        CleanupBatch,
        /// <summary>
        /// The import has completed with no errors.
        /// </summary>
        [ApiRepresentation("imported")]
        Imported,
        /// <summary>
        /// The import has completed with errors.
        /// </summary>
        [ApiRepresentation("imported_with_messages")]
        ImportedWithMessages,
        /// <summary>
        /// The import was aborted before completion.
        /// </summary>
        [ApiRepresentation("aborted")]
        Aborted,
        /// <summary>
        /// The import failed before completion with errors.
        /// </summary>
        [ApiRepresentation("failed_with_messages")]
        FailedWithMessages,
        /// <summary>
        /// The import failed before completion.
        /// </summary>
        [ApiRepresentation("failed")]
        Failed,
        /// <summary>
        /// The import is restoring the states of items.
        /// </summary>
        [ApiRepresentation("restoring")]
        Restoring,
        /// <summary>
        /// The import has partially restored the states of items.
        /// </summary>
        [ApiRepresentation("partially_restored")]
        PartiallyRestored,
        /// <summary>
        /// The import has restored the states of items.
        /// </summary>
        [ApiRepresentation("restored")]
        Restored,
        Invalid = -1
    }
}
