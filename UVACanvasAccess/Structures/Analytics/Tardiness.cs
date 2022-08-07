using JetBrains.Annotations;
using UVACanvasAccess.Model.Analytics;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Analytics {
    
    /// <summary>
    /// Represents a tardiness summary. Depending on the context, the values may be percentages out of 1 or absolute values.
    /// </summary>
    [PublicAPI]
    public class Tardiness : IPrettyPrint {
        public decimal Missing { get; }
        
        public decimal Late { get; }
        
        public decimal OnTime { get; }
        
        public decimal Floating { get; }
        
        public decimal Total { get; }

        internal Tardiness(TardinessModel model) {
            Missing = model.Missing;
            Late = model.Late;
            OnTime = model.OnTime;
            Floating = model.Floating;
            Total = model.Total;
        }

        /// <inheritdoc />
        public string ToPrettyString() {
            return "Tardiness {" + 
                   ($"\n{nameof(Missing)}: {Missing}," +
                    $"\n{nameof(Late)}: {Late}," +
                    $"\n{nameof(OnTime)}: {OnTime}," +
                    $"\n{nameof(Floating)}: {Floating}," +
                    $"\n{nameof(Total)}: {Total}").Indent(4) + 
                   "\n}";
        }
    }
}
