using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.SisImports;

namespace UVACanvasAccess.Structures.SisImports {
    
    [PublicAPI]
    public class SisImportStatistic {
        private readonly Api _api;
        
        public ulong Created { get; }
        
        public ulong Concluded { get; }
        
        public ulong Deactivated { get; }
        
        public ulong Restored { get; }
        
        public ulong Deleted { get; }

        internal SisImportStatistic(Api api, SisImportStatisticModel model) {
            _api = api;
            Created = model.Created ?? 0;
            Concluded = model.Concluded ?? 0;
            Deactivated = model.Deactivated ?? 0;
            Restored = model.Restored ?? 0;
            Deleted = model.Deleted ?? 0;
        }
    }
}
