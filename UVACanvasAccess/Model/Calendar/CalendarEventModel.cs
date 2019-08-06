using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UVACanvasAccess.Model.Assignments;
using UVACanvasAccess.Model.Users;

namespace UVACanvasAccess.Model.Calendar {
    
    /*
     * This class combines the fields of normal, reservation, time-slot, and assignment calendar events.
     * Concrete structure classes will specialize to these types and inherit from a common base.
     */
    
    internal class CalendarEventModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("start_at")]
        public DateTime StartAt { get; set; }
        
        [JsonProperty("end_at")]
        public DateTime EndAt { get; set; }
        
        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("location_name")]
        public string LocationName { get; set; }
        
        [JsonProperty("location_address")]
        public string LocationAddress { get; set; }
        
        [JsonProperty("context_code")]
        public string ContextCode { get; set; }
        
        [CanBeNull]
        [JsonProperty("effective_context_code")]
        public string EffectiveContextCode { get; set; }
        
        [JsonProperty("all_context_codes")]
        public string AllContextCodes { get; set; } // comma separated
        
        [JsonProperty("workflow_state")]
        public string WorkflowState { get; set; }
        
        [JsonProperty("hidden")]
        public bool Hidden { get; set; }
        
        [CanBeNull]
        [JsonProperty("parent_event_id")]
        public string ParentEventId { get; set; }
        
        [JsonProperty("child_events_count")]
        public uint? ChildEventsCount { get; set; }
        
        [CanBeNull]
        [JsonProperty("child_events")]
        public IEnumerable<CalendarEventModel> ChildEvents { get; set; }
        
        [JsonProperty("url")]
        public string Url { get; set; }
        
        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }
        
        [JsonProperty("all_day_date")]
        public DateTime? AllDayDate { get; set; }
        
        [JsonProperty("all_day")]
        public bool AllDay { get; set; }
        
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
        
        [JsonProperty("appointment_group_id")]
        public ulong AppointmentGroupId { get; set; }
        
        [JsonProperty("appointment_group_url")]
        public string AppointmentGroupUrl { get; set; }
        
        [JsonProperty("own_reservation")]
        public bool? OwnReservation { get; set; }
        
        [CanBeNull]
        [JsonProperty("reserve_url")]
        public string ReserveUrl { get; set; }
        
        [JsonProperty("reserved")]
        public bool? Reserved { get; set; }
        
        [JsonProperty("participant_type")]
        public string ParticipantType { get; set; } // User|Group
        
        [JsonProperty("participant_limit")]
        public uint? ParticipantLimit { get; set; }
        
        [JsonProperty("available_slots")]
        public uint? AvailableSlots { get; set; }
        
        [CanBeNull]
        [JsonProperty("user")]
        public UserModel User { get; set; }
        
        [CanBeNull]
        [JsonProperty("group")]
        public object Group { get; set; } // todo Groups api
        
        [CanBeNull]
        [JsonProperty("assignment")]
        public AssignmentModel Assignment { get; set; }
        
        [CanBeNull]
        [JsonProperty("assignment_overrides")]
        public IEnumerable<AssignmentOverrideModel> AssignmentOverrides { get; set; }
        
        [JsonProperty("can_manage_appointment_group")]
        public bool? CanManageAppointmentGroup { get; set; } // undocumented
        
        [JsonProperty("participants_per_appointment")]
        public uint? ParticipantsPerAppointment { get; set; }
    }
}
