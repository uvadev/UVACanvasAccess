using System;
using JetBrains.Annotations;

namespace UVACanvasAccess.Util {

    /// <summary>
    /// Indicates that the presence of a field or property in a Canvas structure is absent from the official
    /// documentation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    internal sealed class Undocumented : Attribute {
        
        [UsedImplicitly]
        public string Comment { get; }
        
        public Undocumented() { }

        public Undocumented(string comment) {
            Comment = comment;
        }
    }
}
