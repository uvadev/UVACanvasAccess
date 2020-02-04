using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.ExternalTools {
    public enum ToolDisplayType : byte {
        [ApiRepresentation("full_width")]
        FullWidth,
        [ApiRepresentation("full_width_in_context")]
        FullWidthInContext,
        [ApiRepresentation("borderless")]
        Borderless,
        [ApiRepresentation("default")]
        Default
    }
}
