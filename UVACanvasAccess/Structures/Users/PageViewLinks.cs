using StatePrinting;
using UVACanvasAccess.Model.Users;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Users {
    
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
    // ReSharper disable MemberCanBePrivate.Global
    public class PageViewLinks : IPrettyPrint {
        private readonly Api _api;
        
        public ulong User { get; private set; }
        
        public ulong? Context { get; private set; }
        
        public ulong? Asset { get; private set; }
        
        public ulong? RealUser { get; private set; }
        
        public ulong Account { get; private set; }

        public PageViewLinks(Api api, PageViewLinksModel model) {
            _api = api;
            User = model.User;
            Context = model.Context;
            Asset = model.Asset;
            RealUser = model.RealUser;
            Account = model.Account;
        }
        
        private static readonly Stateprinter Printer = new Stateprinter();
        public override string ToString() {
            return Printer.PrintObject(this);
        }

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