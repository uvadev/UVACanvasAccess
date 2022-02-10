using System.Collections.Generic;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.GradingPeriods {
    internal class RedundantGradingPeriodResponse {
        [JsonProperty("grading_periods")]
        public IEnumerable<GradingPeriodModel> GradingPeriods { get; set; }
    }
}
