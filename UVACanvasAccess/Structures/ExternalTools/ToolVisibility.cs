using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.ExternalTools {
    public enum ToolVisibility : byte {
        [ApiRepresentation("admins")]
        Admins,
        [ApiRepresentation("members")]
        Members,
        [ApiRepresentation("public")]
        Public
    }
}
