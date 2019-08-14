using System.Collections.Generic;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.ProficiencyRatings;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.ProficiencyRatings {
    
    [PublicAPI]
    public readonly struct Proficiency : IPrettyPrint {
        private readonly Api _api;
        public IEnumerable<ProficiencyRating> Ratings { get; }

        internal Proficiency(Api api, ProficiencyModel model) {
            _api = api;
            Ratings = model.Ratings.SelectNotNull(r => new ProficiencyRating(api, r));
        }

        public string ToPrettyString() {
            return "Proficiency {" + 
                   ($"\n{nameof(Ratings)}: {Ratings.ToPrettyString()}").Indent(4) + 
                   "\n}";
        }
    }
}
