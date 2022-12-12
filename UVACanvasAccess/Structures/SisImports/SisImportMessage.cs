using System.Collections.Generic;
using JetBrains.Annotations;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.SisImports {
    
    /// <summary>
    /// Represents an error or warning in a <see cref="SisImport">SIS import</see>.
    /// </summary>
    [PublicAPI]
    public struct SisImportMessage : IPrettyPrint {
        
        /// <summary>
        /// The file the message originates from.
        /// </summary>
        public string File { get; }
        
        /// <summary>
        /// The message text.
        /// </summary>
        public string Message { get; }

        internal SisImportMessage(IEnumerable<string> pair) {
            using var e = pair.GetEnumerator();
            e.MoveNext();
            File = e.Current;
            e.MoveNext();
            Message = e.Current;
        }

        /// <inheritdoc />
        public string ToPrettyString() {
            return "SisImportMessage {" +
                   ($"\n{nameof(File)}: {File}," +
                    $"\n{nameof(Message)}: {Message}").Indent(4) +
                   "\n}";
        }
    }
}
