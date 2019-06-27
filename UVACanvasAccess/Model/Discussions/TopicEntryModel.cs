using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using StatePrinting;

namespace UVACanvasAccess.Model.Discussions {
    
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class TopicEntryModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("user_id")]
        public ulong UserId { get; set; }
        
        [JsonProperty("editor_id")]
        public ulong? EditorId { get; set; }
        
        [JsonProperty("user_name")]
        public string UserName { get; set; }
        
        [JsonProperty("message")]
        public string Message { get; set; }
        
        [JsonProperty("read_state")]
        public string ReadState { get; set; }
        
        [JsonProperty("forced_read_state")]
        public bool ForcedReadState { get; set; }
        
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        
        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        
        public FileAttachmentModel Attachment { get; set; }
        
        public IEnumerable<TopicReplyModel> RecentReplies { get; set; }
        
        public bool? HasMoreReplies { get; set; }
        
        private static readonly Stateprinter Printer = new Stateprinter();
        public override string ToString() {
            return Printer.PrintObject(this);
        }
    }
}