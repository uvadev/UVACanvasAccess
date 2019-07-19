using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Courses {
    
    internal struct CalendarLinkModel {
        [JsonProperty("ics")]
        public string Ics { get; set; }
    }
}