using System.Collections.Generic;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Assignments;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Assignments {
    
    /// <summary>
    /// Represents some rubric criteria.
    /// </summary>
    [PublicAPI]
    public class RubricCriteria : IPrettyPrint {
        private readonly Api api;
        
        /// <summary>
        /// The total amount of points in this criterion.
        /// </summary>
        public uint? Points { get; }

        /// <summary>
        /// The id of the rubric criterion.
        /// </summary>
        public string Id { get; }
        
        /// <summary>
        /// The id of the learning outcome this rubric uses, if any.
        /// </summary>
        [CanBeNull]
        public string LearningOutcomeId { get; }
        
        /// <summary>
        /// A third-party GUID, if any.
        /// </summary>
        [CanBeNull]
        public string VendorGuid { get; }
        
        /// <summary>
        /// The criterion description.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// The criterion long description.
        /// </summary>
        public string LongDescription { get; }

        /// <summary>
        /// Whether this criterion uses a range.
        /// </summary>
        public bool? CriterionUseRange { get; }
        
        /// <summary>
        /// The list of possible <see cref="RubricRating">ratings</see>.
        /// </summary>
        [CanBeNull]
        public IEnumerable<RubricRating> Ratings { get; }

        /// <summary>
        /// Whether to ignore this criterion when calculating the total score.
        /// </summary>
        public bool? IgnoreForScoring { get; }

        internal RubricCriteria(Api api, RubricCriteriaModel model) {
            this.api = api;
            Points = model.Points;
            Id = model.Id;
            LearningOutcomeId = model.LearningOutcomeId;
            VendorGuid = model.VendorGuid;
            Description = model.Description;
            LongDescription = model.LongDescription;
            CriterionUseRange = model.CriterionUseRange;
            Ratings = model.Ratings.SelectNotNull(m => new RubricRating(api, m));
            IgnoreForScoring = model.IgnoreForScoring;
        }

        /// <inheritdoc />
        public string ToPrettyString() {
            return "RubricCriteria {" +
                   ($"\n{nameof(Points)}: {Points}," +
                   $"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(LearningOutcomeId)}: {LearningOutcomeId}," +
                   $"\n{nameof(VendorGuid)}: {VendorGuid}," +
                   $"\n{nameof(Description)}: {Description}," +
                   $"\n{nameof(LongDescription)}: {LongDescription}," +
                   $"\n{nameof(CriterionUseRange)}: {CriterionUseRange}," +
                   $"\n{nameof(Ratings)}: {Ratings?.ToPrettyString()}," +
                   $"\n{nameof(IgnoreForScoring)}: {IgnoreForScoring}").Indent(4) +
                   "\n}";
        }
    }
}