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
        
        /// <summary>
        /// Streams the enrollment terms for an account.
        /// </summary>
        /// <param name="accountId">The account id. Defaults to the current account.</param>
        /// <param name="workflowState">An optional workflow state to filter by.</param>
        /// <param name="includes">Optional data to include with the results.</param>
        /// <returns>The stream of <see cref="EnrollmentTerm"/>s.</returns>
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
            
            var response = await client.GetAsync($"accounts/{accountId?.ToString() ?? "self"}/terms" + 
                                                  BuildDuplicateKeyQueryString(args.ToArray()));
            
            await foreach (var redundantModel in StreamDeserializeObjectPages<RedundantEnrollmentTermsResponse>(response)) {
                foreach (var model in redundantModel.EnrollmentTerms) {
                    yield return new EnrollmentTerm(this, model);
                }
            }
        }

        /// <summary>
        /// Categories of workflow state to filter by when querying enrollment terms.
        /// </summary>
        /// <seealso cref="Api.StreamEnrollmentTerms"/>
        /// <seealso cref="EnrollmentTerm"/>
        [PublicAPI]
        public enum EnrollmentTermWorkflowStateFilter {
            /// <summary>
            /// The term is active.
            /// </summary>
            [ApiRepresentation("active")]
            Active,
            /// <summary>
            /// The term is deleted.
            /// </summary>
            [ApiRepresentation("deleted")]
            Deleted,
            /// <summary>
            /// All terms.
            /// </summary>
            [ApiRepresentation("all")]
            All
        }

        /// <summary>
        /// Optional data that can be included with <see cref="EnrollmentTerm"/> objects.
        /// </summary>
        [PublicAPI]
        [Flags]
        public enum EnrollmentTermIncludes {
            /// <summary>
            /// Include any term overrides.
            /// </summary>
            [ApiRepresentation("overrides")]
            Overrides
        }

        /// <summary>
        /// Gets an enrollment term by id.
        /// </summary>
        /// <param name="termId">The term id.</param>
        /// <param name="accountId">The account id. Defaults to the current account.</param>
        /// <returns>The <see cref="EnrollmentTerm"/>.</returns>
        public async Task<EnrollmentTerm> GetEnrollmentTerm(ulong termId, ulong? accountId = null) {
            var response = await client.GetAsync($"accounts/{accountId?.ToString() ?? "self"}/terms/{termId}" + BuildQueryString());
            var model = JsonConvert.DeserializeObject<EnrollmentTermModel>(await response.Content.ReadAsStringAsync());
            return new EnrollmentTerm(this, model);
        }
        
        /// <summary>
        /// Deletes an enrollment term.
        /// </summary>
        /// <param name="termId">The term id.</param>
        /// <param name="accountId">The account id. Defaults to the current account.</param>
        /// <returns>The now-deleted <see cref="EnrollmentTerm"/>.</returns>
        public async Task<EnrollmentTerm> DeleteEnrollmentTerm(ulong termId, ulong? accountId = null) {
            var response = await client.DeleteAsync($"accounts/{accountId?.ToString() ?? "self"}/terms/{termId}" + BuildQueryString());
            var model = JsonConvert.DeserializeObject<EnrollmentTermModel>(await response.Content.ReadAsStringAsync());
            return new EnrollmentTerm(this, model);
        }

        /// <summary>
        /// Creates an enrollment term.
        /// </summary>
        /// <param name="builder">The completed <see cref="EnrollmentTermBuilder"/>.</param>
        /// <param name="accountId">The account id. Defaults to the current account.</param>
        /// <returns>The newly created <see cref="EnrollmentTerm"/>.</returns>
        public async Task<EnrollmentTerm> CreateEnrollmentTerm(EnrollmentTermBuilder builder, ulong? accountId = null) {
            var response = await client.PostAsync($"accounts/{accountId?.ToString() ?? "self"}/terms", 
                                                   BuildHttpArguments(builder.ToParams()));
            var model = JsonConvert.DeserializeObject<EnrollmentTermModel>(await response.Content.ReadAsStringAsync());
            return new EnrollmentTerm(this, model);
        }

        /// <summary>
        /// Updates an enrollment term.
        /// </summary>
        /// <param name="enrollmentId">The enrollment id.</param>
        /// <param name="builder">The completed <see cref="EnrollmentTermBuilder"/>.</param>
        /// <param name="accountId">The account id. Defaults to the current account.</param>
        /// <returns>The updated <see cref="EnrollmentTerm"/>.</returns>
        public async Task<EnrollmentTerm> UpdateEnrollmentTerm(ulong enrollmentId, EnrollmentTermBuilder builder, ulong? accountId = null) {
            var response = await client.PutAsync($"accounts/{accountId?.ToString() ?? "self"}/terms/{enrollmentId}", 
                                                  BuildHttpArguments(builder.ToParams()));
            var model = JsonConvert.DeserializeObject<EnrollmentTermModel>(await response.Content.ReadAsStringAsync());
            return new EnrollmentTerm(this, model);
        }
    }
}
