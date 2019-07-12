using System;

namespace UVACanvasAccess.Exceptions {
    public class DoesNotExistException : Exception {
        public DoesNotExistException() { }

        public DoesNotExistException(string message) : base(message) { }

        public DoesNotExistException(string message, Exception innerException) : base(message, innerException) { }
    }
}