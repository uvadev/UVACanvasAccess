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

        /// <summary>
        /// The kind of relationship a user can have with an appointment group.
        /// </summary>
        [PublicAPI]
        public enum AppointmentVisibilityScope : byte {
            /// <summary>
            /// The user can make a reservation in the appointment group.
            /// </summary>
            [ApiRepresentation("reservable")]
            Reservable,
            /// <summary>
            /// The user can manage the appointment group.
            /// </summary>
            [ApiRepresentation("manageable")]
            Manageable
        }

        /// <summary>
        /// Data that can be included in responses containing <see cref="AppointmentGroup">appointment groups.</see>
        /// </summary>
        [PublicAPI]
        [Flags]
        public enum AppointmentGroupIncludes : byte {
            /// <summary>
            /// The individual calendar events representing each time slot.
            /// </summary>
            [ApiRepresentation("appointments")]
            Appointments = 1 << 0,
            /// <summary>
            /// The individual reservations, if any exist.
            /// </summary>
            [ApiRepresentation("child_events")]
            Reservations = 1 << 1,
            /// <summary>
            /// The number of reservations.
            /// </summary>
            [ApiRepresentation("participant_count")]
            ParticipantCount = 1 << 2,
            /// <summary>
            /// The id, start time, and end time of reservations the current user has made in the appointment group.
            /// </summary>
            [ApiRepresentation("reserved_times")]
            ReservedTimes = 1 << 3,
            /// <summary>
            /// All context codes associated with the appointment group.
            /// </summary>
            [ApiRepresentation("all_context_codes")]
            AllContextCodes = 1 << 4,
            Everything = byte.MaxValue
        }
        
        /// <summary>
        /// Streams the appointment groups that can be managed or reserved by the current user.
        /// </summary>
        /// <param name="scope">
        /// The kind of relationship the current user must have with the returned groups.
        /// <see cref="AppointmentVisibilityScope.Reservable"/> by default.
        /// </param>
        /// <param name="includePast">Whether or not to include past appointments.</param>
        /// <param name="includes">Additional data to include.</param>
        /// <param name="contexts">Any contexts to filter the results by.</param>
        /// <returns>The stream of appointment groups.</returns>
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
