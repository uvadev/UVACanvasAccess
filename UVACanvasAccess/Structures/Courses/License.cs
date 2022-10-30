using JetBrains.Annotations;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Courses {
    
    /// <summary>
    /// Represents some common license strings.
    /// </summary>
    [PublicAPI]
    public enum License : byte {
        /// <summary>
        /// All rights reserved.
        /// </summary>
        [ApiRepresentation("private")]
        Private,
        /// <summary>
        /// Creative Commons Attribution-NonCommercial-NoDerivatives
        /// </summary>
        [ApiRepresentation("cc_by_nc_nd")]
        CcByNcNd,
        /// <summary>
        /// Creative Commons Attribution-NonCommercial-ShareAlike
        /// </summary>
        [ApiRepresentation("cc_by_nc_sa")]
        CcByNcSa,
        /// <summary>
        /// Creative Commons Attribution-NonCommercial
        /// </summary>
        [ApiRepresentation("cc_by_nc")]
        CcByNc,
        /// <summary>
        /// Creative Commons Attribution-NoDerivatives
        /// </summary>
        [ApiRepresentation("cc_by_nd")]
        CcByNd,
        /// <summary>
        /// Creative Commons Attribution-ShareAlike
        /// </summary>
        [ApiRepresentation("cc_by_sa")]
        CcBySa,
        /// <summary>
        /// Creative Commons Attribution
        /// </summary>
        [ApiRepresentation("cc_by")]
        CcBy,
        /// <summary>
        /// Public domain work.
        /// </summary>
        [ApiRepresentation("public_domain")]
        PublicDomain
    }
}
