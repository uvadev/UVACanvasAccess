using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UVACanvasAccess.Model.Discussions;
using UVACanvasAccess.Model.Submissions;

namespace UVACanvasAccess.Model.Conversations {
    
    internal class ConversationMessageModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        
        [JsonProperty("body")]
        public string Body { get; set; }
        
        [JsonProperty("author_id")]
        public ulong AuthorId { get; set; }
        
        [JsonProperty("generated")]
        public bool Generated { get; set; }
        
        [CanBeNull]
        [JsonProperty("media_comment")]
        public MediaCommentModel MediaComment { get; set; }
        
        [JsonProperty("forwarded_messages")]
        public IEnumerable<ConversationMessageModel> ForwardedMessages { get; set; }
        
        [JsonProperty("attachments")]
        public IEnumerable<FileAttachmentModel> Attachments { get; set; }
    }
}
