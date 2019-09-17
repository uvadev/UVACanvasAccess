using JetBrains.Annotations;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Conversations {
    
    internal class ConversationParticipantModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("full_name")]
        public string FullName { get; set; }
        
        [CanBeNull]
        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }
    }
}
