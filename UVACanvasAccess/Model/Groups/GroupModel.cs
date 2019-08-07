using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Groups {
    
    internal class GroupModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("description")]
        [CanBeNull]
        public string Description { get; set; }
        
        [JsonProperty("is_public")]
        public bool? IsPublic { get; set; }
        
        [JsonProperty("followed_by_user")]
        public bool FollowedByUser { get; set; }
        
        [JsonProperty("join_level")]
        public string JoinLevel { get; set; }
        
        [JsonProperty("members_count")]
        public uint MembersCount { get; set; }
        
        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }
        
        [JsonProperty("context_type")]
        public string ContextType { get; set; }
        
        [JsonProperty("course_id")]
        public ulong? CourseId { get; set; }
        
        [JsonProperty("account_id")]
        public ulong? AccountId { get; set; }
        
        [JsonProperty("role")]
        public string Role { get; set; }
        
        [JsonProperty("group_category_id")]
        public ulong GroupCategoryId { get; set; }
        
        [JsonProperty("sis_group_id")]
        [CanBeNull]
        public string SisGroupId { get; set; }
        
        [JsonProperty("sis_import_id")]
        public ulong? SisImportId { get; set; }
        
        [JsonProperty("storage_quota_mb")]
        public uint StorageQuoteMb { get; set; }
        
        [JsonProperty("permissions")]
        public Dictionary<string, bool> Permissions { get; set; }
    }
}
