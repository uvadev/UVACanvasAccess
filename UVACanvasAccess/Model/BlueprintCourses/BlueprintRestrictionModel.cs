using Newtonsoft.Json;

namespace UVACanvasAccess.Model.BlueprintCourses {
    
    internal class BlueprintRestrictionModel {
        
        [JsonProperty("content")]
        public bool? Content { get; set; }
        
        [JsonProperty("points")]
        public bool? Points { get; set; }
        
        [JsonProperty("due_dates")]
        public bool? DueDates { get; set; }
        
        [JsonProperty("availability_dates")]
        public bool? AvailabilityDates { get; set; }
    }
}
