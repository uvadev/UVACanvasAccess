using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Users {
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class PageViewLinksModel {
        
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

        public override string ToString() {
            return $"{nameof(User)}: {User}," +
                   $"\n{nameof(Context)}: {Context}," +
                   $"\n{nameof(Asset)}: {Asset}," +
                   $"\n{nameof(RealUser)}: {RealUser}," +
                   $"\n{nameof(Account)}: {Account}";
        }
    }
}