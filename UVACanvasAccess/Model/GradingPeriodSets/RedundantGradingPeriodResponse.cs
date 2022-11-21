using System.Collections.Generic;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.GradingPeriodSets {
    internal class RedundantGradingPeriodSetResponse {
        [JsonProperty("grading_period_set")]
        public IEnumerable<GradingPeriodSetModel> GradingPeriodSet { get; set; }
    }
}
