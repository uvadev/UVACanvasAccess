using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Conversations;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Conversations {
    
    [PublicAPI]
    public class ConversationParticipant : IPrettyPrint {
        private readonly Api _api;
        
        public ulong Id { get; }
        
        public string Name { get; }
        
        public string FullName { get; }
        
        [CanBeNull]
        public string AvatarUrl { get; }

        internal ConversationParticipant(Api api, ConversationParticipantModel model) {
            _api = api;
            Id = model.Id;
            Name = model.Name;
            FullName = model.FullName;
            AvatarUrl = model.AvatarUrl;
        }

        public string ToPrettyString() {
            return "ConversationParticipant {" + 
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(Name)}: {Name}," +
                   $"\n{nameof(FullName)}: {FullName}," +
                   $"\n{nameof(AvatarUrl)}: {AvatarUrl}").Indent(4) + 
                   "\n}";
        }
    }
}
