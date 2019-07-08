using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Gradebook {
    
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SubmissionHistoryModel {
        
        [JsonProperty("submission_id")]
        public ulong SubmissionId { get; set; }
        
        [CanBeNull]
        [JsonProperty("versions")]
        public IEnumerable<SubmissionVersionModel> Versions { get; set; }
    }
}