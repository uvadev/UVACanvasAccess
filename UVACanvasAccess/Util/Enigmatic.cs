using System;

namespace UVACanvasAccess.Util {
    
    /// <summary>
    /// Indicates that a field or property in a Canvas structure has an unclear contract, or is not present sometimes
    /// for unclear reasons. These fields should not be relied upon. They might be empty or null when data is actually present.
    /// Instead, one should call a function intended to retrieve the data this field represents.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    internal sealed class Enigmatic : Attribute { }
}
