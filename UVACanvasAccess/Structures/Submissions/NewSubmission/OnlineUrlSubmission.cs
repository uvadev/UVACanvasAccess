using System.Collections.Generic;
using UVACanvasAccess.Util;

// ReSharper disable MemberCanBePrivate.Global
namespace UVACanvasAccess.Structures.Submissions.NewSubmission {
    
    /// <summary>
    /// Represents the submission of a URL.
    /// </summary>
    public class OnlineUrlSubmission : INewSubmissionContent {
        public ApiSubmissionType Type { get; }
        public string Url { get; }
        
        public OnlineUrlSubmission(string url) {
            Type = ApiSubmissionType.OnlineUrl;
            Url = url;
        }
        
        public IEnumerable<(string, string)> GetTuples() {
            return ("submission[body]", Url).Yield();
        }
    }
}