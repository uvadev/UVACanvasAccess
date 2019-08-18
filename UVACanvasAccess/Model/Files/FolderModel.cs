using System;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Files {
    internal class FolderModel {
        
        [JsonProperty("context_type")]
        public string ContextType { get; set; }
        
        [JsonProperty("context_id")]
        public ulong ContextId { get; set; }
        
        [JsonProperty("files_count")]
        public uint FilesCount { get; set; }
        
        [JsonProperty("position")]
        public int? Position { get; set; }
        
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
        
        [JsonProperty("folders_url")]
        public string FoldersUrl { get; set; }
        
        [JsonProperty("files_url")]
        public string FilesUrl { get; set; }
        
        [JsonProperty("full_name")]
        public string FullName { get; set; }
        
        [JsonProperty("lock_at")]
        public DateTime? LockAt { get; set; }
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("folders_count")]
        public uint FoldersCount { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("parents_folder_id")]
        public ulong? ParentFolderId { get; set; }
        
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        
        [JsonProperty("unlock_at")]
        public DateTime? UnlockAt { get; set; }
        
        [JsonProperty("hidden")]
        public bool? Hidden { get; set; }
        
        [JsonProperty("hidden_for_user")]
        public bool? HiddenForUser { get; set; }
        
        [JsonProperty("locker")]
        public bool? Locked { get; set; }
        
        [JsonProperty("locked_for_user")]
        public bool? LockedForUser { get; set; }
        
        [JsonProperty("for_submissions")]
        public bool? ForSubmissions { get; set; }
    }
}
