using Newtonsoft.Json;
using StatePrinting;

namespace UVACanvasAccess.Model.Users {
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CourseNicknameModel {
        
        [JsonProperty("course_id")]
        public ulong CourseId { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        private static readonly Stateprinter Printer = new Stateprinter();
        public override string ToString() {
            return Printer.PrintObject(this);
        }
    }
}