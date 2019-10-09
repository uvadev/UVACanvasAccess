using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Conversations;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Conversations {
    
    /// <summary>
    /// Summarizes a conversation thread between 2 or more users.
    /// </summary>
    /// <seealso cref="DetailedConversation"/>
    [PublicAPI]
    public class Conversation : IPrettyPrint {
        private protected readonly Api Api;
        
        public ulong Id { get; set; }
        
        public string Subject { get; set; }
        
        public ConversationReadState ReadState { get; set; }
        
        public string LastMessage { get; set; }
        
        public DateTime? LastMessageAt { get; set; }
        
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
            LastMessageAt = model.LastMessageAt;
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
                   $"\n{nameof(LastMessageAt)}: {LastMessageAt}," +
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

        /// <summary>
        /// Converts this conversation to a <see cref="DetailedConversation"/> with full message history.
        /// </summary>
        /// <returns>The <see cref="DetailedConversation"/> summarized by this conversation.</returns>
        /// <seealso cref="UVACanvasAccess.ApiParts.Api.GetConversation"/>
        public virtual Task<DetailedConversation> AsDetailedConversation() {
            return Api.GetConversation(Id);
        }

        /// <summary>
        /// Adds a message to this conversation.
        /// </summary>
        /// <param name="body">The message to send.</param>
        /// <param name="specificRecipients">(Optional) Specific recipients to send the message to.</param>
        /// <param name="includedMessages">(Optional) The set of past messages to send to recipients not already present in the conversation.</param>
        /// <param name="attachmentIds">(Optional) Attachment ids. Attachments must have been uploaded to the current user's attachments folder.</param>
        /// <param name="mediaCommentId">(Optional) Media comment id.</param>
        /// <param name="mediaCommentType">(Optional) Media comment type.</param>
        /// <param name="addJournalEntry">(Optional) If true, a faculty journal entry will be created to record this conversation.</param>
        /// <returns>The updated conversation.</returns>
        public Task<Conversation> AddMessage(string body,
                                             IEnumerable<QualifiedId> specificRecipients = null,
                                             IEnumerable<ulong> includedMessages = null,
                                             IEnumerable<string> attachmentIds = null,
                                             string mediaCommentId = null,
                                             string mediaCommentType = null,
                                             bool? addJournalEntry = null) {
            return Api.AddMessageToConversation(Id, body, specificRecipients, includedMessages, attachmentIds, mediaCommentId, mediaCommentType, addJournalEntry);
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
