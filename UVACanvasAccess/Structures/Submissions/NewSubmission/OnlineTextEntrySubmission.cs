using System.Collections.Generic;
using UVACanvasAccess.Util;

// ReSharper disable MemberCanBePrivate.Global
namespace UVACanvasAccess.Structures.Submissions.NewSubmission {
    
    public class OnlineTextEntrySubmission : INewSubmissionContent {
        public SubmissionType Type { get; }
        public string Body { get; }

        public OnlineTextEntrySubmission(string body) {
            Type = SubmissionType.OnlineTextEntry;
            Body = body;
        }
        
        public IEnumerable<(string, string)> GetTuples() {
            return ("submission[body]", Body).Yield();
        }
    }
}