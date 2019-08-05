using System;
using System.Collections.Generic;
using System.Linq;
using UVACanvasAccess.Model.Calendar;
using UVACanvasAccess.Structures.Calendar;
using static UVACanvasAccess.Util.Extensions;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {
        
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
                if (eventContexts.Count > 10) {
                    Logger.Warn("StreamCalendarEvent allows at most 10 contexts. Additional ones are being ignored.");
                }
                a = a.Concat(eventContexts.Select(cc => ("context_codes[]", cc.ContextCode)));
            }

            var response = await _client.GetAsync($"users/{userId?.ToString() ?? "self"}/calendar_events" + BuildDuplicateKeyQueryString(a.ToArray()));
            
            await foreach (var model in StreamDeserializePages<CalendarEventModel>(response)) {
                yield return CalendarEvent.FromModel(this, model);
            }
        }
    }
}
