using System;

namespace UVACanvasAccess.Exceptions {
    public class CommunicationException : Exception {
        public CommunicationException() { }
        public CommunicationException(string message) : base(message) { }
        public CommunicationException(string message, Exception innerException) : base(message, innerException) { }
    }
}