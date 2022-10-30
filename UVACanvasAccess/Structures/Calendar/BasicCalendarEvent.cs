using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Calendar;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Calendar {

    /// <inheritdoc cref="CalendarEvent"/>
    [PublicAPI]
    public class BasicCalendarEvent : CalendarEvent {
        
        /// <summary>
        /// The event's location.
        /// </summary>
        public string LocationName { get; }
        
        /// <summary>
        /// The event's address.
        /// </summary>
        public string LocationAddress { get; }

        internal BasicCalendarEvent(Api api, CalendarEventModel model) : base(api, model) {
            LocationName = model.LocationName;
            LocationAddress = model.LocationAddress;
        }

        /// <inheritdoc />
        public override string ToPrettyString() {
            return "BasicCalendarEvent {" + 
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
                    $"\n{nameof(LocationName)}: {LocationName}").Indent(4) + 
                    "\n}";
        }
    }
}
