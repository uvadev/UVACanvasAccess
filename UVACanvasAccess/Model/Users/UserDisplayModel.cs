using Newtonsoft.Json;
using StatePrinting;

namespace UVACanvasAccess.Model.Users {
    
    internal class UserDisplayModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("short_name")]
        public string ShortName { get; set; }
        
        [JsonProperty("avatar_image_url")]
        public string AvatarImageUrl { get; set; }
        
        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }

        private static readonly Stateprinter Printer = new Stateprinter();
        public override string ToString() {
            return Printer.PrintObject(this);
        }
    }
}