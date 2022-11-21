using System;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;

namespace UVACanvasAccess.Structures.GradingPeriods {
    
    /// <summary>
    /// Used to perform batch grading period updates. <br/>
    /// Each object represents one updated or newly-created grading period.
    /// </summary>
    /// <seealso cref="Api.BatchUpdateGradingPeriods"/>
    /// <seealso cref="Api.BatchUpdateCourseGradingPeriods"/>
    [PublicAPI]
    public class GradingPeriodUpdate {
        
        /// <summary>
        /// The id of the grading period to update. <br/>
        /// If null, this object represents a new grading period to be created.
        /// </summary>
        public ulong? Id { get; }
        
        /// <inheritdoc cref="GradingPeriod.Title"/>
        public string Title { get; }
        
        /// <inheritdoc cref="GradingPeriod.StartDate"/>
        public DateTime? StartDate { get; }

        /// <inheritdoc cref="GradingPeriod.EndDate"/>
        public DateTime? EndDate { get; }
        
        /// <inheritdoc cref="GradingPeriod.CloseDate"/>
        public DateTime? CloseDate { get; }
        
        /// <inheritdoc cref="GradingPeriod.Weight"/>
        public double? Weight { get; }
        
        /// <inheritdoc cref="GradingPeriod.IsClosed"/>
        public bool? IsClosed { get; }
        
        public GradingPeriodUpdate(ulong? id = null,
                                   string title = null,
                                   DateTime? startDate = null,
                                   DateTime? endDate = null,
                                   DateTime? closeDate = null,
                                   double? weight = null,
                                   bool? isClosed = null) {
            Id = id;
            Title = title;
            StartDate = startDate;
            EndDate = endDate;
            CloseDate = closeDate;
            Weight = weight;
            IsClosed = isClosed;
        }
    }
}
