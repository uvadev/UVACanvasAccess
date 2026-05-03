using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Users.Beta {
    internal class AsyncQueryStatusResponseModel {

        [JsonProperty("query_id")]
        public string QueryId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("results_url")]
        public string ResultsUrl { get; set; }

        [JsonProperty("error_code")]
        public string ErrorCode { get; set; }
        
        [JsonProperty("warnings")]
        [CanBeNull]
        public IEnumerable<AsyncQueryWarningModel> Warnings { get; set; }
    }
}
