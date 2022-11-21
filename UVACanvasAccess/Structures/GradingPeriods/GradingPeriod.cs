using System;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.GradingPeriods;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.GradingPeriods {
    
    /// <summary>
    /// Represents a grading period.
    /// </summary>
    [PublicAPI]
    public class GradingPeriod : IPrettyPrint {
        private readonly Api api;
        
        /// <summary>
        /// The grading period id.
        /// </summary>
        public ulong Id { get; }
        
        /// <summary>
        /// The title of the grading period.
        /// </summary>
        public string Title { get; }
        
        /// <summary>
        /// The start date of the grading period.
        /// </summary>
        public DateTime? StartDate { get; }

        /// <summary>
        /// The end date of the grading period.
        /// </summary>
        public DateTime? EndDate { get; }
        
        /// <summary>
        /// The close date of the grading period.
        /// </summary>
        public DateTime? CloseDate { get; }
        
        /// <summary>
        /// The weight value of the grading period.
        /// </summary>
        public double? Weight { get; }
        
        /// <summary>
        /// Whether the grading period is closed.
        /// </summary>
        public bool? IsClosed { get; }
        
        internal GradingPeriod(Api api, GradingPeriodModel model) {
            this.api = api;
            Id = model.Id;
            Title = model.Title;
            StartDate = model.StartDate;
            EndDate = model.EndDate;
            CloseDate = model.CloseDate;
            Weight = model.Weight;
            IsClosed = model.IsClosed;
        }

        /// <inheritdoc />
        public string ToPrettyString() {
            return "GradingPeriod {" +
                   ($"\n{nameof(Id)}: {Id}," +
                    $"\n{nameof(Title)}: {Title}," +
                    $"\n{nameof(StartDate)}: {StartDate}," +
                    $"\n{nameof(EndDate)}: {EndDate}," +
                    $"\n{nameof(CloseDate)}: {CloseDate}," +
                    $"\n{nameof(Weight)}: {Weight}," +
                    $"\n{nameof(IsClosed)}: {IsClosed}").Indent(4) + 
                   "\n}";
        }
    }
}
