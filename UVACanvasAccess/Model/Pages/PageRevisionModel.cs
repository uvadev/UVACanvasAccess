using System;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UVACanvasAccess.Model.Users;

namespace UVACanvasAccess.Model.Pages {
    internal class PageRevisionModel {
        
        [JsonProperty("revision_id")]
        public ulong RevisionId { get; set; }
        
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
        
        [JsonProperty("latest")]
        public bool Latest { get; set; }
        
        [JsonProperty("edited_by")]
        [CanBeNull]
        public UserDisplayModel EditedBy { get; set; }
        
        [JsonProperty("url")]
        [CanBeNull]
        public string Url { get; set; }
        
        [JsonProperty("title")]
        [CanBeNull]
        public string Title { get; set; }
        
        [JsonProperty("body")]
        [CanBeNull]
        public string Body { get; set; }
    }
}