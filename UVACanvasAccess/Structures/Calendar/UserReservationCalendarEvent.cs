using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Calendar;
using UVACanvasAccess.Structures.Users;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Calendar {
    
    /// <summary>
    /// Represents a user appointment within a <see cref="TimeSlotCalendarEvent"/> of an appointment group.
    /// </summary>
    [PublicAPI]
    public class UserReservationCalendarEvent : BasicCalendarEvent {
        
        /// <summary>
        /// The id of the appointment group this event belongs to.
        /// </summary>
        public ulong AppointmentGroupId { get; }
        
        /// <summary>
        /// The API url of the appointment group this event belongs to.
        /// </summary>
        public string AppointmentGroupUrl { get; }
        
        public bool? CanManageAppointmentGroup { get; }
        
        /// <summary>
        /// The user who made the reservation.
        /// </summary>
        public User User { get; }

        internal UserReservationCalendarEvent(Api api, CalendarEventModel model) : base(api, model) {
            AppointmentGroupId = model.AppointmentGroupId;
            AppointmentGroupUrl = model.AppointmentGroupUrl;
            User = model.User.ConvertIfNotNull(m => new User(api, m));
        }

        /// <inheritdoc />
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
