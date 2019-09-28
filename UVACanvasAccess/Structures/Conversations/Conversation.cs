using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Conversations;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Conversations {
    
    [PublicAPI]
    public class Conversation : IPrettyPrint {
        private protected readonly Api Api;
        
        public ulong Id { get; set; }
        
        public string Subject { get; set; }
        
        public ConversationReadState ReadState { get; set; }
        
        public string LastMessage { get; set; }
        
        public DateTime StartAt { get; set; }
        
        public uint MessageCount { get; set; }
        
        public bool? Subscribed { get; set; }
        
        public bool? Private { get; set; }
        
        public bool? Starred { get; set; }
        
        [NotNull]
        public IEnumerable<string> Properties { get; set; }
        
        [NotNull]
        public IEnumerable<ulong> Audience { get; set; }
        
        [CanBeNull]
        public Dictionary<string, Dictionary<string, IEnumerable<string>>> AudienceContexts { get; set; }
        
        public string AvatarUrl { get; set; }
        
        [NotNull]
        public IEnumerable<ConversationParticipant> Participants { get; set; }
        
        public bool? Visible { get; set; }
        
        public string ContextName { get; set; }

        internal Conversation(Api api, ConversationModel model) {
            Api = api;
            Id = model.Id;
            Subject = model.Subject;
            ReadState = model.WorkflowState.ConvertIfNotNullValue(wf => wf.ToApiRepresentedEnum<ConversationReadState>().Value).Value;
            LastMessage = model.LastMessage;
            StartAt = model.StartAt;
            MessageCount = model.MessageCount;
            Subscribed = model.Subscribed;
            Private = model.Private;
            Starred = model.Starred;
            Properties = model.Properties ?? new string[0];
            Audience = model.Audience ?? new ulong[0];
            AudienceContexts = model.AudienceContexts;
            AvatarUrl = model.AvatarUrl;
            Participants = model.Participants.SelectNotNull(cp => new ConversationParticipant(api, cp));
            Visible = model.Visible;
            ContextName = model.ContextName;
        }

        public string ToPrettyString() {
            return "Conversation {" + 
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(Subject)}: {Subject}," +
                   $"\n{nameof(ReadState)}: {ReadState}," +
                   $"\n{nameof(LastMessage)}: {LastMessage}," +
                   $"\n{nameof(StartAt)}: {StartAt}," +
                   $"\n{nameof(MessageCount)}: {MessageCount}," +
                   $"\n{nameof(Subscribed)}: {Subscribed}," +
                   $"\n{nameof(Private)}: {Private}," +
                   $"\n{nameof(Starred)}: {Starred}," +
                   $"\n{nameof(Properties)}: {Properties.ToPrettyString()}," +
                   $"\n{nameof(Audience)}: {Audience.ToPrettyString()}," +
                   $"\n{nameof(AudienceContexts)}: {AudienceContexts.ToPrettyString()}," +
                   $"\n{nameof(AvatarUrl)}: {AvatarUrl}," +
                   $"\n{nameof(Participants)}: {Participants.ToPrettyString()}," +
                   $"\n{nameof(Visible)}: {Visible}," +
                   $"\n{nameof(ContextName)}: {ContextName}").Indent(4) + 
                   "\n}";
        }
    }

    [PublicAPI]
    public enum ConversationReadState : byte {
        [ApiRepresentation("read")]
        Read,
        [ApiRepresentation("unread")]
        Unread,
        [ApiRepresentation("archived")]
        Archived
    }
}
