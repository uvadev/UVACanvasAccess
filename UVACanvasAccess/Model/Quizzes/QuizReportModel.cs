using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UVACanvasAccess.Model.Discussions;

namespace UVACanvasAccess.Model.Quizzes {
    
    internal class QuizReportModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("quiz_id")]
        public ulong QuizId { get; set; }
        
        [JsonProperty("report_type")]
        public string ReportType { get; set; }
        
        [JsonProperty("readable_type")]
        public string ReadableType { get; set; }
        
        [JsonProperty("includes_all_versions")]
        public bool? IncludesAllVersions { get; set; }
        
        [JsonProperty("anonymous")]
        public bool? Anonymous { get; set; }
        
        [JsonProperty("generatable")]
        public bool? Generatable { get; set; }
        
        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }
        
        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        
        [JsonProperty("url")]
        public string Url { get; set; }
        
        [JsonProperty("file")]
        public FileAttachmentModel File { get; set; }
        
        [JsonProperty("progress_url")]
        public string ProgressUrl { get; set; }
        
        [JsonProperty("progress")]
        public JToken Progress { get; set; }
    }
}
