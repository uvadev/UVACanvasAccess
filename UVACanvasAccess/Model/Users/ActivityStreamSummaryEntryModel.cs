using Newtonsoft.Json;
using StatePrinting;

namespace UVACanvasAccess.Model.Users {
    
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ActivityStreamSummaryEntryModel {
        
        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("unread_count")]
        public uint UnreadCount { get; set; }
        
        [JsonProperty("count")]
        public uint Count { get; set; }
        
        private static readonly Stateprinter Printer = new Stateprinter();
        public override string ToString() {
            return Printer.PrintObject(this);
        }
    }
}