using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.EnrollmentTerms {
    internal class EnrollmentTermModel {
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("sis_term_id")]
        public string SisTermId { get; set; }
        
        [JsonProperty("sis_import_id")]
        public ulong? SisImportId { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("start_at")]
        public DateTime? StartAt { get; set; }
        
        [JsonProperty("end_at")]
        public DateTime? EndAt { get; set; }
        
        [JsonProperty("grading_period_group_id")]
        public ulong? GradingPeriodGroupId { get; set; }
        
        [JsonProperty("workflow_state")]
        public string WorkflowState { get; set; }
        
        [JsonProperty("overrides")]
        public Dictionary<string, EnrollmentTermDateOverrideModel> Overrides { get; set; }
    }

    internal struct EnrollmentTermDateOverrideModel {
        [JsonProperty("start_at")]
        public DateTime? StartAt { get; set; }
        
        [JsonProperty("end_at")]
        public DateTime? EndAt { get; set; }
    }
}
