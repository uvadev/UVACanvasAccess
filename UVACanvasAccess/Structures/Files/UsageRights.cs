using System.Collections.Generic;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Files;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Files {
    
    [PublicAPI]
    public class UsageRights : IPrettyPrint {
        private readonly Api _api;
        
        public string LegalCopyright { get; }
        
        public UsageJustification UsageJustification { get; }
        
        public string License { get; }
        
        public string LicenseName { get; }
        
        public string Message { get; }
        
        public IEnumerable<ulong> FileIds { get; }

        internal UsageRights(Api api, UsageRightsModel model) {
            _api = api;
            LegalCopyright = model.LegalCopyright;
            UsageJustification = model.UsageJustification?.ToApiRepresentedEnum<UsageJustification>()
                                                         ?? UsageJustification.Unknown;
            License = model.License;
            LicenseName = model.LicenseName;
            Message = model.Message;
            FileIds = model.FileIds;
        }

        public string ToPrettyString() {
            return "UsageRights {" +
                   ($"\n{nameof(LegalCopyright)}: {LegalCopyright}," +
                   $"\n{nameof(UsageJustification)}: {UsageJustification.ToString()}," +
                   $"\n{nameof(License)}: {License}," +
                   $"\n{nameof(LicenseName)}: {LicenseName}," +
                   $"\n{nameof(Message)}: {Message}," +
                   $"\n{nameof(FileIds)}: {FileIds}").Indent(4) + 
                   "\n}";
        }
    }

    [PublicAPI]
    public enum UsageJustification : byte {
        Unknown,
        [ApiRepresentation("own_copyright")]
        OwnCopyright,
        [ApiRepresentation("public_domain")]
        PublicDomain,
        [ApiRepresentation("used_by_permission")]
        UsedWithPermission,
        [ApiRepresentation("fair_use")]
        FairUse,
        [ApiRepresentation("creative_commons")]
        CreativeCommons
    }
}
