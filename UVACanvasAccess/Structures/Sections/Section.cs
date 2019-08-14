using System;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Sections;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Sections {
    
    [PublicAPI]
    public class Section : IPrettyPrint {
        private readonly Api _api;
        
        public ulong Id { get; }
        
        public string Name { get; }

        [CanBeNull]
        public string SisSectionId { get; }

        [CanBeNull]
        public string IntegrationId { get; }
        
        public ulong? SisImportId { get; }

        public DateTime? StartAt { get; }

        public DateTime? EndAt { get; }
        
        public bool? RestrictEnrollmentsToSectionDates { get; }

        public ulong? NonCrossListedCourseId { get; }

        [OptIn]
        public uint? TotalStudents { get; }

        internal Section(Api api, SectionModel model) {
            _api = api;
            Id = model.Id;
            Name = model.Name;
            SisSectionId = model.SisSectionId;
            IntegrationId = model.IntegrationId;
            SisImportId = model.SisImportId;
            StartAt = model.StartAt;
            EndAt = model.EndAt;
            RestrictEnrollmentsToSectionDates = model.RestrictEnrollmentsToSectionDates;
            NonCrossListedCourseId = model.NonCrossListedCourseId;
            TotalStudents = model.TotalStudents;
        }

        public string ToPrettyString() {
            return "Section {" +
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(Name)}: {Name}," +
                   $"\n{nameof(SisSectionId)}: {SisSectionId}," +
                   $"\n{nameof(IntegrationId)}: {IntegrationId}," +
                   $"\n{nameof(SisImportId)}: {SisImportId}," +
                   $"\n{nameof(StartAt)}: {StartAt}," +
                   $"\n{nameof(EndAt)}: {EndAt}," +
                   $"\n{nameof(RestrictEnrollmentsToSectionDates)}: {RestrictEnrollmentsToSectionDates}," +
                   $"\n{nameof(NonCrossListedCourseId)}: {NonCrossListedCourseId}," +
                   $"\n{nameof(TotalStudents)}: {TotalStudents}").Indent(4) + 
                   "\n}";
        }
    }
}
