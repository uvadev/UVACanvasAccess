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
        public TardinessModel TardinessBreakdown { get; set; }
    }
}
