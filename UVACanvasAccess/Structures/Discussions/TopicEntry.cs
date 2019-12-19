using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Discussions;
using UVACanvasAccess.Util;
using static UVACanvasAccess.Structures.Discussions.DiscussionTopic.DiscussionHome;

namespace UVACanvasAccess.Structures.Discussions {
    
    [PublicAPI]
    public class TopicEntry : IPrettyPrint {
        private readonly Api _api;
        
        public DiscussionTopic.DiscussionHome Home { get; }
        
        public ulong HomeId { get; }
        
        public ulong ParentId { get; }
        
        public ulong Id { get; }
        
        public ulong UserId { get; }

        public ulong? EditorId { get; }

        public string UserName { get; }

        public string Message { get; }

        public string ReadState { get; }

        public bool ForcedReadState { get; }

        public DateTime CreatedAt { get; }

        public DateTime? UpdatedAt { get; }
        
        [CanBeNull]
        public FileAttachment Attachment { get; }
        
        [NotNull]
        public IEnumerable<TopicReply> RecentReplies { get; }
        
        public bool HasMoreReplies { get; }

        public Task<IEnumerable<TopicReply>> GetReplies() {
            return _api.ListDiscussionEntryReplies(HomeId,
                                                   ParentId,
                                                   Id,
                                                   Home == Course ? "courses"
                                                                  : "groups");
        }

        internal TopicEntry(Api api, TopicEntryModel model, DiscussionTopic.DiscussionHome home, ulong homeId, ulong parentId) {
            _api = api;
            Home = home;
            HomeId = homeId;
            ParentId = parentId;
            Id = model.Id;
            UserId = model.UserId;
            EditorId = model.EditorId;
            UserName = model.UserName;
            Message = model.Message;
            ReadState = model.ReadState;
            ForcedReadState = model.ForcedReadState;
            CreatedAt = model.CreatedAt;
            UpdatedAt = model.UpdatedAt;
            Attachment = model.Attachment == null ? null
                                                  : new FileAttachment(api, model.Attachment);
            HasMoreReplies = model.HasMoreReplies ?? false;
            
            if (model.RecentReplies != null) {
                RecentReplies = from reply in model.RecentReplies
                                select new TopicReply(api, reply);
            } else {
                RecentReplies = Enumerable.Empty<TopicReply>();
            }
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
                   $"\n{nameof(Attachment)}: {Attachment?.ToPrettyString()}," +
                   $"\n{nameof(RecentReplies)}: {RecentReplies.ToPrettyString()}," +
                   $"\n{nameof(HasMoreReplies)}: {HasMoreReplies}").Indent(4) +
                   "\n}";
        }
    }
}