using System.Collections.Generic;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.ProficiencyRatings {
    
    internal struct ProficiencyModel {
        [JsonProperty("ratings")]
        public IEnumerable<ProficiencyRatingModel> Ratings { get; set; }
    }
}
