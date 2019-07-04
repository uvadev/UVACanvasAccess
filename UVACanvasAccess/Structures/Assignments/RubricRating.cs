using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Assignments;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Assignments {
    
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
    // ReSharper disable MemberCanBePrivate.Global
    public class RubricRating : IPrettyPrint {
        private readonly Api _api;
        
        public uint Points { get; }
        
        public string Id { get; }
        
        public string Description { get; }
        
        public string LongDescription { get; }

        public RubricRating(Api api, RubricRatingModel model) {
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