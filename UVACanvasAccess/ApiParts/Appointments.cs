using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UVACanvasAccess.Builders;
using UVACanvasAccess.Model.Appointments;
using UVACanvasAccess.Model.Users;
using UVACanvasAccess.Structures.Appointments;
using UVACanvasAccess.Structures.Calendar;
using UVACanvasAccess.Structures.Users;
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

        internal async Task<AppointmentGroup> PostCreateAppointmentGroup(AppointmentGroupBuilder builder) {
            var args = builder.Fields
                              .Select(kv => (kv.Key, kv.Value))
                              .Concat(builder.ArrayFields
                                             .SelectMany(k => k, (k, v) => (k.Key, v)));
            var response = await _client.PostAsync("appointment_groups", BuildMultipartHttpArguments(args));
            response.AssertSuccess();

            var model = JsonConvert.DeserializeObject<AppointmentGroupModel>(await response.Content.ReadAsStringAsync());
            return new AppointmentGroup(this, model);
        }

        internal async Task<AppointmentGroup> PutUpdateAppointmentsGroup(AppointmentGroupBuilder builder) {
            var args = builder.Fields
                              .Select(kv => (kv.Key, kv.Value))
                              .Concat(builder.ArrayFields
                                             .SelectMany(k => k, (k, v) => (k.Key, v)));
            var response = await _client.PutAsync($"appointment_groups/{builder.EditingId}", BuildMultipartHttpArguments(args));
            response.AssertSuccess();

            var model = JsonConvert.DeserializeObject<AppointmentGroupModel>(await response.Content.ReadAsStringAsync());
            return new AppointmentGroup(this, model);
        }

        public AppointmentGroupBuilder CreateAppointmentGroup(string title, IEnumerable<EventContext> contexts) {
            return new AppointmentGroupBuilder(this, title, contexts);
        }
        
        public AppointmentGroupBuilder CreateAppointmentGroup(string title, params EventContext[] contexts) {
            return CreateAppointmentGroup(title, (IEnumerable<EventContext>) contexts);
        }

        public AppointmentGroupBuilder EditAppointmentGroup(AppointmentGroup appointmentGroup) {
            if (appointmentGroup.ContextCodes == null || !appointmentGroup.ContextCodes.Any()) {
                Logger.Warn("Editing an appointment group with no context codes and without supplying any. The operation " +
                            "might fail.");
            }
            return new AppointmentGroupBuilder(this, appointmentGroup.Id, appointmentGroup.ContextCodes);
        }

        public AppointmentGroupBuilder EditAppointmentGroup(AppointmentGroup appointmentGroup, IEnumerable<EventContext> contexts) {
            return new AppointmentGroupBuilder(this, appointmentGroup.Id, contexts);
        }
        
        public AppointmentGroupBuilder EditAppointmentGroup(AppointmentGroup appointmentGroup, params EventContext[] contexts) {
            return EditAppointmentGroup(appointmentGroup, (IEnumerable<EventContext>) contexts);
        }

        /// <summary>
        /// Gets a single appointment group.
        /// </summary>
        /// <param name="id">The id of the group.</param>
        /// <param name="includes">Optional data to include in the result.</param>
        /// <returns>The appointment group.</returns>
        public async Task<AppointmentGroup> GetAppointmentGroup(ulong id, AppointmentGroupIncludes includes = 0) {
            var response = await _client.GetAsync($"appointment_groups/{id}" +
                                                  BuildDuplicateKeyQueryString(includes.GetFlagsApiRepresentations()
                                                                                       .Select(f => ("include[]", f))
                                                                                       .ToArray()));
            var model = JsonConvert.DeserializeObject<AppointmentGroupModel>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return new AppointmentGroup(this, model);
        }
        
        /// <summary>
        /// Deletes an appointment group.
        /// </summary>
        /// <param name="id">The id of the group.</param>
        /// <param name="reason">An optional reason.</param>
        /// <returns>The deleted appointment group.</returns>
        public async Task<AppointmentGroup> DeleteAppointmentGroup(ulong id, string reason = null) {
            var response = await _client.DeleteAsync($"appointment_groups/{id}" + (reason == null ? "" 
                                                                                                           : $"?reason={reason}"));
            var model = JsonConvert.DeserializeObject<AppointmentGroupModel>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return new AppointmentGroup(this, model);
        }

        /// <summary>
        /// Streams all registered participants in the assignment group.
        /// </summary>
        /// <param name="appointmentGroupId">The id of the appointment group.</param>
        /// <typeparam name="T">The participant type. Either <see cref="User"/> or Group.</typeparam>
        /// <returns>The stream of participants.</returns>
        /// <exception cref="NotImplementedException">fixme</exception>
        public async IAsyncEnumerable<T> StreamAppointmentGroupParticipants<T>(ulong appointmentGroupId)
        where T: IAppointmentGroupParticipant, IPrettyPrint {

            var isUser = typeof(T).IsAssignableFrom(typeof(User));

            if (!isUser) {
                throw new NotImplementedException("group in StreamAppointmentGroupParticipants");
            }
            
            var response = await _client.GetAsync($"appointment_groups/{appointmentGroupId}/{(isUser ? "users" : "groups")}?registration_status=registered")
                                        .AssertSuccess();

            // todo groups
            
            await foreach (var model in StreamDeserializePages<UserModel>(response)) {
                yield return (T) (IAppointmentGroupParticipant) new User(this, model); // c# has a good type checker
            }
        }
    }
}
