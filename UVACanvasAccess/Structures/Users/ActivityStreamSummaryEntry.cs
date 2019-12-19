using JetBrains.Annotations;

using UVACanvasAccess.Model.Users;

namespace UVACanvasAccess.Structures.Users {
    
    [PublicAPI]
    public struct ActivityStreamSummaryEntry {
        public string Type;
        public uint UnreadCount;
        public uint Count;

        internal ActivityStreamSummaryEntry(ActivityStreamSummaryEntryModel model) {
            Type = model.Type;
            UnreadCount = model.UnreadCount;
            Count = model.Count;
        }
        
        
    }
}