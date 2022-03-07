using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        
        /// <summary>
        /// The report id.
        /// </summary>
        public ulong Id { get; }
        
        /// <summary>
        /// The type, or title, of the report.
        /// </summary>
        public string ReportType { get; }

        /// <summary>
        /// If the report is complete; the url from which the report can be downloaded.
        /// </summary>
        [CanBeNull]
        public string FileUrl { get; }

        /// <summary>
        /// If the report is complete, the <see cref="FileAttachment"/> instance representing the complete report file.
        /// </summary>
        [CanBeNull]
        public FileAttachment Attachment { get; }
        
        /// <summary>
        /// The report status.
        /// </summary>
        public string Status { get; }

        /// <summary>
        /// When the report was created.
        /// </summary>
        public DateTime? CreatedAt { get; }

        /// <summary>
        /// When the report was started.
        /// </summary>
        public DateTime? StartedAt { get; }
        
        /// <summary>
        /// When the report was completed.
        /// </summary>
        public DateTime? EndedAt { get; }
        
        /// <summary>
        /// The optional set of parameters provided to this report.
        /// </summary>
        public Dictionary<string, JToken> Parameters { get; }
        
        /// <summary>
        /// The report progress, out of 100.
        /// </summary>
        public double? Progress { get; }

        /// <summary>
        /// The current line size of the report.
        /// </summary>
        public ulong? CurrentLine { get; }

        internal Report(Api api, ReportModel model) {
            _api = api;
            Id = model.Id;
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

        /// <summary>
        /// Fetches the status of this report using <see cref="Api.GetReportStatus"/>.
        /// </summary>
        /// <returns>A task containing the updated Report instance.</returns>
        public Task<Report> Refresh() {
            return _api.GetReportStatus(ReportType, Id);
        }

        public string ToPrettyString() {
            return "Report {" +
                   ($"\n{nameof(Id)}: {Id}," +
                    $"\n{nameof(ReportType)}: {ReportType}," +
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