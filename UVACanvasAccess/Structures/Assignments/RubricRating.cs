using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Assignments;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Assignments {
    
    /// <summary>
    /// Represents one rating as part of a rubric criterion.
    /// </summary>
    [PublicAPI]
    public class RubricRating : IPrettyPrint {
        private readonly Api api;
        
        /// <summary>
        /// The amount of points this rating is worth.
        /// </summary>
        public uint Points { get; }
        
        /// <summary>
        /// The rating id.
        /// </summary>
        public string Id { get; }
        
        /// <summary>
        /// The rating description.
        /// </summary>
        public string Description { get; }
        
        /// <summary>
        /// The long description.
        /// </summary>
        public string LongDescription { get; }

        internal RubricRating(Api api, RubricRatingModel model) {
            this.api = api;
            Points = model.Points;
            Id = model.Id;
            Description = model.Description;
            LongDescription = model.LongDescription;
        }

        /// <inheritdoc />
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