using JetBrains.Annotations;
using UVACanvasAccess.Model.Analytics;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Analytics {
    
    [PublicAPI]
    public class CourseStudentSummary : IPrettyPrint {
        public ulong Id { get; }
        
        public uint PageViews { get; }
        
        public uint? MaxPageViews { get; }
        
        public uint? PageViewsLevel { get; }
        
        public uint Participations { get; }
        
        public uint? MaxParticipations { get; }
        
        public uint? ParticipationsLevel { get; }
        
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
