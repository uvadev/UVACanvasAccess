using System;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Submissions {
    
    [Flags]
    public enum SubmissionTypes {
        [ApiRepresentation("none")]
        None = 0,
        [ApiRepresentation("online_quiz")]
        OnlineQuiz = 1 << 0,
        [ApiRepresentation("on_paper")]
        OnPaper = 1 << 1,
        [ApiRepresentation("discussion_topic")]
        DiscussionTopic = 1 << 2,
        [ApiRepresentation("external_tool")]
        ExternalTool = 1 << 3,
        [ApiRepresentation("online_upload")]
        OnlineUpload = 1 << 4,
        [ApiRepresentation("online_text_entry")]
        OnlineTextEntry = 1 << 5,
        [ApiRepresentation("online_url")]
        OnlineUrl = 1 << 6,
        [ApiRepresentation("media_recording")]
        MediaRecording = 1 << 7
    }
}