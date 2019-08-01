using System;
using JetBrains.Annotations;
using UVACanvasAccess.Model.Analytics;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Analytics {
    
    [PublicAPI]
    public class CourseAssignmentSummary : IPrettyPrint {
        public ulong AssignmentId { get; }
        
        public string Title { get; }
        
        public DateTime DueAt { get; }
        
        public DateTime? UnlockAt { get; }
        
        public bool Muted { get; }
        
        public decimal PointsPossible { get; }
        
        public bool? NonDigitalSubmission { get; }
        
        public decimal? MaxScore { get; }
        
        public decimal? MinScore { get; }
        
        public decimal? FirstQuartile { get; }
        
        public decimal? Median { get; }
        
        public decimal? ThirdQuartile { get; }
        
        public Tardiness TardinessBreakdown { get; }

        internal CourseAssignmentSummary(CourseAssignmentSummaryModel model) {
            AssignmentId = model.AssignmentId;
            Title = model.Title;
            DueAt = model.DueAt;
            UnlockAt = model.UnlockAt;
            Muted = model.Muted;
            PointsPossible = model.PointsPossible;
            NonDigitalSubmission = model.NonDigitalSubmission;
            MaxScore = model.MaxScore;
            MinScore = model.MinScore;
            FirstQuartile = model.FirstQuartile;
            Median = model.Median;
            ThirdQuartile = model.ThirdQuartile;
            TardinessBreakdown = model.TardinessBreakdown.ConvertIfNotNull(m => new Tardiness(m));
        }

        public string ToPrettyString() {
            return "CourseAssignmentSummary {" +
                   ($"\n{nameof(AssignmentId)}: {AssignmentId}," +
                   $"\n{nameof(Title)}: {Title}," +
                   $"\n{nameof(DueAt)}: {DueAt}," +
                   $"\n{nameof(UnlockAt)}: {UnlockAt}," +
                   $"\n{nameof(Muted)}: {Muted}," +
                   $"\n{nameof(PointsPossible)}: {PointsPossible}," +
                   $"\n{nameof(NonDigitalSubmission)}: {NonDigitalSubmission}," +
                   $"\n{nameof(MaxScore)}: {MaxScore}," +
                   $"\n{nameof(MinScore)}: {MinScore}," +
                   $"\n{nameof(FirstQuartile)}: {FirstQuartile}," +
                   $"\n{nameof(Median)}: {Median}," +
                   $"\n{nameof(ThirdQuartile)}: {ThirdQuartile}," +
                   $"\n{nameof(TardinessBreakdown)}: {TardinessBreakdown.ToPrettyString()}").Indent(4) + 
                   "\n}";
        }
    }
}
