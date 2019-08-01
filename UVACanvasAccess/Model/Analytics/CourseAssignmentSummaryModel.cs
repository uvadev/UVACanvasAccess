using System;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Analytics {
    
    internal class CourseAssignmentSummaryModel {
        
        [JsonProperty("assignment_id")]
        public ulong AssignmentId { get; set; }
        
        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("due_at")]
        public DateTime DueAt { get; set; }
        
        [JsonProperty("unlock_at")]
        public DateTime? UnlockAt { get; set; }
        
        [JsonProperty("muted")]
        public bool Muted { get; set; }
        
        [JsonProperty("points_possible")]
        public decimal PointsPossible { get; set; }
        
        [JsonProperty("non_digital_submission")]
        public bool? NonDigitalSubmission { get; set; }
        
        [JsonProperty("max_score")]
        public decimal? MaxScore { get; set; }
        
        [JsonProperty("min_score")]
        public decimal? MinScore { get; set; }
        
        [JsonProperty("first_quartile")]
        public decimal? FirstQuartile { get; set; }
        
        [JsonProperty("median")]
        public decimal? Median { get; set; }
        
        [JsonProperty("third_quartile")]
        public decimal? ThirdQuartile { get; set; }
        
        [JsonProperty("tardiness_breakdown")]
        public TardinessModel TardinessBreakdown { get; set; }
    }
}
