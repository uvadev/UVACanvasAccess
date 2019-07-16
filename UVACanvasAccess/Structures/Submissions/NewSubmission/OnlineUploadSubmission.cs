using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace UVACanvasAccess.Structures.Submissions.NewSubmission {
    
    /// <summary>
    /// Represents the submission of one or more previously uploaded files.
    /// </summary>
    [PublicAPI]
    public class OnlineUploadSubmission : INewSubmissionContent {
        public ApiSubmissionType Type { get; }
        
        public IEnumerable<ulong> FileIds { get; }
        
        public OnlineUploadSubmission(IEnumerable<ulong> fileIds) {
            Type = ApiSubmissionType.OnlineUpload;
            FileIds = fileIds;
        }

        public OnlineUploadSubmission(params ulong[] fileIds) : this(fileIds.AsEnumerable()) { }

        public IEnumerable<(string, string)> GetTuples() {
            return FileIds.Select(l => l.ToString())
                          .Select(s => ("submission[file_ids][]", s));
        }
    }
}