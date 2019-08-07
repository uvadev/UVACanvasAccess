using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Groups {
    
    internal class GroupMembershipModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("group_id")]
        public ulong GroupId { get; set; }
        
        [JsonProperty("user_id")]
        public ulong UserId { get; set; }
        
        [JsonProperty("workflow_state")]
        public string WorkflowState { get; set; }
        
        [JsonProperty("moderator")]
        public bool Moderator { get; set; }
        
        [JsonProperty("just_created")]
        public bool? JustCreated { get; set; }
        
        [JsonProperty("sis_import_id")]
        public ulong? SisImportId { get; set; }
    }
}
