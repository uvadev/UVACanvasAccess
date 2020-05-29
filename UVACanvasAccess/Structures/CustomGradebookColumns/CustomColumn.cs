using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.CustomGradebookColumns;

namespace UVACanvasAccess.Structures.CustomGradebookColumns {
    
    [PublicAPI]
    public class CustomColumn {
        private readonly Api _api;
        
        public ulong Id { get; }
        
        public bool? TeacherNotes { get; }
        
        public string Title { get; }
        
        public int? Position { get; }
        
        public bool? Hidden { get; }
        
        public bool? ReadOnly { get; }

        internal CustomColumn(Api api, CustomColumnModel model) {
            _api = api;
            Id = model.Id;
            TeacherNotes = model.TeacherNotes;
            Title = model.Title;
            Position = model.Position;
            Hidden = model.Hidden;
            ReadOnly = model.ReadOnly;
        }
    }
}
