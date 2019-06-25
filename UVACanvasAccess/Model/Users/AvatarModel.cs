using Newtonsoft.Json;
using StatePrinting;

namespace UVACanvasAccess.Model.Users {
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AvatarModel {
        
        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("url")]
        public string Url { get; set; }
        
        [JsonProperty("token")]
        public string Token { get; set; }
        
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("content_type")]
        public string ContentType { get; set; }
        
        [JsonProperty("filename")]
        public string Filename { get; set; }
        
        [JsonProperty("size")]
        public ulong Size { get; set; }

        private static readonly Stateprinter Printer = new Stateprinter();
        public override string ToString() {
            return Printer.PrintObject(this);
        }
    }
}