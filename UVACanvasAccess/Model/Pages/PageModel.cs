using System;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UVACanvasAccess.Model.Assignments;

namespace UVACanvasAccess.Model.Pages {
    
    internal class PageModel {
        
        [JsonProperty("url")]
        public string Url { get; set; }
        
        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
        
        [JsonProperty("editing_roles")]
        public string EditingRoles { get; set; }
        
        [JsonProperty("last_edited_by")]
        public ulong? LastEditedBy { get; set; }
        
        [JsonProperty("body")]
        [CanBeNull]
        public string Body { get; set; }
        
        [JsonProperty("published")]
        public bool Published { get; set; }
        
        [JsonProperty("front_page")]
        public bool FrontPage { get; set; }
        
        [JsonProperty("locked_for_user")]
        public bool LockedForUser { get; set; }
        
        [JsonProperty("lock_info")]
        [CanBeNull]
        public LockInfoModel LockInfo { get; set; }
        
        [JsonProperty("lock_explanation")]
        [CanBeNull]
        public string LockExplanation { get; set; }
    }
}