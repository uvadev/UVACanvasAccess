using UVACanvasAccess.Model.Assignments;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Assignments {
    
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
    // ReSharper disable MemberCanBePrivate.Global
    public class NeedsGradingCount : IPrettyPrint {
        private readonly Api _api;
        
        public string SectionId { get; }
        
        public uint Count { get; }

        public NeedsGradingCount(Api api, NeedsGradingCountModel model) {
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