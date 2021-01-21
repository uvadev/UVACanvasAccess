using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.SisImports {

    internal class SisImportDataModel {
        
        [JsonProperty("import_type")]
        public string ImportType { get; set; }
        
        [JsonProperty("supplied_batches")]
        public IEnumerable<string> SuppliedBatches { get; set; }

        [JsonProperty("counts")]
        [CanBeNull] 
        public SisImportCountsModel Counts { get; set; }
    }
}
