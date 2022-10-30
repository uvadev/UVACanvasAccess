using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.CustomGradebookColumns;

namespace UVACanvasAccess.Structures.CustomGradebookColumns {
    
    /// <summary>
    /// Represents a custom gradebook column.
    /// </summary>
    [PublicAPI]
    public class CustomColumn {
        private readonly Api api;
        
        /// <summary>
        /// The gradebook column id.
        /// </summary>
        public ulong Id { get; }
        
        /// <summary>
        /// Whether this column is considered to be a 'notes' column.
        /// </summary>
        public bool? TeacherNotes { get; }
        
        /// <summary>
        /// The column title.
        /// </summary>
        public string Title { get; }
        
        /// <summary>
        /// The position of this column relative to other columns.
        /// </summary>
        public int? Position { get; }
        
        /// <summary>
        /// Whether the column is hidden.
        /// </summary>
        public bool? Hidden { get; }
        
        /// <summary>
        /// Whether the column is non-editable from the webpage.
        /// </summary>
        public bool? ReadOnly { get; }

        internal CustomColumn(Api api, CustomColumnModel model) {
            this.api = api;
            Id = model.Id;
            TeacherNotes = model.TeacherNotes;
            Title = model.Title;
            Position = model.Position;
            Hidden = model.Hidden;
            ReadOnly = model.ReadOnly;
        }
    }
}
