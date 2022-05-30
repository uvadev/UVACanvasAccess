using JetBrains.Annotations;
using UVACanvasAccess.Model.Analytics;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Analytics {
    
    /// <summary>
    /// A summary of per-user participation information for a course.
    /// </summary>
    [PublicAPI]
    public class CourseStudentSummary : IPrettyPrint {
        
        /// <summary>
        /// The student id.
        /// </summary>
        public ulong Id { get; }
        
        /// <summary>
        /// The amount of total page views by the student in the course.
        /// </summary>
        public uint PageViews { get; }
        
        /// <summary>
        /// The amount of total page views by <i>any</i> student in the course.
        /// </summary>
        public uint? MaxPageViews { get; }
        
        /// <summary>
        /// The relative 'level' of page views for the student.
        /// </summary>
        public uint? PageViewsLevel { get; }
        
        /// <summary>
        /// The amount of total participations by the student in the course.
        /// </summary>
        public uint Participations { get; }
        
        /// <summary>
        /// The amount of total participations by <i>any</i> student in the course.
        /// </summary>
        public uint? MaxParticipations { get; }
        
        /// <summary>
        /// The relative 'level' of participations for the student.
        /// </summary>
        public uint? ParticipationsLevel { get; }
        
        /// <summary>
        /// A breakdown of on-time/lateness statuses for the student.
        /// </summary>
        /// <seealso cref="Tardiness"/>
        public Tardiness TardinessBreakdown { get; }

        internal CourseStudentSummary(CourseStudentSummaryModel model) {
            Id = model.Id;
            PageViews = model.PageViews;
            MaxPageViews = model.MaxPageViews;
            PageViewsLevel = model.PageViewsLevel;
            Participations = model.Participations;
            MaxParticipations = model.MaxParticipations;
            ParticipationsLevel = model.ParticipationsLevel;
            TardinessBreakdown = model.TardinessBreakdown.ConvertIfNotNull(m => new Tardiness(m));
        }

        /// <inheritdoc/>
        public string ToPrettyString() {
            return "CourseStudentSummary {" +
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(PageViews)}: {PageViews}," +
                   $"\n{nameof(MaxPageViews)}: {MaxPageViews}," +
                   $"\n{nameof(PageViewsLevel)}: {PageViewsLevel}," +
                   $"\n{nameof(Participations)}: {Participations}," +
                   $"\n{nameof(MaxParticipations)}: {MaxParticipations}," +
                   $"\n{nameof(ParticipationsLevel)}: {ParticipationsLevel}," +
                   $"\n{nameof(TardinessBreakdown)}: {TardinessBreakdown?.ToPrettyString()}").Indent(4) + 
                   "\n}";
        }
    }
}
