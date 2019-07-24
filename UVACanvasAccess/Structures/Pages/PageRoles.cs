using System;
using JetBrains.Annotations;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Pages {
    
    [PublicAPI]
    [Flags]
    public enum PageRoles : byte {
        [ApiRepresentation("teachers")]
        Teachers = 1 << 0,
        [ApiRepresentation("students")]
        Students = 1 << 1,
        [ApiRepresentation("members")]
        Members = 1 << 2,
        [ApiRepresentation("public")]
        Public = 1 << 3
    }
}