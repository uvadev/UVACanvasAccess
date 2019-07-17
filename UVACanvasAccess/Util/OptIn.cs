using System;
using JetBrains.Annotations;

namespace UVACanvasAccess.Util {
    
    /// <summary>
    /// Indicates that the annotated property is null or absent by default, and must be explicitly included by an
    /// include parameter.
    /// </summary>
    [PublicAPI]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class OptIn : Attribute { }
}