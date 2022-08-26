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
        
        /// <summary>
        /// The conversation id.
        /// </summary>
        public ulong Id { get; }
        
        /// <summary>
        /// The conversation subject.
        /// </summary>
        public string Subject { get; }
        
        /// <summary>
        /// The conversation's read state from the perspective of the current user.
        /// </summary>
        public ConversationReadState ReadState { get; }
        
        /// <summary>
        /// The most recent message.
        /// </summary>
        public string LastMessage { get; }
        
        /// <summary>
        /// When the last message was sent.
        /// </summary>
        public DateTime? LastMessageAt { get; }
        
        /// <summary>
        /// The message count.
        /// </summary>
        public uint MessageCount { get; }
        
        /// <summary>
        /// Whether the current user is subscribed to the conversation.
        /// </summary>
        public bool? Subscribed { get; }
        
        /// <summary>
        /// Whether the conversation is private.
        /// </summary>
        public bool? Private { get; }
        
        /// <summary>
        /// Whether the conversation is starred.
        /// </summary>
        public bool? Starred { get; }
        
        /// <summary>
        /// Additional flags.
        /// </summary>
        /// <seealso cref="CurrentUserIsLastAuthor"/>
        /// <seealso cref="HasAttachments"/>
        /// <seealso cref="HasMediaObjects"/>
        public ConversationFlags Properties { get; }

        /// <summary>
        /// Whether the current user authored the most recent message in the conversation.
        /// </summary>
        public bool CurrentUserIsLastAuthor => Properties.HasFlag(ConversationFlags.LastAuthor);

        /// <summary>
        /// Whether the conversation has attachments.
        /// </summary>
        public bool HasAttachments => Properties.HasFlag(ConversationFlags.Attachments);

        /// <summary>
        /// Whether the conversation has media objects.
        /// </summary>
        public bool HasMediaObjects => Properties.HasFlag(ConversationFlags.MediaObjects);
        
        /// <summary>
        /// List of user ids involved in the conversation.
        /// </summary>
        [NotNull]
        public IEnumerable<ulong> Audience { get; }
        
        /// <summary>
        /// Shared contexts between the <see cref="Audience">audience members</see>.
        /// </summary>
        [CanBeNull]
        public Dictionary<string, Dictionary<string, IEnumerable<string>>> AudienceContexts { get; }
        
        /// <summary>
        /// The avatar url.
        /// </summary>
        public string AvatarUrl { get; }
        
        /// <summary>
        /// List of participants.
        /// </summary>
        [NotNull]
        public IEnumerable<ConversationParticipant> Participants { get; }
        
        /// <summary>
        /// Whether the conversation would be visible to the current user in the web UI.
        /// </summary>
        public bool? Visible { get; }
        
        /// <summary>
        /// The context name (e.g. which course or group the conversation is occuring in).
        /// </summary>
        public string ContextName { get; }

        internal Conversation(Api api, ConversationModel model) {
            Api = api;
            Id = model.Id;
            Subject = model.Subject;
            ReadState = model.WorkflowState.ConvertIfNotNullValue(wf => wf.ToApiRepresentedEnum<ConversationReadState>().Expect()).Value;
            LastMessage = model.LastMessage;
            LastMessageAt = model.LastMessageAt;
            MessageCount = model.MessageCount;
            Subscribed = model.Subscribed;
            Private = model.Private;
            Starred = model.Starred;
            Properties = (model.Properties ?? Array.Empty<string>()).ToApiRepresentedFlagsEnum<ConversationFlags>();
            Audience = model.Audience ?? Array.Empty<ulong>();
            AudienceContexts = model.AudienceContexts;
            AvatarUrl = model.AvatarUrl;
            Participants = model.Participants.SelectNotNull(cp => new ConversationParticipant(api, cp));
            Visible = model.Visible;
            ContextName = model.ContextName;
        }

        /// <inheritdoc />
        public virtual string ToPrettyString() {
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
                   $"\n{nameof(Properties)}: {Properties.ToString()}," +
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

    /// <summary>
    /// The conversation read state, from the perspective of the current user.
    /// </summary>
    [PublicAPI]
    public enum ConversationReadState : byte {
        /// <summary>
        /// The latest message is read.
        /// </summary>
        [ApiRepresentation("read")]
        Read,
        /// <summary>
        /// The latest message is unread.
        /// </summary>
        [ApiRepresentation("unread")]
        Unread,
        /// <summary>
        /// The conversation is archived.
        /// </summary>
        [ApiRepresentation("archived")]
        Archived
    }

    /// <summary>
    /// Additional property flags for <see cref="Conversation"/>.
    /// </summary>
    [PublicAPI]
    [Flags]
    public enum ConversationFlags : byte {
        /// <summary>
        /// The current user is the author of the most recently sent message.
        /// </summary>
        [ApiRepresentation("last_author")]
        LastAuthor,
        /// <summary>
        /// The conversation contains attachments.
        /// </summary>
        [ApiRepresentation("attachments")]
        Attachments,
        /// <summary>
        /// The conversation contains media objects.
        /// </summary>
        [ApiRepresentation("media_objects")]
        MediaObjects
    } 
}
