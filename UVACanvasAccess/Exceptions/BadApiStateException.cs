using System;
using JetBrains.Annotations;

namespace UVACanvasAccess.Exceptions {
    
    /// <summary>
    /// Indicates that an operation failed due to bad state, or because completing it would result in a bad state.
    /// </summary>
    [PublicAPI]
    public class BadApiStateException : Exception {
        internal BadApiStateException() { }

        internal BadApiStateException(string message) : base(message) { }

        internal BadApiStateException(string message, Exception innerException) : base(message, innerException) { }
    }
}
