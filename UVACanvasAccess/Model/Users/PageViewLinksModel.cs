using Newtonsoft.Json;
using StatePrinting;

namespace UVACanvasAccess.Model.Users {
    internal class PageViewLinksModel {
        
        [JsonProperty("user")]
        public ulong User { get; set; }
        
        [JsonProperty("context")]
        public ulong? Context { get; set; }
        
        [JsonProperty("asset")]
        public ulong? Asset { get; set; }
        
        [JsonProperty("real_user")]
        public ulong? RealUser { get; set; }
        
        [JsonProperty("account")]
        public ulong Account { get; set; }

        private static readonly Stateprinter Printer = new Stateprinter();
        public override string ToString() {
            return Printer.PrintObject(this);
        }
    }
}