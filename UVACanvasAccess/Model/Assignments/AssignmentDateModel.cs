using System;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Assignments {
    
    internal class AssignmentDateModel {
        
        [JsonProperty("id")]
        public ulong? Id { get; set; }
        
        [JsonProperty("base")]
        public bool? Base { get; set; }
        
        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("due_at")]
        public DateTime DueAt { get; set; }
        
        [JsonProperty("unlock_at")]
        public DateTime? UnlockAt { get; set; }
        
        [JsonProperty("lock_at")]
        public DateTime? LockAt { get; set; }
    }
}