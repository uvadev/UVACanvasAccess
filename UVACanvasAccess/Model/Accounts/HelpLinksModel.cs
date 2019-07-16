using System.Collections.Generic;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Accounts {
    
    internal class HelpLinksModel {
        
        [JsonProperty("help_link_name")]
        public string HelpLinkName { get; set; }
        
        [JsonProperty("help_link_icon")]
        public string HelpLinkIcon { get; set; }
        
        [JsonProperty("custom_help_links")]
        public IEnumerable<HelpLinkModel> CustomHelpLinks { get; set; }
        
        [JsonProperty("default_help_links")]
        public IEnumerable<HelpLinkModel> DefaultHelpLinks { get; set; }
    }
}