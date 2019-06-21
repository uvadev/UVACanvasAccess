using Newtonsoft.Json;
using StatePrinting;

namespace UVACanvasAccess.Model.Files {
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CanvasFileModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("uuid")]
        public string Uuid { get; set; }
        
        [JsonProperty("folder_id")]
        public ulong FolderId { get; set; }
        
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
        
        [JsonProperty("filename")]
        public string Filename { get; set; }
        
        [JsonProperty("content_type")]
        public string ContentType { get; set; }
        
        [JsonProperty("url")]
        public string Url { get; set; }
        
        [JsonProperty("size")]
        public ulong Size { get; set; }
        
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
        
        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }
        
        [JsonProperty("unlock_at")]
        public string UnlockAt { get; set; }
        
        [JsonProperty("locked")]
        public bool Locked { get; set; }
        
        [JsonProperty("hidden")]
        public bool Hidden { get; set; }
        
        [JsonProperty("lock_at")]
        public string LockAt { get; set; }
        
        [JsonProperty("hidden_for_user")]
        public bool HiddenForUser { get; set; }
        
        [JsonProperty("thumbnail_url")]
        public string ThumbnailUrl { get; set; }
        
        [JsonProperty("modified_at")]
        public string ModifiedAt { get; set; }
        
        [JsonProperty("mime_class")]
        public string MimeClass { get; set; }
        
        [JsonProperty("media_entry_id")]
        public string MediaEntryId { get; set; }
        
        [JsonProperty("locked_for_user")]
        public bool LockedForUser { get; set; }
        
        [JsonProperty("lock_info")]
        public object LockInfo { get; set; }
        
        [JsonProperty("lock_explanation")]
        public string LockExplanation { get; set; }
        
        [JsonProperty("preview_url")]
        public string PreviewUrl { get; set; }

        private static readonly Stateprinter Printer = new Stateprinter();
        public override string ToString() {
            return Printer.PrintObject(this);
        }
    }
}