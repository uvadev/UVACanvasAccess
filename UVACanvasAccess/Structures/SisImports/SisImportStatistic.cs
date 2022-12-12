using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.SisImports;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.SisImports {
    
    /// <summary>
    /// An individual statistic for an item in <see cref="SisImportStatistics"/>.
    /// </summary>
    [PublicAPI]
    public class SisImportStatistic : IPrettyPrint {
        private readonly Api api;
        
        /// <summary>
        /// The number of items created.
        /// </summary>
        public ulong Created { get; }
        
        /// <summary>
        /// The number of course and enrollment items marked as concluded.
        /// </summary>
        public ulong Concluded { get; }

        /// <summary>
        /// The number of enrollment items marked as inactive.
        /// </summary>
        public ulong Deactivated { get; }
        
        /// <summary>
        /// The number of items restored from a completed, inactive, or deleted state.
        /// </summary>
        public ulong Restored { get; }
        
        /// <summary>
        /// The number of items deleted.
        /// </summary>
        public ulong Deleted { get; }

        internal SisImportStatistic(Api api, SisImportStatisticModel model) {
            this.api = api;
            Created = model.Created ?? 0;
            Concluded = model.Concluded ?? 0;
            Deactivated = model.Deactivated ?? 0;
            Restored = model.Restored ?? 0;
            Deleted = model.Deleted ?? 0;
        }

        /// <inheritdoc />
        public string ToPrettyString() {
            return "SisImportStatistic {" + 
                   ($"\n{nameof(Created)}: {Created}," +
                   $"\n{nameof(Concluded)}: {Concluded}," +
                   $"\n{nameof(Deactivated)}: {Deactivated}," +
                   $"\n{nameof(Restored)}: {Restored}," +
                   $"\n{nameof(Deleted)}: {Deleted}").Indent(4) +
                   "\n}";
        }
    }
}
