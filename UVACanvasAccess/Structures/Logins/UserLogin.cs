using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Logins;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Logins {
    
    /// <summary>
    /// Represents a user login, i.e., an identity used to authenticate with Canvas.
    /// </summary>
    [PublicAPI]
    public class UserLogin : IPrettyPrint {
        private readonly Api api;
        
        /// <summary>
        /// The id of the account which the login belongs to.
        /// </summary>
        public ulong AccountId { get; }

        /// <summary>
        /// The login id.
        /// </summary>
        public ulong Id { get; }

        /// <summary>
        /// The login SIS id.
        /// </summary>
        /// <remarks>
        /// This should correspond to the relevant user's SIS id.
        /// </remarks>
        public string SisUserId { get; }
        
        /// <summary>
        /// The integration id.
        /// </summary>
        public string IntegrationId { get; }
        
        /// <summary>
        /// The unique identifier, e.g. the user's email or username.
        /// </summary>
        public string UniqueId { get; }

        /// <summary>
        /// The id of the user which this login belongs to.
        /// </summary>
        public ulong UserId { get; }
        
        /// <summary>
        /// The authentication provider id, if any.
        /// </summary>
        public ulong? AuthenticationProviderId { get; }
        
        /// <summary>
        /// The authentication provider type, if any.
        /// </summary>
        public string AuthenticationProviderType { get; }
        
        /// <summary>
        /// The workflow state.
        /// </summary>
        public string WorkflowState { get; }

        /// <summary>
        /// The declared user type, if any.
        /// </summary>
        public string DeclaredUserType { get; }

        internal UserLogin(Api api, UserLoginModel model) {
            this.api = api;
            AccountId = model.AccountId;
            Id = model.Id;
            SisUserId = model.SisUserId;
            IntegrationId = model.IntegrationId;
            UniqueId = model.UniqueId;
            UserId = model.UserId;
            AuthenticationProviderId = model.AuthenticationProviderId;
            AuthenticationProviderType = model.AuthenticationProviderType;
            WorkflowState = model.WorkflowState;
            DeclaredUserType = model.DeclaredUserType;
        }

        /// <inheritdoc />
        public string ToPrettyString() {
            return "UserLogin {" + 
                   ($"\n{nameof(AccountId)}: {AccountId}," +
                   $"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(SisUserId)}: {SisUserId}," +
                   $"\n{nameof(IntegrationId)}: {IntegrationId}," +
                   $"\n{nameof(UniqueId)}: {UniqueId}," +
                   $"\n{nameof(UserId)}: {UserId}," +
                   $"\n{nameof(AuthenticationProviderId)}: {AuthenticationProviderId}," +
                   $"\n{nameof(AuthenticationProviderType)}: {AuthenticationProviderType}," +
                   $"\n{nameof(WorkflowState)}: {WorkflowState}," +
                   $"\n{nameof(DeclaredUserType)}: {DeclaredUserType}").Indent(4) +
                   "\n}";
        }
    }
}
