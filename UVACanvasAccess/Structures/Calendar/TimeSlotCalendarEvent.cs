using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Calendar;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Calendar {

    /// <summary>
    /// Represents a possible time-slot appointment within an appointment group.
    /// Actual reserved appointments within this time-slot are represented by <see cref="UserReservationCalendarEvent"/>.
    /// </summary>
    [PublicAPI]
    public class TimeSlotCalendarEvent : BasicCalendarEvent {
        
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
        /// The API url which can be followed to reserve this time-slot.
        /// </summary>
        public string ReserveUrl { get; }
        
        /// <summary>
        /// Whether this slot has been reserved.
        /// </summary>
        public bool? Reserved { get; }
        
        /// <summary>
        /// The maximum amount of participants which may reserve this slot.
        /// </summary>
        public uint? ParticipantsPerAppointment { get; }
        
        /// <summary>
        /// The amount of participants which may yet reserve this slot.
        /// </summary>
        public uint? AvailableSlots { get; }
        
        /// <summary>
        /// Any reservations which have been made.
        /// </summary>
        public IEnumerable<UserReservationCalendarEvent> Reservations { get; }

        internal TimeSlotCalendarEvent(Api api, CalendarEventModel model) : base(api, model) {
            AppointmentGroupId = model.AppointmentGroupId;
            AppointmentGroupUrl = model.AppointmentGroupUrl;
            CanManageAppointmentGroup = model.CanManageAppointmentGroup;
            ReserveUrl = model.ReserveUrl;
            Reserved = model.Reserved;
            ParticipantsPerAppointment = model.ParticipantsPerAppointment;
            AvailableSlots = model.AvailableSlots;
            Reservations = model.ChildEvents?.Select(m => new UserReservationCalendarEvent(api, m)) 
                                            ?? new List<UserReservationCalendarEvent>();
        }
        
        /// <inheritdoc />
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
