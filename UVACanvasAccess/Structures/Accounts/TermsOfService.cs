using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Accounts;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Accounts {
    
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
    // ReSharper disable MemberCanBePrivate.Global
    public class TermsOfService : IPrettyPrint {
        private readonly Api _api;
        
        public ulong Id { get; }
        
        public string TermsType { get; }
        
        public bool Passive { get; }
        
        public ulong AccountId { get; }
        
        public string Content { get; }

        public TermsOfService(Api api, TermsOfServiceModel model) {
            _api = api;
            Id = model.Id;
            TermsType = model.TermsType;
            Passive = model.Passive;
            AccountId = model.AccountId;
            Content = model.Content;
        }

        public string ToPrettyString() {
            return "TermsOfService {" +
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(TermsType)}: {TermsType}," +
                   $"\n{nameof(Passive)}: {Passive}," +
                   $"\n{nameof(AccountId)}: {AccountId}," +
                   $"\n{nameof(Content)}: {Content}").Indent(4) + 
                   "\n}";
        }
    }
}