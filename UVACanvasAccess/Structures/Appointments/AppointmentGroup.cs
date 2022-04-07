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
    
    [PublicAPI]
    public class AppointmentGroup : IPrettyPrint {
        private readonly Api _api;
        
        public ulong Id { get; }
        
        public string Title { get; }

        public DateTime StartAt { get; }

        public DateTime EndAt { get; }

        public string Description { get; }

        public string LocationName { get; }

        public string LocationAddress { get; }

        public uint? ParticipantCount { get; }

        public IEnumerable<Appointment> ReservedTimes { get; }

        public IEnumerable<EventContext> ContextCodes { get; }

        public IEnumerable<EventContext> SubContextCodes { get; }
        
        public string WorkflowState { get; }
        
        public bool? RequiringAction { get; }
        
        public uint AppointmentsCount { get; }
        
        public IEnumerable<TimeSlotCalendarEvent> Appointments { get; }

        [CanBeNull]
        public IEnumerable<TimeSlotCalendarEvent> NewAppointments { get; }

        public uint? MaxAppointmentsPerParticipant { get; }

        public uint? MinAppointmentsPerParticipant { get; }
        
        public uint? ParticipantsPerAppointment { get; }
        
        public string ParticipantVisibility { get; }
        
        public string ParticipantType { get; }
        
        public string Url { get; }
        
        public string HtmlUrl { get; }
        
        public DateTime CreatedAt { get; }
        
        public DateTime UpdatedAt { get; }

        public IAsyncEnumerable<User> StreamUserParticipants() {
            return _api.StreamAppointmentGroupParticipants<User>(Id);
        }
        
        public IAsyncEnumerable<Group> StreamGroupParticipants() {
            return _api.StreamAppointmentGroupParticipants<Group>(Id);
        }

        internal AppointmentGroup(Api api, AppointmentGroupModel model) {
            _api = api;
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
