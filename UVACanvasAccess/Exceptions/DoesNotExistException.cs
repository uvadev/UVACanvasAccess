using System;

namespace UVACanvasAccess.Exceptions {
    
    /// <summary>
    /// Indicates that some operation failed because the target thereof does not exist or is invisible to the current user.
    /// </summary>
    public class DoesNotExistException : Exception {
        public DoesNotExistException() { }

        public DoesNotExistException(string message) : base(message) { }

        public DoesNotExistException(string message, Exception innerException) : base(message, innerException) { }
    }
}