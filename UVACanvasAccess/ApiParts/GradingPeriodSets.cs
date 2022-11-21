using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UVACanvasAccess.Model.GradingPeriodSets;
using UVACanvasAccess.Structures.GradingPeriodSets;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {
        
        /// <summary>
        /// Streams all grading period sets in an account.
        /// </summary>
        /// <param name="accountId">The account id.</param>
        /// <returns>The stream of grading period sets.</returns>
        public async IAsyncEnumerable<GradingPeriodSet> StreamGradingPeriodSets(ulong accountId) {
            var response = await client.GetAsync($"accounts/{accountId}/grading_period_sets" + BuildQueryString());
            
            await foreach (var redundantModel in StreamDeserializeObjectPages<RedundantGradingPeriodSetResponse>(response)) {
                foreach (var model in redundantModel.GradingPeriodSet) {
                    yield return new GradingPeriodSet(this, model);
                }
            }
        }

        /// <summary>
        /// Creates a new grading period set.
        /// </summary>
        /// <param name="accountId">The id of the account under which to create the grading period set.</param>
        /// <param name="title">The title of the set.</param>
        /// <param name="termIds">Optional; Term ids to associate with the set.</param>
        /// <param name="weighted">Optional; Whether the set is weighted.</param>
        /// <param name="displayTotalsForAllGradingPeriods">Optional; Whether the totals for all grading periods in the set are displayed.</param>
        /// <returns>The newly created set.</returns>
        public async Task<GradingPeriodSet> CreateGradingPeriodSet(ulong accountId,
                                                                   string title,
                                                                   [CanBeNull] IEnumerable<ulong> termIds = null,
                                                                   bool? weighted = null,
                                                                   bool? displayTotalsForAllGradingPeriods = null) {
            var args = new[] {
                ("title", title),
                ("weighted", weighted?.ToShortString()),
                ("display_totals_for_all_grading_periods", displayTotalsForAllGradingPeriods?.ToShortString())
            }.Select(t => ($"grading_period_set[][{t.Item1}]", t.Item2)).ToList();

            if (termIds != null) {
                args.AddRange(termIds.Select(id => ("enrollment_term_ids[]", id.ToString())));
            }

            var response =
                await client.PostAsync($"accounts/{accountId}/grading_period_sets", BuildHttpArguments(args));
            
            var model = JsonConvert.DeserializeObject<RedundantGradingPeriodSetResponse>(await response.Content.ReadAsStringAsync());
            return new GradingPeriodSet(this, model.GradingPeriodSet.First());
        }

        /// <summary>
        /// Updates an existing grading period set.
        /// </summary>
        /// <param name="accountId">The account id.</param>
        /// <param name="gradingPeriodSetId">The grading period set id.</param>
        /// <param name="title">Optional; The new title.</param>
        /// <param name="termIds">Optional; The new list of term ids to associate with the set.</param>
        /// <param name="weighted">Optional; The new value of this setting.</param>
        /// <param name="displayTotalsForAllGradingPeriods">Optional; The new value of this setting.</param>
        public async Task UpdateGradingPeriodSet(ulong accountId,
                                                 ulong gradingPeriodSetId,
                                                 [CanBeNull] string title = null,
                                                 [CanBeNull] IEnumerable<ulong> termIds = null,
                                                 bool? weighted = null,
                                                 bool? displayTotalsForAllGradingPeriods = null) {
            var args = new[] {
                ("title", title),
                ("weighted", weighted?.ToShortString()),
                ("display_totals_for_all_grading_periods", displayTotalsForAllGradingPeriods?.ToShortString())
            }.Select(t => ($"grading_period_set[][{t.Item1}]", t.Item2)).ToList();

            if (termIds != null) {
                args.AddRange(termIds.Select(id => ("enrollment_term_ids[]", id.ToString())));
            }
            
            await client.PatchAsync($"accounts/{accountId}/grading_period_sets/{gradingPeriodSetId}", BuildHttpArguments(args));
        }
        
        /// <summary>
        /// Deletes a grading period set.
        /// </summary>
        /// <param name="accountId">The id of the account the grading period set belongs to.</param>
        /// <param name="gradingPeriodSetId">The id of the grading period set.</param>
        public async Task DeleteGradingPeriodSet(ulong accountId, ulong gradingPeriodSetId) {
            await client.DeleteAsync($"accounts/{accountId}/grading_period_sets/{gradingPeriodSetId}" + BuildQueryString());
        }
    }
}
