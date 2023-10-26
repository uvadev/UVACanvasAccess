using JetBrains.Annotations;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.ExternalTools {
    
    [PublicAPI]
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
