using Newtonsoft.Json;
using StatePrinting;

namespace UVACanvasAccess.Model.Users {
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AnonymousUserDisplayModel {
        
        [JsonProperty("anonymous_id")]
        public string AnonymousId { get; set; }
        
        [JsonProperty("avatar_image_url")]
        public string AvatarImageUrl { get; set; }

        private static readonly Stateprinter Printer = new Stateprinter();
        public override string ToString() {
            return Printer.PrintObject(this);
        }
    }
}