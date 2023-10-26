using System;
using JetBrains.Annotations;

namespace UVACanvasAccess.Util {

    /// <summary>
    /// Indicates that the range of possible values a field or property in a Canvas structure may contain is
    /// absent from the official documentation, despite the clear existence of such a range.
    /// </summary>
    /// <example>
    /// The `workflow_state` property often lacks a documented range despite always being an
    /// enumeration.
    /// </example>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    internal sealed class UndocumentedRange : Attribute {
        
        [UsedImplicitly]
        public string Comment { get; }
        
        public UndocumentedRange() { }

        public UndocumentedRange(string comment) {
            Comment = comment;
        }
    }
}
