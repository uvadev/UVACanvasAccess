using System;
using JetBrains.Annotations;

namespace UVACanvasAccess.Util {

    /// <summary>
    /// Indicates that the type of a field or property in a Canvas structure is absent or unclear from official
    /// documentation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    internal sealed class UndocumentedType : Attribute {
        
        [UsedImplicitly]
        public string Comment { get; }
        
        public UndocumentedType() { }

        public UndocumentedType(string comment) {
            Comment = comment;
        }
    }
}
