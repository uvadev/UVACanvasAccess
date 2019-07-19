using System;

namespace UVACanvasAccess.Exceptions {
    
    /// <summary>
    /// Indicates that some operation failed because the target thereof does not exist.
    /// </summary>
    public class DoesNotExistException : Exception {
        public DoesNotExistException() { }

        public DoesNotExistException(string message) : base(message) { }

        public DoesNotExistException(string message, Exception innerException) : base(message, innerException) { }
    }
}