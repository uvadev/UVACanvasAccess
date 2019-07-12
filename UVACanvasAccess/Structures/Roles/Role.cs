using System;
using System.Collections.Generic;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Roles;
using UVACanvasAccess.Structures.Accounts;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Roles {
    
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
    // ReSharper disable MemberCanBePrivate.Global
    public class Role : IPrettyPrint {
        private readonly Api _api;

        public string Label { get; }

        public string BaseRoleType { get; }

        public Account Account { get; }

        public string WorkflowState { get; }

        public Dictionary<string, RolePermissions> Permissions { get; }

        public Role(Api api, RoleModel model) {
            _api = api;
            Label = model.Label;
            BaseRoleType = model.BaseRoleType;
            Account = model.Account.ConvertIfNotNull(m => new Account(api, m));
            WorkflowState = model.WorkflowState;
            Permissions = model.Permissions.ValSelect(m => new RolePermissions(api, m));
        }

        public string ToPrettyString() {
            return "Role {" +
                   ($"\n{nameof(Label)}: {Label}," +
                   $"\n{nameof(BaseRoleType)}: {BaseRoleType}," +
                   $"\n{nameof(Account)}: {Account}," +
                   $"\n{nameof(WorkflowState)}: {WorkflowState}," +
                   $"\n{nameof(Permissions)}: {Permissions.ToPrettyString()}").Indent(4) + 
                   "\n}";
        }
    }

    [Flags]
    public enum RoleState : byte {
        [ApiRepresentation("active")]
        Active = 1 << 0,
        [ApiRepresentation("inactive")]
        Inactive = 1 << 1
    }
}