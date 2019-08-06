using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UVACanvasAccess.Model.Appointments;
using UVACanvasAccess.Structures.Appointments;
using UVACanvasAccess.Structures.Calendar;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {

        [PublicAPI]
        public enum AppointmentVisibilityScope : byte {
            [ApiRepresentation("reservable")]
            Reservable,
            [ApiRepresentation("manageable")]
            Manageable
        }

        [PublicAPI]
        [Flags]
        public enum AppointmentGroupIncludes : byte {
            [ApiRepresentation("appointments")]
            Appointments = 1 << 0,
            [ApiRepresentation("child_events")]
            Reservations = 1 << 1,
            [ApiRepresentation("participant_count")]
            ParticipantCount = 1 << 2,
            [ApiRepresentation("reserved_times")]
            ReservedTimes = 1 << 3,
            [ApiRepresentation("all_context_codes")]
            AllContextCodes = 1 << 4,
            Everything = byte.MaxValue
        }
        
        public async IAsyncEnumerable<AppointmentGroup> StreamAppointmentGroups(AppointmentVisibilityScope? scope = null,
                                                                                bool? includePast = null,
                                                                                AppointmentGroupIncludes? includes = null,
                                                                                IEnumerable<EventContext> contexts = null) {

            IEnumerable<(string, string)> args = new[] {
                ("scope", scope?.GetApiRepresentation()),
                ("include_past_appointments", includePast?.ToShortString())
            };

            if (includes != null) {
                args = args.Concat(includes.GetFlagsApiRepresentations().Select(r => ("include[]", r)));
            }

            if (contexts != null) {
                args = args.Concat(contexts.Select(c => ("context_codes[]", c.ContextCode)));
            }
            
            var response = await _client.GetAsync("appointment_groups" + BuildDuplicateKeyQueryString(args.ToArray()));

            await foreach (var model in StreamDeserializePages<AppointmentGroupModel>(response)) {
                yield return new AppointmentGroup(this, model);
            }
        }
    }
}
