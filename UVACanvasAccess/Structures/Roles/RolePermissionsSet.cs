using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Roles {
    
    // ReSharper disable MemberCanBePrivate.Global
    public readonly struct RolePermissionsSet {
        
        public GeneralRolePermissions GeneralAllowed { get; }
        public GeneralRolePermissions GeneralDenied { get; }
        public AccountRolePermissions AccountAllowed { get; }
        public AccountRolePermissions AccountDenied { get; }
        public GeneralRolePermissions GeneralLocked { get; }
        public AccountRolePermissions AccountLocked { get; }

        public RolePermissionsSet(GeneralRolePermissions generalAllowed = default,
                                  GeneralRolePermissions generalDenied = default, 
                                  AccountRolePermissions accountAllowed = default,
                                  AccountRolePermissions accountDenied = default, 
                                  GeneralRolePermissions generalLocked = default, 
                                  AccountRolePermissions accountLocked = default) {
            GeneralAllowed = generalAllowed;
            GeneralDenied = generalDenied;
            AccountAllowed = accountAllowed;
            AccountDenied = accountDenied;
            GeneralLocked = generalLocked;
            AccountLocked = accountLocked;
        }

        [Pure]
        internal IEnumerable<(string, string)> GetAsParams() {
            IEnumerable<(string, string)> allowed = GeneralAllowed.GetFlags()
                                                                  .Select(f => f.GetApiRepresentation())
                                                                  .Concat(AccountAllowed.GetFlags()
                                                                                        .Select(f => f.GetApiRepresentation()))
                                                                  .Select(a => (($"permissions[{a}][explicit]", "1"), 
                                                                                ($"permissions[{a}][enabled]",  "1")))
                                                                  .Interleave();
            IEnumerable<(string, string)> denied = GeneralDenied.GetFlags()
                                                                .Select(f => f.GetApiRepresentation())
                                                                .Concat(AccountDenied.GetFlags()
                                                                                     .Select(f => f.GetApiRepresentation()))
                                                                .Select(a => (($"permissions[{a}][explicit]", "1"), 
                                                                              ($"permissions[{a}][enabled]",  "0")))
                                                                .Interleave();

            IEnumerable<(string, string)> locked = GeneralLocked.GetFlags()
                                                                .Select(f => f.GetApiRepresentation())
                                                                .Concat(AccountLocked.GetFlags()
                                                                                     .Select(f => f.GetApiRepresentation()))
                                                                .Select(a => ($"permissions[{a}][locked]", "1"));
            
            return allowed.Concat(denied).Concat(locked);
        }
    }
}