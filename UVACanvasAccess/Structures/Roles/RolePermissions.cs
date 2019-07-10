using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Roles;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Roles {
    
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
    // ReSharper disable MemberCanBePrivate.Global
    public class RolePermissions : IPrettyPrint {
        private readonly Api _api;
        
        public bool Enabled { get; }
        
        public bool Locked { get; }
        
        public bool AppliesToSelf { get; }
        
        public bool AppliesToDescendants { get; }
        
        public bool Readonly { get; }
        
        public bool Explicit { get; }
        
        public bool PriorDefault { get; }

        public RolePermissions(Api api, RolePermissionsModel model) {
            _api = api;
            Enabled = model.Enabled;
            Locked = model.Locked;
            AppliesToSelf = model.AppliesToSelf;
            AppliesToDescendants = model.AppliesToDescendants;
            Readonly = model.Readonly;
            Explicit = model.Explicit;
            PriorDefault = model.PriorDefault;
        }

        public string ToPrettyString() {
            return "RolePermissions { " +
                   ($"\n{nameof(Enabled)}: {Enabled}," +
                   $"\n{nameof(Locked)}: {Locked}," +
                   $"\n{nameof(AppliesToSelf)}: {AppliesToSelf}," +
                   $"\n{nameof(AppliesToDescendants)}: {AppliesToDescendants}," +
                   $"\n{nameof(Readonly)}: {Readonly}," +
                   $"\n{nameof(Explicit)}: {Explicit}," +
                   $"\n{nameof(PriorDefault)}: {PriorDefault}").Indent(4) + 
                   "\n}";
        }
    }
}