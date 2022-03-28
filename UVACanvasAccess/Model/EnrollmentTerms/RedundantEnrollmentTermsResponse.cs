using System.Collections.Generic;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.EnrollmentTerms {
    internal struct RedundantEnrollmentTermsResponse {
        [JsonProperty("enrollment_terms")]
        public IEnumerable<EnrollmentTermModel> EnrollmentTerms { get; set; }
    }
}
