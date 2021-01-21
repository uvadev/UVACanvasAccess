using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.ExternalTools {
    
    internal class ExternalToolModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("domain")]
        public string Domain { get; set; }
        
        [JsonProperty("consumer_key")]
        public string ConsumerKey { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }
        
        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        
        [JsonProperty("privacy_level")]
        public string PrivacyLevel { get; set; } 
        
        [JsonProperty("custom_fields")]
        public Dictionary<string, string> CustomFields { get; set; }
        
        [JsonProperty("account_navigation")]
        public AccountNavigationModel AccountNavigation { get; set; }
        
        [JsonProperty("course_home_sub_navigation")]
        public CourseHomeSubNavigationModel CourseHomeSubNavigation { get; set; }
        
        [JsonProperty("course_navigation")]
        public CourseNavigationModel CourseNavigation { get; set; }
        
        [JsonProperty("editor_button")]
        public EditorButtonModel EditorButton { get; set; }
        
        [JsonProperty("homework_submission")]
        public HomeworkSubmissionModel HomeworkSubmission { get; set; }
        
        [JsonProperty("migration_selection")]
        public MigrationSelectionModel MigrationSelection { get; set; }
        
        [JsonProperty("resource_selection")]
        public ResourceSelectionModel ResourceSelection { get; set; }
        
        [JsonProperty("tool_configuration")]
        public ToolConfigurationModel ToolConfiguration { get; set; }
        
        [JsonProperty("user_navigation")]
        public UserNavigationModel UserNavigation { get; set; }
        
        [JsonProperty("selection_width")]
        public uint? SelectionWidth { get; set; }
        
        [JsonProperty("selection_height")]
        public uint? SelectionHeight { get; set; }
        
        [JsonProperty("icon_url")]
        public string IconUrl { get; set; }
        
        [JsonProperty("not_selectable")]
        public bool? NotSelectable { get; set; }
    }
}
