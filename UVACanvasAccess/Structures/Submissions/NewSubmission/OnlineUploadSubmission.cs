using System.Collections.Generic;
using System.Linq;

// ReSharper disable MemberCanBePrivate.Global
namespace UVACanvasAccess.Structures.Submissions.NewSubmission {
    
    /// <summary>
    /// Represents the submission of one or more previously uploaded files.
    /// </summary>
    public class OnlineUploadSubmission : INewSubmissionContent {
        public SubmissionType Type { get; }
        
        public IEnumerable<ulong> FileIds { get; }
        
        public OnlineUploadSubmission(IEnumerable<ulong> fileIds) {
            Type = SubmissionType.OnlineUpload;
            FileIds = fileIds;
        }

        public OnlineUploadSubmission(params ulong[] fileIds) : this(fileIds.AsEnumerable()) { }

        public IEnumerable<(string, string)> GetTuples() {
            return FileIds.Select(l => l.ToString())
                          .Select(s => ("submission[file_ids][]", s));
        }
    }
}