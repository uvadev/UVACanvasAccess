using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Calendar;
using UVACanvasAccess.Structures.Users;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Calendar {
    
    /*
     {
       "id": 2562,
       "title": "Test Appointment Group",
       "start_at": "2019-08-07T03:34:00Z",
       "end_at": "2019-08-07T13:12:00Z",
       "workflow_state": "locked",
       "created_at": "2019-08-06T16:45:15Z",
       "updated_at": "2019-08-06T16:45:15Z",
       "all_day": false,
       "all_day_date": null,
       "comments": "",
       "location_address": null,
       "location_name": "",
       "type": "event",
       "description": "TEST appointment group details",
       "child_events_count": 0,
       "effective_context_code": "course_1028",
       "all_context_codes": "course_1028",
       "context_code": "user_3392",
       "parent_event_id": 1202,
       "hidden": false,
       "user": {
         "id": 3392,
         "name": "UVACanvasAccess Test Account 2",
         "created_at": "2019-06-20T11:16:11-05:00",
         "sortable_name": "2, UVACanvasAccess Test Account",
         "short_name": "UVACanvasAccess Test Account 2",
         "sis_user_id": null,
         "integration_id": null,
         "sis_import_id": null,
         "login_id": "CANVAS_ACCESS_TEST_2"
       },
       "appointment_group_id": 58,
       "appointment_group_url": "https://uview.instructure.com/api/v1/appointment_groups/58",
       "can_manage_appointment_group": true,
       "participant_type": "User",
       "url": "https://uview.instructure.com/api/v1/calendar_events/2562",
       "html_url": "https://uview.instructure.com/calendar?event_id=2562&include_contexts=course_1028",
       "duplicates": []
      }
     */
    
    [PublicAPI]
    public class UserReservationCalendarEvent : BasicCalendarEvent {
        public ulong AppointmentGroupId { get; }
        
        public string AppointmentGroupUrl { get; }
        
        public bool? CanManageAppointmentGroup { get; }
        
        public User User { get; set; }

        internal UserReservationCalendarEvent(Api api, CalendarEventModel model) : base(api, model) {
            AppointmentGroupId = model.AppointmentGroupId;
            AppointmentGroupUrl = model.AppointmentGroupUrl;
            User = model.User.ConvertIfNotNull(m => new User(api, m));
        }

        public override string ToPrettyString() {
            return "UserReservationCalendarEvent {" + 
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
                    $"\n{nameof(User)}: {User.ToPrettyString()}").Indent(4) + 
                    "\n}";
        }
    }
}
