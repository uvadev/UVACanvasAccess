using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Reports {
    
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable ClassNeverInstantiated.Global
    internal class ReportDescriptionModel {
        
        [JsonProperty("report")]
        public string Report { get; set; }
        
        [JsonProperty("title")]
        public string Title { get; set; }

        [CanBeNull] 
        [JsonProperty("parameters")]
        public Dictionary<string, ReportParameterDescriptionModel> Parameters { get; set; }
        
        [CanBeNull] 
        [JsonProperty("last_run")]
        public ReportModel LastRun { get; set; }
    }

    internal class ReportParameterDescriptionModel {
        
        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("required")]
        public bool Required { get; set; }
    }
}