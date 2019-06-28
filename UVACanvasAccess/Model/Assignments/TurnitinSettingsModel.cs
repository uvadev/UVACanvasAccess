using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Assignments {
    
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class TurnitinSettingsModel {
        
        [JsonProperty("originality_report_visibility")]
        public string OriginalityReportVisibility { get; set; }
        
        [JsonProperty("s_paper_check")]
        public string SPaperCheck { get; set; }
        
        [JsonProperty("internet_check")]
        public string InternetCheck { get; set; }
        
        [JsonProperty("journal_check")]
        public string JournalCheck { get; set; }
        
        [JsonProperty("exclude_biblio")]
        public string ExcludeBiblio { get; set; }
        
        [JsonProperty("exclude_quoted")]
        public string ExcludeQuoted { get; set; }
        
        [JsonProperty("exclude_small_matches_type")]
        public string ExcludeSmallMatchesType { get; set; }
        
        [JsonProperty("exclude_small_matches_value")]
        public uint ExcludeSmallMatchesValue { get; set; }
    }
}