using System;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Quizzes;
using UVACanvasAccess.Structures.Discussions;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Quizzes {
    
    /// <summary>
    /// Represents a quiz report.
    /// </summary>
    [PublicAPI]
    public class QuizReport : IPrettyPrint {
        private readonly Api api;
        
        public ulong Id { get; }
        
        public ulong QuizId { get; }
        
        public QuizReportType? ReportType { get; }
        
        public string ReadableType { get; }
        
        public bool? IncludesAllVersions { get; }
        
        public bool? Anonymous { get; }
        
        public bool? Generatable { get; }
        
        public DateTime? CreatedAt { get; }
        
        public DateTime? UpdatedAt { get; }
        
        public string Url { get; }
        
        [CanBeNull]
        public FileAttachment File { get; }
        
        public string ProgressUrl { get; }
        
        [CanBeNull]
        public JToken Progress { get; }

        internal QuizReport(Api api, QuizReportModel model) {
            this.api = api;
            Id = model.Id;
            QuizId = model.QuizId;
            ReportType = model.ReportType?.ToApiRepresentedEnum<QuizReportType>();
            ReadableType = model.ReadableType;
            IncludesAllVersions = model.IncludesAllVersions;
            Anonymous = model.Anonymous;
            Generatable = model.Generatable;
            CreatedAt = model.CreatedAt;
            UpdatedAt = model.UpdatedAt;
            Url = model.Url;
            File = model.File.ConvertIfNotNull(m => new FileAttachment(api, m));
            ProgressUrl = model.ProgressUrl;
            Progress = model.Progress;
        }

        public string ToPrettyString() {
            return "QuizReport {" +
                   ($"\n{nameof(Id)}: {Id}," +
                    $"\n{nameof(QuizId)}: {QuizId}," +
                    $"\n{nameof(ReportType)}: {ReportType}," +
                    $"\n{nameof(ReadableType)}: {ReadableType}," +
                    $"\n{nameof(IncludesAllVersions)}: {IncludesAllVersions}," +
                    $"\n{nameof(Anonymous)}: {Anonymous}," +
                    $"\n{nameof(Generatable)}: {Generatable}," +
                    $"\n{nameof(CreatedAt)}: {CreatedAt}," +
                    $"\n{nameof(UpdatedAt)}: {UpdatedAt}," +
                    $"\n{nameof(Url)}: {Url}," +
                    $"\n{nameof(File)}: {File?.ToPrettyString()}," +
                    $"\n{nameof(ProgressUrl)}: {ProgressUrl}," +
                    $"\n{nameof(Progress)}: {Progress}").Indent(4) +
                   "\n}";
        }
    }

    /// <summary>
    /// The types of quiz report.
    /// </summary>
    [PublicAPI]
    public enum QuizReportType : byte {
        [ApiRepresentation("student_analysis")]
        StudentAnalysis,
        [ApiRepresentation("item_analysis")]
        ItemAnalysis
    }

    /// <summary>
    /// Optional include fields for quiz report endpoints.
    /// </summary>
    [PublicAPI]
    [Flags]
    public enum QuizReportInclude {
        [ApiRepresentation("file")]
        File = 1 << 0,
        [ApiRepresentation("progress")]
        Progress = 1 << 1
    }
}
