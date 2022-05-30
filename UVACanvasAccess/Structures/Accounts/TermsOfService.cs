using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Accounts;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Accounts {
    
    /// <summary>
    /// Represents the active Terms of Service for an account.
    /// </summary>
    [PublicAPI]
    public class TermsOfService : IPrettyPrint {
        private readonly Api _api;
        
        /// <summary>
        /// The id.
        /// </summary>
        public ulong Id { get; }
        
        /// <summary>
        /// The type.
        /// </summary>
        public string TermsType { get; }
        
        /// <summary>
        /// Whether or not new users must accept the Terms of Service to use Canvas.
        /// </summary>
        public bool Passive { get; }
        
        /// <summary>
        /// The account owning these terms.
        /// </summary>
        public ulong AccountId { get; }
        
        /// <summary>
        /// The content.
        /// </summary>
        public string Content { get; }

        internal TermsOfService(Api api, TermsOfServiceModel model) {
            _api = api;
            Id = model.Id;
            TermsType = model.TermsType;
            Passive = model.Passive;
            AccountId = model.AccountId;
            Content = model.Content;
        }

        /// <inheritdoc />
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