using System.Collections.Generic;
using Newtonsoft.Json;
using UVACanvasAccess.Model.Accounts;

namespace UVACanvasAccess.Model.Roles {
    
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class RoleModel {
        
        [JsonProperty("label")]
        public string Label { get; set; }
        
        [JsonProperty("base_role_type")]
        public string BaseRoleType { get; set; }
        
        [JsonProperty("account")]
        public AccountModel Account { get; set; }
        
        [JsonProperty("workflow_state")]
        public string WorkflowState  { get; set; }
        
        [JsonProperty("permissions")]
        public Dictionary<string, RolePermissionsModel> Permissions  { get; set; }
    }
}