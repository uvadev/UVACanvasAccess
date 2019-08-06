using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UVACanvasAccess.Model.Calendar;

namespace UVACanvasAccess.Model.Appointments {
    
    internal class AppointmentGroupModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("start_at")]
        public DateTime StartAt { get; set; }
        
        [JsonProperty("end_at")]
        public DateTime EndAt { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("location_name")]
        public string LocationName { get; set; }
        
        [JsonProperty("location_address")]
        public string LocationAddress { get; set; }
        
        [JsonProperty("participant_count")]
        public uint? ParticipantCount { get; set; }
        
        [JsonProperty("reserved_times")]
        public IEnumerable<AppointmentModel> ReservedTimes { get; set; }
        
        [JsonProperty("context_codes")]
        public IEnumerable<string> ContextCodes { get; set; }
        
        [JsonProperty("sub_context_codes")]
        public IEnumerable<string> SubContextCodes { get; set; }
        
        [JsonProperty("workflow_state")]
        public string WorkflowState { get; set; }
        
        [JsonProperty("requiring_action")]
        public bool? RequiringAction { get; set; }
        
        [JsonProperty("appointments_count")]
        public uint AppointmentsCount { get; set; }
        
        [JsonProperty("appointments")]
        public IEnumerable<CalendarEventModel> Appointments { get; set; }
        
        [JsonProperty("new_appointments")]
        [CanBeNull]
        public IEnumerable<CalendarEventModel> NewAppointments { get; set; }
        
        [JsonProperty("max_appointments_per_participant")]
        public uint? MaxAppointmentsPerParticipant { get; set; }
        
        [JsonProperty("min_appointments_per_participant")]
        public uint? MinAppointmentsPerParticipant { get; set; }
        
        [JsonProperty("participants_per_appointment")]
        public uint? ParticipantsPerAppointment { get; set; }
        
        [JsonProperty("participant_visibility")]
        public string ParticipantVisibility { get; set; }
        
        [JsonProperty("participant_type")]
        public string ParticipantType { get; set; }
        
        [JsonProperty("url")]
        public string Url { get; set; }
        
        [JsonProperty("html")]
        public string HtmlUrl { get; set; }
        
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
