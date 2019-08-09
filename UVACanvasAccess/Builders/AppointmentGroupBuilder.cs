using System;
using System.Collections.Generic;
using System.Linq;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Structures.Calendar;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Builders {

    public class AppointmentGroupBuilder {
        private readonly Api _api;
        private readonly bool _isEditing;
        private uint dateN = 0;
        
        internal Dictionary<string, string> Fields { get; } = new Dictionary<string, string>();

        private readonly List<KeyValuePair<string, string>> _arrayFields = new List<KeyValuePair<string, string>>();

        internal ILookup<string, string> ArrayFields => _arrayFields.Distinct()
                                                                    .ToLookup(kv => kv.Key,
                                                                              kv => kv.Value);

        internal AppointmentGroupBuilder(string title, IEnumerable<EventContext> contexts) {
            Put("title", title);
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
            PutArr2("new_appointments", dateN.ToString(), start.ToIso8601Date());
            PutArr2("new_appointments", dateN.ToString(), end.ToIso8601Date());
            dateN++;
            return this;
        }

        public AppointmentGroupBuilder WithProtectedVisibility(bool @protected = true) {
            return Put("participant_visibility", @protected ? "protected" : "private");
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
