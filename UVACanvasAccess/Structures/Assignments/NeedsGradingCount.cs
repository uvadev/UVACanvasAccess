using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Assignments;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Assignments {
    
    [PublicAPI]
    public class NeedsGradingCount : IPrettyPrint {
        private readonly Api _api;
        
        public string SectionId { get; }
        
        public uint Count { get; }

        internal NeedsGradingCount(Api api, NeedsGradingCountModel model) {
            _api = api;
            SectionId = model.SectionId;
            Count = model.NeedsGradingCount;
        }

        public string ToPrettyString() {
            return "NeedsGradingCount {" + 
                   ($"\n{nameof(SectionId)}: {SectionId}," +
                   $"\n{nameof(Count)}: {Count}").Indent(4) +
                   "\n}";
        }
    }
}