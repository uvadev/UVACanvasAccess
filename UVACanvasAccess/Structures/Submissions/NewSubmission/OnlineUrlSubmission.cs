using System.Collections.Generic;
using UVACanvasAccess.Util;

// ReSharper disable MemberCanBePrivate.Global
namespace UVACanvasAccess.Structures.Submissions.NewSubmission {
    
    public class OnlineUrlSubmission : INewSubmissionContent {
        public SubmissionType Type { get; }
        public string Url { get; }
        
        public OnlineUrlSubmission(string url) {
            Type = SubmissionType.OnlineUrl;
            Url = url;
        }
        
        public IEnumerable<(string, string)> GetTuples() {
            return ("submission[body]", Url).Yield();
        }
    }
}