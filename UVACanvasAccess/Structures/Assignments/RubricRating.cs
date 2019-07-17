using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Assignments;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Assignments {
    
    /// <summary>
    /// Represents one rating as part of a rubric.
    /// </summary>
    [PublicAPI]
    public class RubricRating : IPrettyPrint {
        private readonly Api _api;
        
        public uint Points { get; }
        
        public string Id { get; }
        
        public string Description { get; }
        
        public string LongDescription { get; }

        internal RubricRating(Api api, RubricRatingModel model) {
            _api = api;
            Points = model.Points;
            Id = model.Id;
            Description = model.Description;
            LongDescription = model.LongDescription;
        }

        public string ToPrettyString() {
            return "RubricRating {" + 
                   ($"\n{nameof(Points)}: {Points}," +
                   $"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(Description)}: {Description}," +
                   $"\n{nameof(LongDescription)}: {LongDescription}").Indent(4) +
                   "\n}";
        }
    }
}