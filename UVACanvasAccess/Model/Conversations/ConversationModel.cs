using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Conversations {
    
    internal class ConversationModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("subject")]
        public string Subject { get; set; }
        
        [JsonProperty("workflow_state")]
        public string WorkflowState { get; set; }
        
        [JsonProperty("last_message")]
        public string LastMessage { get; set; }
        
        [JsonProperty("last_message_at")]
        public DateTime LastMessageAt { get; set; }
        
        [JsonProperty("message_count")]
        public uint MessageCount { get; set; }
        
        [JsonProperty("subscribed")]
        public bool? Subscribed { get; set; }
        
        [JsonProperty("private")]
        public bool? Private { get; set; }
        
        [JsonProperty("starred")]
        public bool? Starred { get; set; }
        
        [CanBeNull]
        [JsonProperty("properties")]
        public IEnumerable<string> Properties { get; set; }
        
        [CanBeNull]
        [JsonProperty("audience")]
        public IEnumerable<ulong> Audience { get; set; }
        
        [CanBeNull]
        [JsonProperty("audience_contexts")]
        public Dictionary<string, Dictionary<string, IEnumerable<string>>> AudienceContexts { get; set; }
        
        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }
        
        [CanBeNull]
        [JsonProperty("participants")]
        public IEnumerable<ConversationParticipantModel> Participants { get; set; }
        
        [JsonProperty("visible")]
        public bool? Visible { get; set; }
        
        [JsonProperty("context_name")]
        public string ContextName { get; set; }
    }
}
