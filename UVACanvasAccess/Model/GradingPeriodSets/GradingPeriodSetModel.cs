using Newtonsoft.Json;

namespace UVACanvasAccess.Model.GradingPeriodSets {
    internal class GradingPeriodSetModel {
        
        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("weighted")]
        public bool? Weighted { get; set; }
        
        [JsonProperty("display_totals_for_all_grading_periods")]
        public bool? DisplayTotalsForAllGradingPeriods { get; set; }
    }
}
