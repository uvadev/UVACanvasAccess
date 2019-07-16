using System.Collections.Generic;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Assignments;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Assignments {
    
    [PublicAPI]
    public class RubricCriteria : IPrettyPrint {
        private readonly Api _api;
        
        public uint Points { get; }

        public string Id { get; }
        
        [CanBeNull]
        public string LearningOutcomeId { get; }
        
        [CanBeNull]
        public string VendorGuid { get; }
        
        public string Description { get; }

        public string LongDescription { get; }

        public bool CriterionUseRange { get; }
        
        [CanBeNull]
        public IEnumerable<RubricRating> Ratings { get; }

        public bool IgnoreForScoring { get; }

        internal RubricCriteria(Api api, RubricCriteriaModel model) {
            _api = api;
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