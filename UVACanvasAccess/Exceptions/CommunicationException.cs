using System;

namespace UVACanvasAccess.Exceptions {
    
    /// <summary>
    /// Indicates a generic HTTP failure response. If the API replied with a JSON error body, it is included.
    /// </summary>
    public class CommunicationException : Exception {
        /// <inheritdoc/>
        public CommunicationException() { }
        
        /// <inheritdoc/>
        public CommunicationException(string message) : base(message) { }
        
        /// <inheritdoc/>
        public CommunicationException(string message, Exception innerException) : base(message, innerException) { }
    }
}