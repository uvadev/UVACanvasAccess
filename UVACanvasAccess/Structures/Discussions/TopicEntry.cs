using System;
using System.Collections.Generic;
using System.Linq;
using StatePrinting;
using UVACanvasAccess.Model.Discussions;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Discussions {
    
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
    // ReSharper disable MemberCanBePrivate.Global
    public class TopicEntry : IPrettyPrint {
        private readonly Api _api;
        
        public ulong Id { get; }
        
        public ulong UserId { get; }

        public ulong? EditorId { get; }

        public string UserName { get; }

        public string Message { get; }

        public string ReadState { get; }

        public bool ForcedReadState { get; }

        public DateTime CreatedAt { get; }

        public DateTime? UpdatedAt { get; }
        
        public FileAttachmentModel Attachment { get; }
        
        public IEnumerable<TopicReply> RecentReplies { get; }
        
        public bool? HasMoreReplies { get; }

        public TopicEntry(Api api, TopicEntryModel model) {
            _api = api;
            Id = model.Id;
            UserId = model.UserId;
            EditorId = model.EditorId;
            UserName = model.UserName;
            Message = model.Message;
            ReadState = model.ReadState;
            ForcedReadState = model.ForcedReadState;
            CreatedAt = model.CreatedAt;
            UpdatedAt = model.UpdatedAt;
            Attachment = model.Attachment;
            RecentReplies = from reply in model.RecentReplies
                            select new TopicReply(api, reply);
            HasMoreReplies = model.HasMoreReplies;
        }

        private static readonly Stateprinter Printer = new Stateprinter();
        public override string ToString() {
            return Printer.PrintObject(this);
        }

        public string ToPrettyString() {
            return "TopicEntry {" + 
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(UserId)}: {UserId}," +
                   $"\n{nameof(EditorId)}: {EditorId}," +
                   $"\n{nameof(UserName)}: {UserName}," +
                   $"\n{nameof(Message)}: {Message}," +
                   $"\n{nameof(ReadState)}: {ReadState}," +
                   $"\n{nameof(ForcedReadState)}: {ForcedReadState}," +
                   $"\n{nameof(CreatedAt)}: {CreatedAt}," +
                   $"\n{nameof(UpdatedAt)}: {UpdatedAt}," +
                   $"\n{nameof(Attachment)}: {Attachment}," +
                   $"\n{nameof(RecentReplies)}: {RecentReplies}," +
                   $"\n{nameof(HasMoreReplies)}: {HasMoreReplies}").Indent(4) +
                   "\n}";
        }
    }
}