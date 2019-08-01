using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Analytics {
    
    internal class CourseStudentSummaryModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("page_views")]
        public uint PageViews { get; set; }
        
        [JsonProperty("max_page_views")]
        public uint? MaxPageViews { get; set; }
        
        [JsonProperty("page_views_level")]
        public uint? PageViewsLevel { get; set; }
        
        [JsonProperty("participations")]
        public uint Participations { get; set; }
        
        [JsonProperty("max_participations")]
        public uint? MaxParticipations { get; set; }
        
        [JsonProperty("participations_level")]
        public uint? ParticipationsLevel { get; set; }
        
        [JsonProperty("tardiness_breakdown")]
        public CourseStudentTardinessModel TardinessBreakdown { get; set; }
    }

    internal class CourseStudentTardinessModel {
        
        [JsonProperty("missing")]
        public uint Missing { get; set; }
        
        [JsonProperty("late")]
        public uint Late { get; set; }
        
        [JsonProperty("on_time")]
        public uint OnTime { get; set; }
        
        [JsonProperty("floating")]
        public uint Floating { get; set; }
        
        [JsonProperty("total")]
        public uint Total { get; set; }
    }
}
