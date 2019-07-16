using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Gradebook {
    
    internal class SubmissionHistoryModel {
        
        [JsonProperty("submission_id")]
        public ulong SubmissionId { get; set; }
        
        [CanBeNull]
        [JsonProperty("versions")]
        public IEnumerable<SubmissionVersionModel> Versions { get; set; }
    }
}