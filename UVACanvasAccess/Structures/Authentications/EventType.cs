using JetBrains.Annotations;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Authentications {
    
    [PublicAPI]
    public enum EventType : byte {
        [ApiRepresentation("login")]
        Login,
        [ApiRepresentation("logout")]
        Logout
    }
}