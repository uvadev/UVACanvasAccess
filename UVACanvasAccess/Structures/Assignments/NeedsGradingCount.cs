using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Assignments;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Assignments {
    
    /// <summary>
    /// A count of how many assignments need to be graded within a section.
    /// </summary>
    [PublicAPI]
    public class NeedsGradingCount : IPrettyPrint {
        private readonly Api api;
        
        /// <summary>
        /// The section id.
        /// </summary>
        public string SectionId { get; }
        
        /// <summary>
        /// The amount of ungraded assignments.
        /// </summary>
        public uint Count { get; }

        internal NeedsGradingCount(Api api, NeedsGradingCountModel model) {
            this.api = api;
            SectionId = model.SectionId;
            Count = model.NeedsGradingCount;
        }

        /// <inheritdoc />
        public string ToPrettyString() {
            return "NeedsGradingCount {" + 
                   ($"\n{nameof(SectionId)}: {SectionId}," +
                   $"\n{nameof(Count)}: {Count}").Indent(4) +
                   "\n}";
        }
    }
}