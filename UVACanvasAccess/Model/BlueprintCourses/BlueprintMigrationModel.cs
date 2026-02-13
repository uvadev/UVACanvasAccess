using System;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.BlueprintCourses {
    
    internal class BlueprintMigrationModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("template_id")]
        public ulong? TemplateId { get; set; }
        
        [JsonProperty("subscription_id")]
        public ulong? SubscriptionId { get; set; }
        
        [JsonProperty("user_id")]
        public ulong UserId { get; set; }
        
        [JsonProperty("workflow_state")]
        public string WorkflowState { get; set; }
        
        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }
        
        [JsonProperty("exports_started_at")]
        public DateTime? ExportsStartedAt { get; set; }
        
        [JsonProperty("imports_queued_at")]
        public DateTime? ImportsQueuedAt { get; set; }
        
        [JsonProperty("imports_completed_at")]
        public DateTime? ImportsCompletedAt { get; set; }
        
        [JsonProperty("comment")]
        [CanBeNull]
        public string Comment { get; set; }
    }
}
