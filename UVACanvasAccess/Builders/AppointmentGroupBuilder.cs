using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Structures.Appointments;
using UVACanvasAccess.Structures.Calendar;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Builders {

    [PublicAPI]
    public class AppointmentGroupBuilder {
        private readonly Api _api;
        private readonly bool _isEditing;
        internal readonly ulong EditingId;
        private uint _dateN;
        
        internal Dictionary<string, string> Fields { get; } = new Dictionary<string, string>();

        private readonly List<KeyValuePair<string, string>> _arrayFields = new List<KeyValuePair<string, string>>();

        internal ILookup<string, string> ArrayFields => _arrayFields.Distinct()
                                                                    .ToLookup(kv => kv.Key,
                                                                              kv => kv.Value);

        internal AppointmentGroupBuilder(Api api, string title, IEnumerable<EventContext> contexts) {
            _api = api;
            _isEditing = false;
            Put("title", title);
            foreach (var context in contexts) {
                PutArr("context_codes", context.ContextCode);
            }
        }

        internal AppointmentGroupBuilder(Api api, ulong editingId, IEnumerable<EventContext> contexts) {
            _api = api;
            _isEditing = true;
            EditingId = editingId;
            foreach (var context in contexts) {
                PutArr("context_codes", context.ContextCode);
            }
        }

        public AppointmentGroupBuilder WithSubContextCodes(IEnumerable<EventContext> subContexts) {
            foreach (var context in subContexts) {
                PutArr("sub_context_codes", context.ContextCode);
            }

            return this;
        }

        public AppointmentGroupBuilder WithDescription(string description) {
            return Put("description", description);
        }

        public AppointmentGroupBuilder WithLocationName(string location) {
            return Put("location_name", location);
        }

        public AppointmentGroupBuilder WithLocationAddress(string address) {
            return Put("location_address", address);
        }

        public AppointmentGroupBuilder Published(bool publish = true) {
            return Put("publish", publish.ToShortString());
        }

        public AppointmentGroupBuilder WithMaxParticipants(uint participants) {
            return Put("participants_per_appointment", participants.ToString());
        }

        public AppointmentGroupBuilder WithMaxAppointments(uint appointments) {
            return Put("max_appointments_per_participant", appointments.ToString());
        }
        
        public AppointmentGroupBuilder WithMinAppointments(uint appointments) {
            return Put("min_appointments_per_participant", appointments.ToString());
        }

        public AppointmentGroupBuilder AddTimeSlot(DateTime start, DateTime end) {
            PutArr2("new_appointments", _dateN.ToString(), start.ToIso8601Date());
            PutArr2("new_appointments", _dateN.ToString(), end.ToIso8601Date());
            _dateN++;
            return this;
        }

        public AppointmentGroupBuilder WithProtectedVisibility(bool @protected = true) {
            return Put("participant_visibility", @protected ? "protected" : "private");
        }

        public Task<AppointmentGroup> Post() {
            if (!_isEditing) {
                return _api.PostCreateAppointmentGroup(this);
            }
            Debug.Assert(EditingId != 0);
            return _api.PutUpdateAppointmentsGroup(this);
        }
        
        private AppointmentGroupBuilder Put(string key, string s) {
            Fields[$"appointment_group[{key}]"] = s;
            return this;
        }

        private AppointmentGroupBuilder PutArr(string key, string s) {
            _arrayFields.Add(new KeyValuePair<string, string>($"appointment_group[{key}][]", s));
            return this;
        }

        private AppointmentGroupBuilder PutArr2(string key, string key2, string s) {
            _arrayFields.Add(new KeyValuePair<string, string>($"appointment_group[{key}][{key2}][]", s));
            return this;
        }
    }
}
