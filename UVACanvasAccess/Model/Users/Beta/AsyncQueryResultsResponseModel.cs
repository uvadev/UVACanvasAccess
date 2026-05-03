using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Users.Beta {
    internal class AsyncQueryResultsResponseModel {

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("filename")]
        public string Filename { get; set; }

        [JsonProperty("content_type")]
        public string ContentType { get; set; }

        [JsonProperty("content_encoding")]
        public string ContentEncoding { get; set; }
    }
}
