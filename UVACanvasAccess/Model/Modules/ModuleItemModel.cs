using JetBrains.Annotations;
using Newtonsoft.Json;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Model.Modules {
    
    internal class ModuleItemModel {
    
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("module_id")]
        public ulong ModuleId { get; set; }
        
        [JsonProperty("position")]
        public uint Position { get; set; }
        
        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("indent")]
        public uint? Indent { get; set; }
        
        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("content_id")]
        public ulong? ContentId { get; set; }
        
        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }
        
        [CanBeNull]
        [JsonProperty("url")]
        public string Url { get; set; }
        
        [CanBeNull]
        [JsonProperty("page_url")]
        public string PageUrl { get; set; }
        
        [CanBeNull]
        [JsonProperty("external_url")]
        public string ExternalUrl { get; set; }
        
        [JsonProperty("new_tab")]
        public bool NewTab { get; set; }
        
        [OptIn]
        [CanBeNull]
        [JsonProperty("completion_requirement")]
        public CompletionRequirementModel CompletionRequirement { get; set; }
        
        [JsonProperty("published")]
        public bool? Published { get; set; }
    }
}
