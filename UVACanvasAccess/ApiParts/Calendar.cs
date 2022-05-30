using System;
using System.Collections.Generic;
using System.Linq;
using UVACanvasAccess.Model.Calendar;
using UVACanvasAccess.Structures.Calendar;
using static UVACanvasAccess.Util.Extensions;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {
        
        /// <summary>
        /// Stream calendar events.
        /// </summary>
        /// <param name="userId">(Optional) The user to filter by.</param>
        /// <param name="type">(Optional) The event type to filter by.</param>
        /// <param name="startDate">(Optional) The beginning of the date range to search.</param>
        /// <param name="endDate">(Optional) The end of the date range to search.</param>
        /// <param name="undated">(Optional) Allow undated events.</param>
        /// <param name="allEvents">(Optional) Include all events.</param>
        /// <param name="contexts">Event contexts to search.</param>
        /// <returns></returns>
        public async IAsyncEnumerable<CalendarEvent> StreamCalendarEvents(ulong? userId = null, 
                                                                          EventType? type = null, 
                                                                          DateTime? startDate = null,
                                                                          DateTime? endDate = null,
                                                                          bool? undated = null,
                                                                          bool? allEvents = null,
                                                                          IEnumerable<EventContext> contexts = null) {
            IEnumerable<(string, string)> a = new[] {
                ("type", type?.GetApiRepresentation()),
                ("start_date", startDate?.ToIso8601Date()),
                ("end_date", endDate?.ToIso8601Date()),
                ("undated", undated?.ToShortString()),
                ("all_events", allEvents?.ToShortString())
            };

            if (contexts != null) {
                var eventContexts = contexts.ToList();
                a = a.Concat(eventContexts.Select(cc => ("context_codes[]", cc.ContextCode)));
            }

            var response = await client.GetAsync($"users/{userId?.ToString() ?? "self"}/calendar_events" + BuildDuplicateKeyQueryString(a.ToArray()));
            
            await foreach (var model in StreamDeserializePages<CalendarEventModel>(response)) {
                yield return CalendarEvent.FromModel(this, model);
            }
        }
        
        
    }
}
