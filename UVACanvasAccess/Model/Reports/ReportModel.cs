using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UVACanvasAccess.Model.Discussions;

namespace UVACanvasAccess.Model.Reports {
    
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ReportModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("report")]
        public string Report { get; set; }
        
        [JsonProperty("file_url")]
        [CanBeNull]
        public string FileUrl { get; set; }
        
        [JsonProperty("attachment")]
        [CanBeNull]
        public FileAttachmentModel Attachment { get; set; }
        
        [JsonProperty("status")]
        public string Status { get; set; }
        
        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }
        
        [JsonProperty("started_at")]
        public DateTime? StartedAt { get; set; }
        
        [JsonProperty("ended_at")]
        public DateTime? EndedAt { get; set; }
        
        // the fields in this object can vary wildly depending on the report type, so for now we will loosely type it
        // like this
        [JsonProperty("parameters")]
        public Dictionary<string, JToken> Parameters { get; set; }
        
        [JsonProperty("progress")]
        public double? Progress { get; set; }
        
        [JsonProperty("current_line")]
        public ulong? CurrentLine { get; set; }
    }
}