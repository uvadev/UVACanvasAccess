using System.Collections.Generic;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Accounts {
    
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class HelpLinkModel {
        
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("text")]
        public string Text { get; set; }
        
        [JsonProperty("subtext")]
        public string Subtext { get; set; }
        
        [JsonProperty("url")]
        public string Url { get; set; }
        
        [JsonProperty("available_to")]
        public IEnumerable<string> AvailableTo { get; set; }
    }
}