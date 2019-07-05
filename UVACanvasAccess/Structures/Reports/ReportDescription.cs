using System.Collections.Generic;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Reports;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Reports {
    
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
    // ReSharper disable MemberCanBePrivate.Global
    public class ReportDescription : IPrettyPrint {
        private readonly Api _api;
        
        public string Report { get; }
        
        public string Title { get; }

        [NotNull]
        public Dictionary<string, ReportParameterDescription> Parameters { get; }

        public ReportDescription(Api api, ReportDescriptionModel model) {
            _api = api;
            Report = model.Report;
            Title = model.Title;
            Parameters = model.Parameters?.ValSelect(m => new ReportParameterDescription(m))
                                         ?? new Dictionary<string, ReportParameterDescription>();
        }

        public string ToPrettyString() {
            return "ReportDescription {" +
                   ($"\n{nameof(Report)}: {Report}," +
                   $"\n{nameof(Title)}: {Title}," + 
                   $"\n{nameof(Parameters)}: {Parameters.ToPrettyString()}").Indent(4) +
                   "\n}";
        }
    }

    public class ReportParameterDescription : IPrettyPrint {
        public string Description { get; }
        
        public bool Required { get; }

        public ReportParameterDescription(ReportParameterDescriptionModel model) {
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