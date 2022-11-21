using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.GradingPeriodSets;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.GradingPeriodSets {
    
    /// <summary>
    /// Represents a grading period set.
    /// </summary>
    [PublicAPI]
    public class GradingPeriodSet : IPrettyPrint {
        private readonly Api api;
        
        /// <summary>
        /// The set title.
        /// </summary>
        public string Title { get; }
        
        /// <summary>
        /// Whether the grading periods within this set are weighted.
        /// </summary>
        public bool? Weighted { get; }

        /// <summary>
        /// Whether the totals for all grading periods in this set are displayed.
        /// </summary>
        public bool? DisplayTotalsForAllGradingPeriods { get; }

        internal GradingPeriodSet(Api api, GradingPeriodSetModel model) {
            this.api = api;
            Title = model.Title;
            Weighted = model.Weighted;
            DisplayTotalsForAllGradingPeriods = model.DisplayTotalsForAllGradingPeriods;
        }

        /// <inheritdoc />
        public string ToPrettyString() {
            return "GradingPeriodSet {" + 
                  ($"\n{nameof(Title)}: {Title}," +
                   $"\n{nameof(Weighted)}: {Weighted}," +
                   $"\n{nameof(DisplayTotalsForAllGradingPeriods)}: {DisplayTotalsForAllGradingPeriods}").Indent(4) + 
                   "\n}";
        }
    }
}
