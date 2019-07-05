using System.Collections.Generic;

namespace UVACanvasAccess.Util {
    
    internal static class KeyValuePair {
        internal static KeyValuePair<TK, TV> New<TK,TV>(TK k, TV v) => new KeyValuePair<TK, TV>(k, v);
    }
}