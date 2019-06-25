using StatePrinting;
using UVACanvasAccess.Model.Users;

namespace UVACanvasAccess.Structures.Users {
    
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
    // ReSharper disable MemberCanBePrivate.Global
    public struct ActivityStreamSummaryEntry {
        public string Type;
        public uint UnreadCount;
        public uint Count;

        public ActivityStreamSummaryEntry(ActivityStreamSummaryEntryModel model) {
            Type = model.Type;
            UnreadCount = model.UnreadCount;
            Count = model.Count;
        }
        
        private static readonly Stateprinter Printer = new Stateprinter();
        public override string ToString() {
            return Printer.PrintObject(this);
        }
    }
}