using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Users;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Users {
    
    /// <summary>
    /// A short representation of a user.
    /// </summary>
    [PublicAPI]
    public class ShortUser : IPrettyPrint {
        private readonly Api _api;
        
        public ulong Id { get; }
        
        public string DisplayName { get; }
        
        public string AvatarImageUrl { get; }
        
        public string HtmlUrl { get; }

        internal ShortUser(Api api, ShortUserModel model) {
            _api = api;
            Id = model.Id;
            DisplayName = model.DisplayName;
            AvatarImageUrl = model.AvatarImageUrl;
            HtmlUrl = model.HtmlUrl;
        }

        public string ToPrettyString() {
            return "ShortUser {" + 
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(DisplayName)}: {DisplayName}," +
                   $"\n{nameof(AvatarImageUrl)}: {AvatarImageUrl}," +
                   $"\n{nameof(HtmlUrl)}: {HtmlUrl}").Indent(4) + 
                   "\n}";
        }
    }
}
