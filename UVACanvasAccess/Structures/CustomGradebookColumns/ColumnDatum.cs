using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.CustomGradebookColumns;

namespace UVACanvasAccess.Structures.CustomGradebookColumns {
    
    [PublicAPI]
    public class ColumnDatum {
        private readonly Api _api;
        
        public string Content { get; }
        
        public ulong UserId { get; }

        internal ColumnDatum(Api api, ColumnDatumModel model) {
            _api = api;
            Content = model.Content;
            UserId = model.UserId;
        }
    }
}
