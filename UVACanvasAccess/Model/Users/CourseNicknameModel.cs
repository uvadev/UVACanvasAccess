using Newtonsoft.Json;

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

        public override string ToString() {
            return $"{nameof(CourseId)}: {CourseId}," +
                   $"\n{nameof(Name)}: {Name}," +
                   $"\n{nameof(Nickname)}: {Nickname}";
        }
    }
}