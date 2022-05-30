using JetBrains.Annotations;

using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Users;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Users {
    
    /// <summary>
    /// Represents relationships in a <see cref="PageView"/>.
    /// </summary>
    [PublicAPI]
    public class PageViewLinks : IPrettyPrint {
        private readonly Api _api;
        
        /// <summary>
        /// The id of the user responsible for the view.
        /// </summary>
        public ulong User { get; }
        
        /// <summary>
        /// The id of the context for the view. E.g. the course id, if <see cref="PageView.ContextType"/> is <c>Course</c>.
        /// </summary>
        public ulong? Context { get; }
        
        /// <summary>
        /// The id of the asset for the view, if relevant.
        /// </summary>
        public ulong? Asset { get; }
        
        /// <summary>
        /// If this request was made while masquerading, i.e. "act as", the id of the responsible user.
        /// </summary>
        public ulong? RealUser { get; }
        
        /// <summary>
        /// The account id for the context.
        /// </summary>
        public ulong? Account { get; }

        internal PageViewLinks(Api api, PageViewLinksModel model) {
            _api = api;
            User = model.User;
            Context = model.Context;
            Asset = model.Asset;
            RealUser = model.RealUser;
            Account = model.Account;
        }
        
        /// <inheritdoc /> 
        public string ToPrettyString() {
            return "PageViewLinks {" +
                   ($"\n{nameof(User)}: {User}," +
                   $"\n{nameof(Context)}: {Context}," +
                   $"\n{nameof(Asset)}: {Asset}," +
                   $"\n{nameof(RealUser)}: {RealUser}," +
                   $"\n{nameof(Account)}: {Account}").Indent(4) +
                   "\n}";
        }
    }
}