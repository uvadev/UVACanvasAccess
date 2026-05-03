using System.Collections.Generic;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Users.Beta {
    internal class AsyncApiErrorResponseModel {

        [JsonProperty("errors")]
        public List<string> Errors { get; set; }
    }
}
