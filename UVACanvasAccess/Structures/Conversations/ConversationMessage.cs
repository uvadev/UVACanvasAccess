using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Conversations;
using UVACanvasAccess.Structures.Discussions;
using UVACanvasAccess.Structures.Submissions;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Conversations {
    
    [PublicAPI]
    public class ConversationMessage : IPrettyPrint {
        private readonly Api _api;
        
        public ulong Id { get; }

        public DateTime CreatedAt { get; }
        
        public string Body { get; }
        
        public ulong AuthorId { get; }
        
        public bool Generated { get; }
        
        [CanBeNull]
        public MediaComment MediaComment { get; }
        
        public IEnumerable<ConversationMessage> ForwardedMessages { get; }

        public IEnumerable<FileAttachment> Attachments { get; }

        internal ConversationMessage(Api api, ConversationMessageModel model) {
            _api = api;
            Id = model.Id;
            CreatedAt = model.CreatedAt;
            Body = model.Body;
            AuthorId = model.AuthorId;
            Generated = model.Generated;
            MediaComment = model.MediaComment.ConvertIfNotNull(mc => new MediaComment(api, mc));
            ForwardedMessages = model.ForwardedMessages.SelectNotNull(fm => new ConversationMessage(api, fm));
            Attachments = model.Attachments.SelectNotNull(a => new FileAttachment(api, a));
        }

        public string ToPrettyString() {
            return "ConversationMessage {" +
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(CreatedAt)}: {CreatedAt}," +
                   $"\n{nameof(Body)}: {Body}," +
                   $"\n{nameof(AuthorId)}: {AuthorId}," +
                   $"\n{nameof(Generated)}: {Generated}," +
                   $"\n{nameof(MediaComment)}: {MediaComment?.ToPrettyString()}," +
                   $"\n{nameof(ForwardedMessages)}: {ForwardedMessages.ToPrettyString()}," +
                   $"\n{nameof(Attachments)}: {Attachments.ToPrettyString()}").Indent(4) + 
                   "\n}";
        }
    }
}
