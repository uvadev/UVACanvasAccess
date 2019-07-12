using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Accounts;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Accounts {
    
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
    // ReSharper disable MemberCanBePrivate.Global
    public class Account : IPrettyPrint {
        private readonly Api _api;
        
        public ulong Id { get; }
        
        public string Name { get; }
        
        public string Uuid { get; }
        
        public ulong? ParentAccountId { get; }
        
        public ulong? RootAccountId { get; }
        
        public ulong? DefaultUserStorageQuotaMb { get; }
        
        public ulong? DefaultGroupStorageQuotaMb { get; }
        
        public string DefaultTimeZone { get; }
        
        [CanBeNull]
        public string SisAccountId { get; }
        
        [CanBeNull]
        public string IntegrationId { get; }
        
        [CanBeNull]
        public string SisImportId { get; }
        
        public string LtiGuid { get; }
        
        public string WorkflowState { get; }

        public Account(Api api, AccountModel model) {
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