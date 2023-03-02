using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Logins {
    internal class UserLoginModel {
        
        [JsonProperty("account_id")]
        public ulong AccountId { get; set; }
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("sis_user_id")]
        public string SisUserId { get; set; }
        
        [JsonProperty("integration_id")]
        public string IntegrationId { get; set; }
        
        [JsonProperty("unique_id")]
        public string UniqueId { get; set; }
        
        [JsonProperty("user_id")]
        public ulong UserId { get; set; }
        
        [JsonProperty("authentication_provider_id")]
        public ulong? AuthenticationProviderId { get; set; }
        
        [JsonProperty("authentication_provider_type")]
        public string AuthenticationProviderType { get; set; }
        
        [JsonProperty("workflow_state")]
        public string WorkflowState { get; set; }
        
        [JsonProperty("declared_user_type")]
        public string DeclaredUserType { get; set; }
    }
}
