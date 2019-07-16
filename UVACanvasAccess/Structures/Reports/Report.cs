using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Reports;
using UVACanvasAccess.Structures.Discussions;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Reports {
    
    [PublicAPI]
    public class Report : IPrettyPrint {
        private readonly Api _api;
        
        public string ReportType { get; }

        [CanBeNull]
        public string FileUrl { get; }

        [CanBeNull]
        public FileAttachment Attachment { get; }
        
        public string Status { get; }

        public DateTime? CreatedAt { get; }

        public DateTime? StartedAt { get; }
        
        public DateTime? EndedAt { get; }
        
        public Dictionary<string, JToken> Parameters { get; }
        
        public double? Progress { get; }

        public ulong? CurrentLine { get; }

        internal Report(Api api, ReportModel model) {
            _api = api;
            ReportType = model.Report;
            FileUrl = model.FileUrl;
            Attachment = model.Attachment.ConvertIfNotNull(m => new FileAttachment(api, m));
            Status = model.Status;
            CreatedAt = model.CreatedAt;
            StartedAt = model.StartedAt;
            EndedAt = model.EndedAt;
            Parameters = model.Parameters;
            Progress = model.Progress;
            CurrentLine = model.CurrentLine;
        }

        public string ToPrettyString() {
            return "Report {" +
                   ($"\n{nameof(ReportType)}: {ReportType}," +
                   $"\n{nameof(FileUrl)}: {FileUrl}," +
                   $"\n{nameof(Attachment)}: {Attachment?.ToPrettyString()}," +
                   $"\n{nameof(Status)}: {Status}," +
                   $"\n{nameof(CreatedAt)}: {CreatedAt}," +
                   $"\n{nameof(StartedAt)}: {StartedAt}," +
                   $"\n{nameof(EndedAt)}: {EndedAt}," +
                   $"\n{nameof(Parameters)}: {Parameters.ToPrettyString()}," +
                   $"\n{nameof(Progress)}: {Progress}," +
                   $"\n{nameof(CurrentLine)}: {CurrentLine}").Indent(4) + 
                   "\n}";
        }
    }
}