using System;

namespace UVACanvasAccess.Exceptions {
    
    /// <summary>
    /// Indicates that an operation failed due to bad state, or because completing it would result in a bad state.
    /// </summary>
    public class BadApiStateException : Exception {
        public BadApiStateException() { }

        public BadApiStateException(string message) : base(message) { }

        public BadApiStateException(string message, Exception innerException) : base(message, innerException) { }
    }
}
