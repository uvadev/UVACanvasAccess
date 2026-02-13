using JetBrains.Annotations;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.BlueprintCourses {
    
    [PublicAPI]
    public enum BlueprintAssetType {
        [ApiRepresentation("assignment")]
        Assignment,
        [ApiRepresentation("attachment")]
        Attachment,
        [ApiRepresentation("discussion_topic")]
        DiscussionTopic,
        [ApiRepresentation("external_tool")]
        ExternalTool,
        [ApiRepresentation("quiz")]
        Quiz,
        [ApiRepresentation("wiki_page")]
        WikiPage,
        [ApiRepresentation("syllabus")]
        Syllabus,
        [ApiRepresentation("settings")]
        Settings
    }
}
