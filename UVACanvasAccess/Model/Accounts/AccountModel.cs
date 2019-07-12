using JetBrains.Annotations;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Accounts {
    
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AccountModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("uuid")]
        public string Uuid { get; set; }
        
        [JsonProperty("parent_account_id")]
        public ulong? ParentAccountId { get; set; }
        
        [JsonProperty("root_account_id")]
        public ulong? RootAccountId { get; set; }
        
        [JsonProperty("default_user_storage_quota_mb")]
        public ulong? DefaultUserStorageQuotaMb { get; set; }
        
        [JsonProperty("default_group_storage_quota_mb")]
        public ulong? DefaultGroupStorageQuotaMb { get; set; }
        
        [JsonProperty("default_time_zone")]
        public string DefaultTimeZone { get; set; }
        
        [JsonProperty("sis_account_id")]
        [CanBeNull]
        public string SisAccountId { get; set; }
        
        [JsonProperty("integrationI-d")]
        [CanBeNull]
        public string IntegrationId { get; set; }
        
        [JsonProperty("sis_import_id")]
        [CanBeNull]
        public string SisImportId { get; set; }
        
        [JsonProperty("lti_guid")]
        public string LtiGuid { get; set; }
        
        [JsonProperty("workflow_state")]
        public string WorkflowState { get; set; }
    }
}