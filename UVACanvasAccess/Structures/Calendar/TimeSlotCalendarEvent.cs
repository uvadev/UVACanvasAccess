using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Calendar;
using UVACanvasAccess.Structures.Users;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Calendar {
    
    /*
      { 
        "id": 1201,
        "title": "Test Appointment Group",
        "start_at": "2019-08-07T23:24:00Z",
        "end_at": "2019-08-08T02:24:00Z",
        "workflow_state": "active",
        "created_at": "2019-08-02T17:17:29Z",
        "updated_at": "2019-08-02T17:17:29Z",
        "all_day": false,
        "all_day_date": null,
        "comments": null,
        "location_address": null,
        "location_name": "",
        "type": "event",
        "description": "TEST appointment group details",
        "child_events_count": 0,
        "effective_context_code": "course_1028",
        "all_context_codes": "course_1028",
        "context_code": "appointment_group_58",
        "parent_event_id": null,
        "hidden": false,
        "appointment_group_id": 58,
        "appointment_group_url": "https://uview.instructure.com/api/v1/appointment_groups/58",
        "can_manage_appointment_group": true,
        "participant_type": "User",
        "reserve_url": "https://uview.instructure.com/api/v1/calendar_events/1201/reservations/%7B%7B%20id%20%7D%7D",
        "available_slots": 2,
        "participants_per_appointment": 2,
        "url": "https://uview.instructure.com/api/v1/calendar_events/1201",
        "html_url": "https://uview.instructure.com/calendar?event_id=1201&include_contexts=course_1028",
        "duplicates": []
      }
     */
    
    [PublicAPI]
    public class TimeSlotCalendarEvent : BasicCalendarEvent {
        
        public ulong AppointmentGroupId { get; }
        
        public string AppointmentGroupUrl { get; }
        
        public bool? CanManageAppointmentGroup { get; }
        
        public string ReserveUrl { get; }
        
        public bool? Reserved { get; }
        
        public uint? ParticipantsPerAppointment { get; }
        
        public IEnumerable<UserReservationCalendarEvent> Reservations { get; }

        internal TimeSlotCalendarEvent(Api api, CalendarEventModel model) : base(api, model) {
            AppointmentGroupId = model.AppointmentGroupId;
            AppointmentGroupUrl = model.AppointmentGroupUrl;
            CanManageAppointmentGroup = model.CanManageAppointmentGroup;
            ReserveUrl = model.ReserveUrl;
            Reserved = model.Reserved;
            ParticipantsPerAppointment = model.ParticipantsPerAppointment;
            Reservations = model.ChildEvents?.Select(m => new UserReservationCalendarEvent(api, m)) 
                                            ?? new List<UserReservationCalendarEvent>();
        }
        
        public override string ToPrettyString() {
            return "TimeSlotCalendarEvent {" + 
                   ($"\n{nameof(Id)}: {Id}," +
                    $"\n{nameof(Title)}: {Title}," +
                    $"\n{nameof(StartAt)}: {StartAt}," +
                    $"\n{nameof(EndAt)}: {EndAt}," +
                    $"\n{nameof(Type)}: {Type}," +
                    $"\n{nameof(Description)}: {Description}," +
                    $"\n{nameof(ContextCode)}: {ContextCode}," +
                    $"\n{nameof(EffectiveContextCode)}: {EffectiveContextCode}," +
                    $"\n{nameof(AllContextCodes)}: {AllContextCodes.ToPrettyString()}," +
                    $"\n{nameof(WorkflowState)}: {WorkflowState}," +
                    $"\n{nameof(Hidden)}: {Hidden}," +
                    $"\n{nameof(ParentEventId)}: {ParentEventId}," +
                    $"\n{nameof(ChildEventsCount)}: {ChildEventsCount}," +
                    $"\n{nameof(ChildEvents)}: {ChildEvents?.ToPrettyString()}," +
                    $"\n{nameof(Url)}: {Url}," +
                    $"\n{nameof(HtmlUrl)}: {HtmlUrl}," +
                    $"\n{nameof(AllDayDate)}: {AllDayDate}," +
                    $"\n{nameof(AllDay)}: {AllDay}," +
                    $"\n{nameof(CreatedAt)}: {CreatedAt}," +
                    $"\n{nameof(UpdatedAt)}: {UpdatedAt}," +
                    $"\n{nameof(LocationAddress)}: {LocationAddress}," +
                    $"\n{nameof(LocationName)}: {LocationName}," + 
                    $"\n{nameof(AppointmentGroupId)}: {AppointmentGroupId}," +
                    $"\n{nameof(AppointmentGroupUrl)}: {AppointmentGroupUrl}," +
                    $"\n{nameof(CanManageAppointmentGroup)}: {CanManageAppointmentGroup}," +
                    $"\n{nameof(ReserveUrl)}: {ReserveUrl}," +
                    $"\n{nameof(Reserved)}: {Reserved}," + 
                    $"\n{nameof(ParticipantsPerAppointment)}: {ParticipantsPerAppointment}," +
                    $"\n{nameof(Reservations)}: {Reservations?.ToPrettyString()},").Indent(4) + 
                    "\n}";
        }
    }
}
