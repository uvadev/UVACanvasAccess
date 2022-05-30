using System;
using JetBrains.Annotations;
using UVACanvasAccess.Model.Analytics;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Analytics {
    
    /// <summary>
    /// A summary of per-assignment statistical information for a course.
    /// </summary>
    [PublicAPI]
    public class CourseAssignmentSummary : IPrettyPrint {
        /// <summary>
        /// The assignment id.
        /// </summary>
        public ulong AssignmentId { get; }
        
        /// <summary>
        /// The assignment title.
        /// </summary>
        public string Title { get; }
        
        /// <summary>
        /// The assignment's due date.
        /// </summary>
        public DateTime DueAt { get; }
        
        /// <summary>
        /// The date at which the assignment unlocks.
        /// </summary>
        public DateTime? UnlockAt { get; }
        
        /// <summary>
        /// Whether the assignment is muted.
        /// </summary>
        public bool Muted { get; }
        
        /// <summary>
        /// The maximum points possible for the assignment.
        /// </summary>
        public decimal PointsPossible { get; }
        
        /// <summary>
        /// Whether the assignment is to be submitted non-digitally; i.e. on paper or outside of Canvas.
        /// </summary>
        public bool? NonDigitalSubmission { get; }
        
        /// <summary>
        /// The maximum score any student has earned on this assignment.
        /// </summary>
        public decimal? MaxScore { get; }
        
        /// <summary>
        /// The minimum score any student has earned on this assignment.
        /// </summary>
        public decimal? MinScore { get; }
        
        /// <summary>
        /// The first quartile (Q1) score.
        /// </summary>
        public decimal? FirstQuartile { get; }
        
        /// <summary>
        /// The median score.
        /// </summary>
        public decimal? Median { get; }
        
        /// <summary>
        /// The third quartile (Q3) score.
        /// </summary>
        public decimal? ThirdQuartile { get; }
        
        /// <summary>
        /// A breakdown of on-time/lateness statuses for the assignment.
        /// </summary>
        /// <seealso cref="Tardiness"/>
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

        /// <inheritdoc/>
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
