using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Calendar;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Calendar {

    /// <summary>
    /// Represents a calendar event.
    /// </summary>
    [PublicAPI]
    public abstract class CalendarEvent : IPrettyPrint {
        private protected readonly Api Api;
        
        /// <summary>
        /// The event id.
        /// </summary>
        public ulong Id { get; }
        
        /// <summary>
        /// The event title.
        /// </summary>
        public string Title { get; }
        
        /// <summary>
        /// When the event begins.
        /// </summary>
        public DateTime StartAt { get; }
        
        /// <summary>
        /// When the event ends.
        /// </summary>
        public DateTime EndAt { get; }
        
        /// <summary>
        /// The event type; 'event' or 'assignment'.
        /// </summary>
        public string Type { get; }
        
        /// <summary>
        /// The event description.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// The context code of the event, indicating which calendar the event belongs to.
        /// The code can begin with {'user_', 'course_', 'group_'} followed by the appropriate id.
        /// </summary>
        public string ContextCode { get; }
        
        /// <summary>
        /// If applicable, a more specific context code under <see cref="ContextCode"/>.
        /// </summary>
        [CanBeNull]
        public string EffectiveContextCode { get; }
        
        /// <summary>
        /// All context codes this event is under.
        /// </summary>
        /// <seealso cref="ContextCode"/>
        public IEnumerable<string> AllContextCodes { get; }
        
        public CalendarState? WorkflowState { get; }
        
        /// <summary>
        /// Whether the event is hidden from the calendar.
        /// </summary>
        public bool Hidden { get; }
        
        /// <summary>
        /// If the event has a parent, such as a timeslot or a course-level event above a section-level event, its id.
        /// </summary>
        [CanBeNull]
        public string ParentEventId { get; }
        
        /// <summary>
        /// The amount of child events.
        /// </summary>
        public uint? ChildEventsCount { get; }
        
        /// <summary>
        /// The child events. For timeslots, these will be any reservations. For a course-level event, these will be
        /// section-level events.
        /// </summary>
        [OptIn]
        [CanBeNull]
        public IEnumerable<CalendarEvent> ChildEvents { get; }
        
        /// <summary>
        /// The API url for this event.
        /// </summary>
        public string Url { get; }
        
        /// <summary>
        /// The Canvas web url for this event.
        /// </summary>
        public string HtmlUrl { get; }
        
        /// <summary>
        /// The date of this event.
        /// </summary>
        public DateTime? AllDayDate { get; }
        
        /// <summary>
        /// Whether this event is all-day.
        /// </summary>
        public bool AllDay { get; }
        
        /// <summary>
        /// When the event was created.
        /// </summary>
        public DateTime CreatedAt { get; }
        
        /// <summary>
        /// When the event was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; }

        /// <inheritdoc />
        public virtual string ToPrettyString() {
            return "CalendarEvent {" + 
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
                   $"\n{nameof(UpdatedAt)}: {UpdatedAt}").Indent(4) + 
                   "\n}";
        }

        private protected CalendarEvent(Api api, CalendarEventModel model) {
            Api = api;
            Id = model.Id;
            Title = model.Title;
            StartAt = model.StartAt;
            EndAt = model.EndAt;
            Type = model.Type;
            Description = model.Description;
            ContextCode = model.ContextCode;
            EffectiveContextCode = model.EffectiveContextCode;
            AllContextCodes = model.AllContextCodes.Split(',');
            WorkflowState = model.WorkflowState?.ToApiRepresentedEnum<CalendarState>();
            Hidden = model.Hidden;
            ParentEventId = model.ParentEventId;
            ChildEvents = model.ChildEvents.SelectNotNull(child => FromModel(api, child));
            ChildEventsCount = model.ChildEventsCount;
            Url = model.Url;
            HtmlUrl = model.HtmlUrl;
            AllDayDate = model.AllDayDate;
            AllDay = model.AllDay;
            CreatedAt = model.CreatedAt;
            UpdatedAt = model.UpdatedAt;
        }

        internal static CalendarEvent FromModel(Api api, CalendarEventModel model) {
            if (model.ReserveUrl != null) {
                return new TimeSlotCalendarEvent(api, model);
            } if (model.User != null) {
                return new UserReservationCalendarEvent(api, model);
            } if (model.Type == "event") {
                return new BasicCalendarEvent(api, model);
            }

            throw new NotImplementedException("CalendarEvent::FromModel didn't recognize model");
        }
    }

    /// <summary>
    /// Represents a workflow state a <see cref="CalendarEvent"/> can be in.
    /// </summary>
    [PublicAPI]
    public enum CalendarState {
        /// <summary>
        /// The event is active.
        /// </summary>
        [ApiRepresentation("active")]
        Active,
        /// <summary>
        /// The event's start and end dates cannot be changed.
        /// </summary>
        [ApiRepresentation("locked")]
        Locked,
        /// <summary>
        /// The event is deleted.
        /// </summary>
        [ApiRepresentation("deleted")]
        Deleted
    }
}
