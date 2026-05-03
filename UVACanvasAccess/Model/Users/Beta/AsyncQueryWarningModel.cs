using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Users.Beta {
    internal class AsyncQueryWarningModel {

        [JsonProperty("code")]
        public string Code { get; set; }
        
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
