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
        
        public CourseStudentTardiness TardinessBreakdown { get; }

        internal CourseStudentSummary(CourseStudentSummaryModel model) {
            Id = model.Id;
            PageViews = model.PageViews;
            MaxPageViews = model.MaxPageViews;
            PageViewsLevel = model.PageViewsLevel;
            Participations = model.Participations;
            MaxParticipations = model.MaxParticipations;
            ParticipationsLevel = model.ParticipationsLevel;
            TardinessBreakdown = model.TardinessBreakdown.ConvertIfNotNull(m => new CourseStudentTardiness(m));
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

    [PublicAPI]
    public class CourseStudentTardiness : IPrettyPrint {
        public uint Missing { get; }
        
        public uint Late { get; }
        
        public uint OnTime { get; }
        
        public uint Floating { get; }
        
        public uint Total { get; }

        internal CourseStudentTardiness(CourseStudentTardinessModel model) {
            Missing = model.Missing;
            Late = model.Late;
            OnTime = model.OnTime;
            Floating = model.Floating;
            Total = model.Total;
        }

        public string ToPrettyString() {
            return "CourseStudentTardiness {" + 
                   ($"\n{nameof(Missing)}: {Missing}," +
                   $"\n{nameof(Late)}: {Late}," +
                   $"\n{nameof(OnTime)}: {OnTime}," +
                   $"\n{nameof(Floating)}: {Floating}," +
                   $"\n{nameof(Total)}: {Total}").Indent(4) + 
                   "\n}";
        }
    }
}
