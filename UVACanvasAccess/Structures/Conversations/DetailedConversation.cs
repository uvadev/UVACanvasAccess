using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Conversations;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Conversations {
    
    /// <summary>
    /// Fully represents a conversation thread between 2 or more users.
    /// </summary>
    [PublicAPI]
    public class DetailedConversation : Conversation {
        
        /// <summary>
        /// List of messages in the conversation.
        /// </summary>
        public IEnumerable<ConversationMessage> Messages { get; }
        
        internal DetailedConversation(Api api, DetailedConversationModel model) : base(api, model) {
            Messages = model.Messages.SelectNotNull(m => new ConversationMessage(api, m));
        }
        
        /// <inheritdoc />
        public override string ToPrettyString() {
            return "DetailedConversation {" + 
                   ($"\n{nameof(Id)}: {Id}," +
                    $"\n{nameof(Subject)}: {Subject}," +
                    $"\n{nameof(ReadState)}: {ReadState}," +
                    $"\n{nameof(LastMessage)}: {LastMessage}," +
                    $"\n{nameof(LastMessageAt)}: {LastMessageAt}," +
                    $"\n{nameof(MessageCount)}: {MessageCount}," +
                    $"\n{nameof(Subscribed)}: {Subscribed}," +
                    $"\n{nameof(Private)}: {Private}," +
                    $"\n{nameof(Starred)}: {Starred}," +
                    $"\n{nameof(Properties)}: {Properties}," +
                    $"\n{nameof(Audience)}: {Audience.ToPrettyString()}," +
                    $"\n{nameof(AudienceContexts)}: {AudienceContexts.ToPrettyString()}," +
                    $"\n{nameof(AvatarUrl)}: {AvatarUrl}," +
                    $"\n{nameof(Participants)}: {Participants.ToPrettyString()}," +
                    $"\n{nameof(Visible)}: {Visible}," +
                    $"\n{nameof(ContextName)}: {ContextName}," +
                    $"\n{nameof(Messages)}: {Messages.ToPrettyString()}").Indent(4) + 
                   "\n}";
        }

        /// <summary>
        /// Returns this conversation.
        /// </summary>
        /// <returns>This.</returns>
        public sealed override Task<DetailedConversation> AsDetailedConversation() {
            return Task.FromResult(this);
        }
    }
}
