using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UVACanvasAccess.Model.EnrollmentTerms;
using UVACanvasAccess.Structures.EnrollmentTerms;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {
        public async IAsyncEnumerable<EnrollmentTerm> StreamEnrollmentTerms(ulong? accountId = null, 
                                                                            EnrollmentTermWorkflowStateFilter? workflowState = null,
                                                                            EnrollmentTermIncludes? includes = null) {
            var args = new List<(string, string)>();
            
            if (workflowState != null) {
                args.Add(("workflow_state[]", workflowState.GetApiRepresentation()));
            }

            if (includes != null) {
                args.AddRange(includes.GetFlagsApiRepresentations().Select(f => ("include[]", f)));
            }
            
            var response = await _client.GetAsync($"accounts/{accountId?.ToString() ?? "self"}/terms" + 
                                                  BuildDuplicateKeyQueryString(args.ToArray()));
            
            await foreach (var redundantModel in StreamDeserializeObjectPages<RedundantEnrollmentTermsResponse>(response)) {
                foreach (var model in redundantModel.EnrollmentTerms) {
                    yield return new EnrollmentTerm(this, model);
                }
            }
        }

        [PublicAPI]
        public enum EnrollmentTermWorkflowStateFilter {
            [ApiRepresentation("active")]
            Active,
            [ApiRepresentation("deleted")]
            Deleted,
            [ApiRepresentation("all")]
            All
        }

        [PublicAPI]
        [Flags]
        public enum EnrollmentTermIncludes {
            [ApiRepresentation("overrides")]
            Overrides
        }

        public async Task<EnrollmentTerm> GetEnrollmentTerm(ulong termId, ulong? accountId = null) {
            var response = await _client.GetAsync($"accounts/{accountId?.ToString() ?? "self"}/terms/{termId}" + BuildQueryString());
            var model = JsonConvert.DeserializeObject<EnrollmentTermModel>(await response.Content.ReadAsStringAsync());
            return new EnrollmentTerm(this, model);
        }
        
        public async Task<EnrollmentTerm> DeleteEnrollmentTerm(ulong termId, ulong? accountId = null) {
            var response = await _client.DeleteAsync($"accounts/{accountId?.ToString() ?? "self"}/terms/{termId}" + BuildQueryString());
            var model = JsonConvert.DeserializeObject<EnrollmentTermModel>(await response.Content.ReadAsStringAsync());
            return new EnrollmentTerm(this, model);
        }

        public async Task<EnrollmentTerm> CreateEnrollmentTerm(EnrollmentTermBuilder builder, ulong? accountId = null) {
            var response = await _client.PostAsync($"accounts/{accountId?.ToString() ?? "self"}/terms", 
                                                   BuildHttpArguments(builder.ToParams()));
            var model = JsonConvert.DeserializeObject<EnrollmentTermModel>(await response.Content.ReadAsStringAsync());
            return new EnrollmentTerm(this, model);
        }

        public async Task<EnrollmentTerm> UpdateEnrollmentTerm(ulong enrollmentId, EnrollmentTermBuilder builder, ulong? accountId = null) {
            var response = await _client.PutAsync($"accounts/{accountId?.ToString() ?? "self"}/terms/{enrollmentId}", 
                                                  BuildHttpArguments(builder.ToParams()));
            var model = JsonConvert.DeserializeObject<EnrollmentTermModel>(await response.Content.ReadAsStringAsync());
            return new EnrollmentTerm(this, model);
        }
    }
}
