using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Users;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Users {
    
    [PublicAPI]
    public class UserDisplay : IPrettyPrint {
        private readonly Api _api;
        
        public ulong Id { get; }
        
        public string ShortName { get; }
        
        public string DisplayName { get; }
        
        public string AvatarImageUrl { get; }
        
        public string HtmlUrl { get; }
        
        public string Pronouns { get; }
        
        public string AnonymousId { get; }

        internal UserDisplay(Api api, UserDisplayModel model) {
            _api = api;
            Id = model.Id ?? 0;
            ShortName = model.ShortName;
            AvatarImageUrl = model.AvatarImageUrl;
            HtmlUrl = model.HtmlUrl;
            DisplayName = model.DisplayName;
            Pronouns = model.Pronouns;
            AnonymousId = model.AnonymousId;
        }

        public string ToPrettyString() {
            return "UserDisplay {" +
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(ShortName)}: {ShortName}," +
                   $"\n{nameof(DisplayName)}: {DisplayName}," +
                   $"\n{nameof(AvatarImageUrl)}: {AvatarImageUrl}," +
                   $"\n{nameof(HtmlUrl)}: {HtmlUrl}," +
                   $"\n{nameof(AnonymousId)}: {AnonymousId}," +
                   $"\n{nameof(Pronouns)}: {Pronouns},").Indent(4) + 
                   "\n}";
        }
    }
}