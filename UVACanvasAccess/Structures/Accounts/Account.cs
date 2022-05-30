using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Accounts;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Accounts {
    
    /// <summary>
    /// Represents a Canvas account.
    /// </summary>
    /// <remarks>Distinct from a <see cref="UVACanvasAccess.Structures.Users.User">user account</see>.</remarks>
    [PublicAPI]
    public class Account : IPrettyPrint {
        private readonly Api _api;
        
        /// <summary>
        /// The account id.
        /// </summary>
        public ulong Id { get; }
        
        /// <summary>
        /// The account's display name.
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// The account's UUID.
        /// </summary>
        public string Uuid { get; }
        
        /// <summary>
        /// The parent account id, if it exists.
        /// </summary>
        public ulong? ParentAccountId { get; }
        
        /// <summary>
        /// The root account id, unless this is the root account.
        /// </summary>
        public ulong? RootAccountId { get; }
        
        /// <summary>
        /// The default storage quota for users under this account, in megabytes.
        /// </summary>
        public ulong? DefaultUserStorageQuotaMb { get; }
        
        /// <summary>
        /// The default storage quota for groups under this account, in megabytes.
        /// </summary>
        public ulong? DefaultGroupStorageQuotaMb { get; }
        
        /// <summary>
        /// The default time zone for this account.
        /// 
        /// This may be in either <a href="http://www.iana.org/time-zones">IANA format</a> or
        /// <a href="http://api.rubyonrails.org/classes/ActiveSupport/TimeZone.html">Ruby on Rails format</a>.
        /// </summary>
        public string DefaultTimeZone { get; }
        
        /// <summary>
        /// The account's SIS identifier.
        /// </summary>
        [CanBeNull]
        public string SisAccountId { get; }
        
        /// <summary>
        /// The account's integration identifier.
        /// </summary>
        [CanBeNull]
        public string IntegrationId { get; }
        
        /// <summary>
        /// The account's SIS import id, if this account was created by SIS.
        /// </summary>
        [CanBeNull]
        public string SisImportId { get; }
        
        /// <summary>
        /// The account's LTI context identifier.
        /// </summary>
        public string LtiGuid { get; }
        
        /// <summary>
        /// The state of the account. Either <c>active</c> or <c>deleted</c>.
        /// </summary>
        public string WorkflowState { get; } // todo Should this be an enum?

        internal Account(Api api, AccountModel model) {
            _api = api;
            Id = model.Id;
            Name = model.Name;
            Uuid = model.Uuid;
            ParentAccountId = model.ParentAccountId;
            RootAccountId = model.RootAccountId;
            DefaultUserStorageQuotaMb = model.DefaultUserStorageQuotaMb;
            DefaultGroupStorageQuotaMb = model.DefaultGroupStorageQuotaMb;
            DefaultTimeZone = model.DefaultTimeZone;
            SisAccountId = model.SisAccountId;
            IntegrationId = model.IntegrationId;
            SisImportId = model.SisImportId;
            LtiGuid = model.LtiGuid;
            WorkflowState = model.WorkflowState;
        }

        /// <inheritdoc />
        public string ToPrettyString() {
            return "Account {" + 
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(Name)}: {Name}," +
                   $"\n{nameof(Uuid)}: {Uuid}," +
                   $"\n{nameof(ParentAccountId)}: {ParentAccountId}," +
                   $"\n{nameof(RootAccountId)}: {RootAccountId}," +
                   $"\n{nameof(DefaultUserStorageQuotaMb)}: {DefaultUserStorageQuotaMb}," +
                   $"\n{nameof(DefaultGroupStorageQuotaMb)}: {DefaultGroupStorageQuotaMb}," +
                   $"\n{nameof(DefaultTimeZone)}: {DefaultTimeZone}," +
                   $"\n{nameof(SisAccountId)}: {SisAccountId}," +
                   $"\n{nameof(IntegrationId)}: {IntegrationId}," +
                   $"\n{nameof(SisImportId)}: {SisImportId}," +
                   $"\n{nameof(LtiGuid)}: {LtiGuid}," +
                   $"\n{nameof(WorkflowState)}: {WorkflowState}").Indent(4) + 
                   "\n}";
        }
    }
}