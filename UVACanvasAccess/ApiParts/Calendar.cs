using System;
using System.Collections.Generic;
using System.Linq;
using UVACanvasAccess.Model.Calendar;
using UVACanvasAccess.Structures.Calendar;
using static UVACanvasAccess.Util.Extensions;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {

        // todo string type -> enum
        // todo better way for context codes
        public async IAsyncEnumerable<CalendarEvent> StreamCalendarEvents(ulong? userId = null, 
                                                                          string type = null, 
                                                                          DateTime? startDate = null,
                                                                          DateTime? endDate = null,
                                                                          bool? undated = null,
                                                                          bool? allEvents = null,
                                                                          IEnumerable<string> contextCodes = null) {
            IEnumerable<(string, string)> a = new[] {
                ("type", type),
                ("start_date", startDate?.ToIso8601Date()),
                ("end_date", endDate?.ToIso8601Date()),
                ("undated", undated?.ToShortString()),
                ("all_events", allEvents?.ToShortString())
            };

            if (contextCodes != null) {
                a = a.Concat(contextCodes.Select(cc => ("context_codes[]", cc)));
            }

            var response = await _client.GetAsync($"users/{userId?.ToString() ?? "self"}/calendar_events" +  BuildDuplicateKeyQueryString(a.ToArray()));

            var models = StreamDeserializePages<CalendarEventModel>(response);

            await foreach (var model in models) {
                yield return CalendarEvent.FromModel(this, model);
            }
        }
    }
}
