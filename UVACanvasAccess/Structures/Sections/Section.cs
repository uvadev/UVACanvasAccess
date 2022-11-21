using System;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Sections;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Sections {
    
    /// <summary>
    /// Represents a section in a course.
    /// </summary>
    [PublicAPI]
    public class Section : IPrettyPrint {
        private readonly Api api;
        
        /// <summary>
        /// The section id.
        /// </summary>
        public ulong Id { get; }
        
        /// <summary>
        /// The section name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The section SIS id.
        /// </summary>
        [CanBeNull]
        public string SisSectionId { get; }

        /// <summary>
        /// The section integration id.
        /// </summary>
        [CanBeNull]
        public string IntegrationId { get; }
        
        /// <summary>
        /// The section SIS import id.
        /// </summary>
        public ulong? SisImportId { get; }
        
        /// <summary>
        /// The id of the course this section belongs to.
        /// </summary>
        public ulong? CourseId { get; }
        
        /// <summary>
        /// The SIS id of the course this section belongs to.
        /// </summary>
        [CanBeNull] 
        public string SisCourseId { get; }

        /// <summary>
        /// The section start date.
        /// </summary>
        public DateTime? StartAt { get; }

        /// <summary>
        /// The section end date.
        /// </summary>
        public DateTime? EndAt { get; }
        
        /// <summary>
        /// Prevent enrollments outside of the section start and end dates.
        /// </summary>
        public bool? RestrictEnrollmentsToSectionDates { get; }

        /// <summary>
        /// If this is a cross-listed section, the id of the original course.
        /// </summary>
        public ulong? NonCrossListedCourseId { get; }

        /// <summary>
        /// The number of active and invited students in this section.
        /// </summary>
        [OptIn]
        public uint? TotalStudents { get; }

        internal Section(Api api, SectionModel model) {
            this.api = api;
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
            CourseId = model.CourseId;
            SisCourseId = model.SisCourseId;
        }

        /// <inheritdoc />
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
