using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Appointments;
using UVACanvasAccess.Structures.Calendar;
using UVACanvasAccess.Structures.Groups;
using UVACanvasAccess.Structures.Users;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Appointments {
    
    /// <summary>
    /// Represents an appointment group.
    /// </summary>
    [PublicAPI]
    public class AppointmentGroup : IPrettyPrint {
        private readonly Api api;
        
        /// <summary>
        /// The appointment group id.
        /// </summary>
        public ulong Id { get; }
        
        /// <summary>
        /// The title.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// When the appointment group begins.
        /// </summary>
        public DateTime StartAt { get; }

        /// <summary>
        /// When the appointment group ends.
        /// </summary>
        public DateTime EndAt { get; }

        /// <summary>
        /// The description.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// The location at which the appointment group takes place.
        /// </summary>
        public string LocationName { get; }

        /// <summary>
        /// The address of the <see cref="AppointmentGroup.LocationName">location</see>.
        /// </summary>
        public string LocationAddress { get; }

        /// <summary>
        /// The participant count.
        /// </summary>
        public uint? ParticipantCount { get; }

        /// <summary>
        /// The set of reserved <see cref="Appointment">appointments</see>.
        /// </summary>
        public IEnumerable<Appointment> ReservedTimes { get; }

        /// <summary>
        /// The set of <see cref="EventContext">event contexts</see>.
        /// </summary>
        public IEnumerable<EventContext> ContextCodes { get; }
        
        /// <summary>
        /// The set of sub-event contexts.
        /// </summary>
        public IEnumerable<EventContext> SubContextCodes { get; }
        
        /// <summary>
        /// The state of the appointment group.
        /// </summary>
        public string WorkflowState { get; }
        
        /// <summary>
        /// Whether or not the appointment group requires action from attendees.
        /// </summary>
        public bool? RequiringAction { get; }
        
        /// <summary>
        /// The amount of appointments.
        /// </summary>
        public uint AppointmentsCount { get; }
        
        /// <summary>
        /// The list of appointments, in the form of calendar events.
        /// </summary>
        public IEnumerable<TimeSlotCalendarEvent> Appointments { get; }

        /// <summary>
        /// The list of new appointments, in the form of calendar events.
        /// </summary>
        [CanBeNull]
        public IEnumerable<TimeSlotCalendarEvent> NewAppointments { get; }

        /// <summary>
        /// The max appointments per participant.
        /// </summary>
        public uint? MaxAppointmentsPerParticipant { get; }

        /// <summary>
        /// The minimum appointments per participant.
        /// </summary>
        public uint? MinAppointmentsPerParticipant { get; }
        
        /// <summary>
        /// The amount of participants per appointment.
        /// </summary>
        public uint? ParticipantsPerAppointment { get; }
        
        /// <summary>
        /// The participant visibility.
        /// </summary>
        public string ParticipantVisibility { get; }
        
        /// <summary>
        /// The participant type.
        /// </summary>
        public string ParticipantType { get; }
        
        /// <summary>
        /// The url.
        /// </summary>
        public string Url { get; }
        
        /// <summary>
        /// The url to the web interface.
        /// </summary>
        public string HtmlUrl { get; }
        
        /// <summary>
        /// The creation date.
        /// </summary>
        public DateTime CreatedAt { get; }
        
        /// <summary>
        /// The update date.
        /// </summary>
        public DateTime UpdatedAt { get; }

        /// <summary>
        /// Stream the list of participants in this appointment group.
        /// </summary>
        /// <returns>The stream of users.</returns>
        public IAsyncEnumerable<User> StreamUserParticipants() {
            return api.StreamAppointmentGroupParticipants<User>(Id);
        }
        
        /// <summary>
        /// Stream the list of group participants in this appointment group.
        /// </summary>
        /// <returns>The stream of groups.</returns>
        public IAsyncEnumerable<Group> StreamGroupParticipants() {
            return api.StreamAppointmentGroupParticipants<Group>(Id);
        }

        internal AppointmentGroup(Api api, AppointmentGroupModel model) {
            this.api = api;
            Id = model.Id;
            Title = model.Title;
            StartAt = model.StartAt;
            EndAt = model.EndAt;
            Description = model.Description;
            LocationName = model.LocationName;
            LocationAddress = model.LocationAddress;
            ParticipantCount = model.ParticipantCount;
            ReservedTimes = model.ReservedTimes.SelectNotNull(m => new Appointment(api, m));
            ContextCodes = model.ContextCodes.Select(cc => new EventContext(cc));
            SubContextCodes = model.SubContextCodes.Select(scc => new EventContext(scc));
            WorkflowState = model.WorkflowState;
            RequiringAction = model.RequiringAction;
            AppointmentsCount = model.AppointmentsCount;
            Appointments = model.Appointments.SelectNotNull(m => new TimeSlotCalendarEvent(api, m));
            NewAppointments = model.NewAppointments.SelectNotNull(m => new TimeSlotCalendarEvent(api, m));
            MaxAppointmentsPerParticipant = model.MaxAppointmentsPerParticipant;
            MinAppointmentsPerParticipant = model.MinAppointmentsPerParticipant;
            ParticipantsPerAppointment = model.ParticipantsPerAppointment;
            ParticipantVisibility = model.ParticipantVisibility;
            ParticipantType = model.ParticipantType;
            Url = model.Url;
            HtmlUrl = model.HtmlUrl;
            CreatedAt = model.CreatedAt;
            UpdatedAt = model.UpdatedAt;
        }

        /// <inheritdoc />
        public string ToPrettyString() {
            return "AppointmentGroup {" +
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(Title)}: {Title}," +
                   $"\n{nameof(StartAt)}: {StartAt}," +
                   $"\n{nameof(EndAt)}: {EndAt}," +
                   $"\n{nameof(Description)}: {Description}," +
                   $"\n{nameof(LocationName)}: {LocationName}," +
                   $"\n{nameof(LocationAddress)}: {LocationAddress}," +
                   $"\n{nameof(ParticipantCount)}: {ParticipantCount}," +
                   $"\n{nameof(ReservedTimes)}: {ReservedTimes.ToPrettyString()}," +
                   $"\n{nameof(ContextCodes)}: {ContextCodes.ToPrettyString()}," +
                   $"\n{nameof(SubContextCodes)}: {SubContextCodes.ToPrettyString()}," +
                   $"\n{nameof(WorkflowState)}: {WorkflowState}," +
                   $"\n{nameof(RequiringAction)}: {RequiringAction}," +
                   $"\n{nameof(AppointmentsCount)}: {AppointmentsCount}," +
                   $"\n{nameof(Appointments)}: {Appointments?.ToPrettyString()}," +
                   $"\n{nameof(NewAppointments)}: {NewAppointments?.ToPrettyString()}," +
                   $"\n{nameof(MaxAppointmentsPerParticipant)}: {MaxAppointmentsPerParticipant}," +
                   $"\n{nameof(MinAppointmentsPerParticipant)}: {MinAppointmentsPerParticipant}," +
                   $"\n{nameof(ParticipantsPerAppointment)}: {ParticipantsPerAppointment}," +
                   $"\n{nameof(ParticipantVisibility)}: {ParticipantVisibility}," +
                   $"\n{nameof(ParticipantType)}: {ParticipantType}," +
                   $"\n{nameof(Url)}: {Url}," +
                   $"\n{nameof(HtmlUrl)}: {HtmlUrl}," +
                   $"\n{nameof(CreatedAt)}: {CreatedAt}," +
                   $"\n{nameof(UpdatedAt)}: {UpdatedAt}").Indent(4) +
                   "\n}";
        }
    }
}
