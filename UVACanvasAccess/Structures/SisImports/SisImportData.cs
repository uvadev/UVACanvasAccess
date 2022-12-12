using System.Collections.Generic;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.SisImports;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.SisImports {
    
    /// <summary>
    /// Represents some metadata of a <see cref="SisImport">SIS import</see>.
    /// </summary>
    [PublicAPI]
    public class SisImportData {
        private readonly Api api;
        
        /// <summary>
        /// The type of import.
        /// </summary>
        public string ImportType { get; set; }
        
        /// <summary>
        /// The import categories supplied to the import.
        /// </summary>
        public IEnumerable<string> SuppliedBatches { get; set; }

        /// <summary>
        /// Counts of processed rows per import category.
        /// </summary>
        [CanBeNull] 
        public SisImportCounts Counts { get; set; }

        internal SisImportData(Api api, SisImportDataModel model) {
            this.api = api;
            ImportType = model.ImportType;
            SuppliedBatches = model.SuppliedBatches;
            Counts = model.Counts.ConvertIfNotNull(c => new SisImportCounts(api, c));
        }

        /// <inheritdoc />
        public override string ToString() {
            return $"{nameof(ImportType)}: {ImportType}, {nameof(SuppliedBatches)}: {SuppliedBatches}, {nameof(Counts)}: {Counts}";
        }
    }
}
