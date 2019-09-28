using System.Collections.Generic;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Conversations {
    
    internal class DetailedConversationModel : ConversationModel {
        
        [JsonProperty("messages")]
        public IEnumerable<ConversationMessageModel> Messages { get; set; }
        
    }
}
