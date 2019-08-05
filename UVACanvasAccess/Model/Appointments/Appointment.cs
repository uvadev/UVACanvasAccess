using System;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Appointments {
    
    internal class Appointment {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("start_at")]
        public DateTime StartAt { get; set; }
        
        [JsonProperty("end_at")]
        public DateTime EndAt { get; set; }
    }
}
