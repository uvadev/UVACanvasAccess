using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Quizzes;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Quizzes {
    
    /// <summary>
    /// Represents quiz statistics.
    /// </summary>
    [PublicAPI]
    public class QuizStatistics : IPrettyPrint {
        private readonly Api api;
        
        public ulong? Id { get; }
        
        public ulong? QuizId { get; }
        
        public bool? MultipleAttemptsExist { get; }
        
        public bool? IncludesAllVersions { get; }
        
        public DateTime? GeneratedAt { get; }
        
        public string Url { get; }
        
        public string HtmlUrl { get; }
        
        [CanBeNull]
        public JToken QuestionStatistics { get; }
        
        [CanBeNull]
        public JToken SubmissionStatistics { get; }
        
        [CanBeNull]
        public JToken Links { get; }

        internal QuizStatistics(Api api, QuizStatisticsModel model) {
            this.api = api;
            Id = model.Id;
            QuizId = model.QuizId;
            MultipleAttemptsExist = model.MultipleAttemptsExist;
            IncludesAllVersions = model.IncludesAllVersions;
            GeneratedAt = model.GeneratedAt;
            Url = model.Url;
            HtmlUrl = model.HtmlUrl;
            QuestionStatistics = model.QuestionStatistics;
            SubmissionStatistics = model.SubmissionStatistics;
            Links = model.Links;
        }

        public string ToPrettyString() {
            return "QuizStatistics {" +
                   ($"\n{nameof(Id)}: {Id}," +
                    $"\n{nameof(QuizId)}: {QuizId}," +
                    $"\n{nameof(MultipleAttemptsExist)}: {MultipleAttemptsExist}," +
                    $"\n{nameof(IncludesAllVersions)}: {IncludesAllVersions}," +
                    $"\n{nameof(GeneratedAt)}: {GeneratedAt}," +
                    $"\n{nameof(Url)}: {Url}," +
                    $"\n{nameof(HtmlUrl)}: {HtmlUrl}," +
                    $"\n{nameof(QuestionStatistics)}: {QuestionStatistics}," +
                    $"\n{nameof(SubmissionStatistics)}: {SubmissionStatistics}," +
                    $"\n{nameof(Links)}: {Links}").Indent(4) +
                   "\n}";
        }
    }

    /// <summary>
    /// A container for quiz statistics responses.
    /// </summary>
    [PublicAPI]
    public class QuizStatisticsResponse : IPrettyPrint {
        private readonly Api api;
        
        [CanBeNull]
        public IEnumerable<QuizStatistics> Statistics { get; }
        
        [CanBeNull]
        public JToken Links { get; }

        internal QuizStatisticsResponse(Api api, QuizStatisticsListModel model) {
            this.api = api;
            Statistics = model.QuizStatistics?.Select(s => new QuizStatistics(api, s));
            Links = model.Links;
        }

        public string ToPrettyString() {
            return "QuizStatisticsResponse {" +
                   ($"\n{nameof(Statistics)}: {Statistics?.ToPrettyString()}," +
                    $"\n{nameof(Links)}: {Links}").Indent(4) +
                   "\n}";
        }
    }
}
