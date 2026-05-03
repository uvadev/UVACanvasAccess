using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Users.Beta {
    internal class AsyncQueryResponseModel {

        [JsonProperty("poll_url")]
        public string PollUrl { get; set; }
    }
}
