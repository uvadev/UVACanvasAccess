using System.Collections.Generic;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Reports;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Reports {
    
    [PublicAPI]
    public class ReportDescription : IPrettyPrint {
        private readonly Api _api;
        
        public string Report { get; }
        
        public string Title { get; }

        [NotNull]
        public Dictionary<string, ReportParameterDescription> Parameters { get; }
        
        [CanBeNull]
        public Report LastRun { get; }

        internal ReportDescription(Api api, ReportDescriptionModel model) {
            _api = api;
            Report = model.Report;
            Title = model.Title;
            Parameters = model.Parameters?.ValSelect(m => new ReportParameterDescription(m))
                                         ?? new Dictionary<string, ReportParameterDescription>();
            if (model.LastRun != null) {
                LastRun = new Report(api, model.LastRun);
            }
        }

        public string ToPrettyString() {
            return "ReportDescription {" +
                   ($"\n{nameof(Report)}: {Report}," +
                   $"\n{nameof(Title)}: {Title}," + 
                   $"\n{nameof(Parameters)}: {Parameters.ToPrettyString()}," +
                   $"\n{nameof(LastRun)}: {LastRun?.ToPrettyString()}").Indent(4) +
                   "\n}";
        }
    }

    [PublicAPI]
    public class ReportParameterDescription : IPrettyPrint {
        public string Description { get; }
        
        public bool Required { get; }

        internal ReportParameterDescription(ReportParameterDescriptionModel model) {
            Description = model.Description;
            Required = model.Required;
        }

        public string ToPrettyString() {
            return "ReportParameterDescription {" +
                   ($"\n{nameof(Description)}: {Description}," +
                   $"\n{nameof(Required)}: {Required}").Indent(4) +
                   "\n}";
        }
    }
}