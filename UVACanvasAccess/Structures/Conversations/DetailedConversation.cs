using System.Collections.Generic;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Conversations;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Conversations {
    
    [PublicAPI]
    public class DetailedConversation : Conversation, IPrettyPrint {
        
        public IEnumerable<ConversationMessage> Messages { get; }
        
        internal DetailedConversation(Api api, DetailedConversationModel model) : base(api, model) {
            Messages = model.Messages.SelectNotNull(m => new ConversationMessage(api, m));
        }
        
        public new string ToPrettyString() {
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
                    $"\n{nameof(Properties)}: {Properties.ToPrettyString()}," +
                    $"\n{nameof(Audience)}: {Audience.ToPrettyString()}," +
                    $"\n{nameof(AudienceContexts)}: {AudienceContexts.ToPrettyString()}," +
                    $"\n{nameof(AvatarUrl)}: {AvatarUrl}," +
                    $"\n{nameof(Participants)}: {Participants.ToPrettyString()}," +
                    $"\n{nameof(Visible)}: {Visible}," +
                    $"\n{nameof(ContextName)}: {ContextName}," +
                    $"\n{nameof(Messages)}: {Messages.ToPrettyString()}").Indent(4) + 
                   "\n}";
        }
    }
}
