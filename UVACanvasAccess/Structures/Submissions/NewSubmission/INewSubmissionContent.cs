using System.Collections.Generic;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Submissions.NewSubmission {
    
    public enum SubmissionType {
        [ApiRepresentation("online_text_entry")]
        OnlineTextEntry,
        [ApiRepresentation("online_url")]
        OnlineUrl,
        [ApiRepresentation("online_upload")]
        OnlineUpload,
        [ApiRepresentation("media_recording")]
        MediaRecording,
        [ApiRepresentation("basic_lti_launch")]
        BasicLtiLaunch
    }
    
    public interface INewSubmissionContent {
        SubmissionType Type { get; }
        
        IEnumerable<(string, string)> GetTuples();
    }
}