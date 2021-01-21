using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.ExternalTools {
    public enum ToolWindowTarget : byte {
        [ApiRepresentation("_blank")]
        Blank,
        [ApiRepresentation("_self")]
        Self
    }
}
