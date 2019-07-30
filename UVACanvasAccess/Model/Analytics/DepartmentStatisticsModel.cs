using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Analytics {
    
    internal class DepartmentStatisticsModel {
        
        [JsonProperty("courses")]
        public ulong Courses { get; set; }
        
        [JsonProperty("subaccounts")]
        public ulong Subaccounts { get; set; }
        
        [JsonProperty("teacher")]
        public ulong Teachers { get; set; }
        
        [JsonProperty("students")]
        public ulong Students { get; set; }
        
        [JsonProperty("discussion_topics")]
        public ulong DiscussionTopics { get; set; }
        
        [JsonProperty("media_objects")]
        public ulong MediaObjects { get; set; }
        
        [JsonProperty("attachments")]
        public ulong Attachments { get; set; }
        
        [JsonProperty("assignments")]
        public ulong Assignments { get; set; }
    }
}
