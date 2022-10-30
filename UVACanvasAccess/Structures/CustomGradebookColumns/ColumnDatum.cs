using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.CustomGradebookColumns;

namespace UVACanvasAccess.Structures.CustomGradebookColumns {
    
    /// <summary>
    /// Represents an entry for a user in a custom gradebook column.
    /// </summary>
    [PublicAPI]
    public class ColumnDatum {
        private readonly Api api;
        
        /// <summary>
        /// The content of the entry.
        /// </summary>
        public string Content { get; }
        
        /// <summary>
        /// The user associated with the entry.
        /// </summary>
        public ulong UserId { get; }

        internal ColumnDatum(Api api, ColumnDatumModel model) {
            this.api = api;
            Content = model.Content;
            UserId = model.UserId;
        }
    }
}
