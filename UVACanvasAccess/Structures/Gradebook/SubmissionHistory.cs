using System.Collections.Generic;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Gradebook;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Gradebook {
    
    [PublicAPI]
    public class SubmissionHistory : IPrettyPrint {
        private readonly Api _api;
        
        public ulong SubmissionId { get; }
        
        [NotNull]
        public IEnumerable<SubmissionVersion> Versions { get; }

        internal SubmissionHistory(Api api, SubmissionHistoryModel model) {
            _api = api;
            SubmissionId = model.SubmissionId;
            Versions = model.Versions.SelectNotNull(m => new SubmissionVersion(api, m));
        }

        public string ToPrettyString() {
            return "SubmissionHistory {" +
                   ($"\n{nameof(SubmissionId)}: {SubmissionId}," +
                   $"\n{nameof(Versions)}: {Versions.ToPrettyString()}").Indent(4) + 
                   "\n}";
        }
    }
}