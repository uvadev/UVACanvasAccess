using System.Collections.Generic;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Authentications {
    internal struct AuthenticationEventsResponseModel {
        
        [JsonProperty("events")]
        public IEnumerable<AuthenticationEventModel> Events { get; set; }
        
    }
}