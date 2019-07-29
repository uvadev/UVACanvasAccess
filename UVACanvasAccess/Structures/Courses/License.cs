using JetBrains.Annotations;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Courses {
    
    [PublicAPI]
    public enum License : byte {
        [ApiRepresentation("private")]
        Private,
        [ApiRepresentation("cc_by_nc_nd")]
        CcByNcNd,
        [ApiRepresentation("cc_by_nc_sa")]
        CcByNcSa,
        [ApiRepresentation("cc_by_nc")]
        CcByNc,
        [ApiRepresentation("cc_by_nd")]
        CcByNd,
        [ApiRepresentation("cc_by_sa")]
        CcBySa,
        [ApiRepresentation("cc_by")]
        CcBy,
        [ApiRepresentation("public_domain")]
        PublicDomain
    }
}
