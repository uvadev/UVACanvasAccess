using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Analytics {
    
    internal struct UserAssignmentSubmissionDataModel {
        
        [JsonProperty("submitted_at")]
        public DateTime? SubmittedAt { get; set; }
        
        [JsonProperty("score")]
        public uint? Score { get; set; }
    }
    
    internal class UserAssignmentDataModel {
        
        [JsonProperty("assignment_id")]
        public ulong AssignmentId { get; set; }
        
        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("points_possible")]
        public uint? PointsPossible { get; set; }
        
        [JsonProperty("due_at")]
        public DateTime? DueAt { get; set; }
        
        [JsonProperty("unlock_at")]
        public DateTime? UnlockAt { get; set; }
        
        [JsonProperty("muted")]
        public bool? Muted { get; set; }
        
        [JsonProperty("min_score")]
        public uint? MinScore { get; set; }
        
        [JsonProperty("max_score")]
        public uint? MaxScore { get; set; }
        
        [JsonProperty("median")]
        public uint? Median { get; set; }
        
        [JsonProperty("first_quartile")]
        public uint? FirstQuartile { get; set; }
        
        [JsonProperty("third_quartile")]
        public uint? ThirdQuartile { get; set; }
        
        [JsonProperty("module_ids")]
        public IEnumerable<ulong> ModuleIds { get; set; }
        
        [JsonProperty("submission")]
        public UserAssignmentSubmissionDataModel? Submission { get; set; }
    }
}
