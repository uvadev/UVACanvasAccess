using JetBrains.Annotations;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.ExternalTools {
    
    [PublicAPI]
    public enum ExternalToolPrivacyLevel : byte {
        [ApiRepresentation("anonymous")]
        Anonymous,
        [ApiRepresentation("name_only")]
        NameOnly,
        [ApiRepresentation("email_only")]
        EmailOnly,
        [ApiRepresentation("public")]
        Public
    }
}
