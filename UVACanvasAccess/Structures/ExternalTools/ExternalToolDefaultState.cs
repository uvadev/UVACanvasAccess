using JetBrains.Annotations;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.ExternalTools {
    
    [PublicAPI]
    public enum ExternalToolDefaultState {
        [ApiRepresentation("enabled")]
        Enabled,
        [ApiRepresentation("disabled")]
        Disabled
    }
}
