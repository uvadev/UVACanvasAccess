using System;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Appointments;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Appointments {
    
    /// <summary>
    /// Represents an appointment.
    /// </summary>
    [PublicAPI]
    public class Appointment : IPrettyPrint {
        private readonly Api api;
        
        /// <summary>
        /// The appointment id.
        /// </summary>
        public ulong Id { get; }
        
        /// <summary>
        /// When the appointment starts.
        /// </summary>
        public DateTime StartAt { get; }
        
        /// <summary>
        /// When the appointment ends.
        /// </summary>
        public DateTime EndAt { get; }

        internal Appointment(Api api, AppointmentModel model) {
            this.api = api;
            Id = model.Id;
            StartAt = model.StartAt;
            EndAt = model.EndAt;
        }

        /// <inheritdoc />
        public string ToPrettyString() {
            return "Appointment {" +
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(StartAt)}: {StartAt}," +
                   $"\n{nameof(EndAt)}: {EndAt}").Indent(4) + 
                   "\n}";
        }
    }
}
