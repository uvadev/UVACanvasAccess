using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UVACanvasAccess.Model.GradingPeriods;
using UVACanvasAccess.Structures.GradingPeriods;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {
        
        /// <summary>
        /// Streams all grading periods in a course.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <returns>The stream of <see cref="GradingPeriod"/>s.</returns>
        public async IAsyncEnumerable<GradingPeriod> StreamGradingPeriods(ulong courseId) {
            var response = await client.GetAsync($"courses/{courseId}/grading_periods" + BuildQueryString());
            
            await foreach (var redundantModel in StreamDeserializeObjectPages<RedundantGradingPeriodResponse>(response)) {
                foreach (var model in redundantModel.GradingPeriods) {
                    yield return new GradingPeriod(this, model);
                }
            }
        }
        
        /// <summary>
        /// Streams all grading periods in an account.
        /// </summary>
        /// <param name="accountId">The account id.</param>
        /// <returns>The stream of <see cref="GradingPeriod"/>s.</returns>
        public async IAsyncEnumerable<GradingPeriod> StreamAccountGradingPeriods(ulong accountId) {
            var response = await client.GetAsync($"accounts/{accountId}/grading_periods" + BuildQueryString());
            
            await foreach (var redundantModel in StreamDeserializeObjectPages<RedundantGradingPeriodResponse>(response)) {
                foreach (var model in redundantModel.GradingPeriods) {
                    yield return new GradingPeriod(this, model);
                }
            }
        }

        /// <summary>
        /// Gets a grading period by its id.
        /// </summary>
        /// <param name="courseId">The id of the course the grading period belongs to.</param>
        /// <param name="gradingPeriodId">The id of the grading period.</param>
        /// <returns>The grading period.</returns>
        public async Task<GradingPeriod> GetGradingPeriod(ulong courseId, ulong gradingPeriodId) {
            var response = await client.GetAsync($"courses/{courseId}/grading_periods/{gradingPeriodId}" + BuildQueryString());
            
            var model = JsonConvert.DeserializeObject<RedundantGradingPeriodResponse>(await response.Content.ReadAsStringAsync());
            return new GradingPeriod(this, model.GradingPeriods.First());
        }

        /// <summary>
        /// Updates a single grading period.
        /// </summary>
        /// <param name="courseId">The id of the course the grading period belongs to.</param>
        /// <param name="gradingPeriodId">The id of the grading period.</param>
        /// <param name="startDate">Optional; The new start date.</param>
        /// <param name="endDate">Optional; The new end date.</param>
        /// <param name="weight">Optional; The new weight.</param>
        /// <returns>The updated grading period.</returns>
        public async Task<GradingPeriod> UpdateGradingPeriod(ulong courseId,
                                                             ulong gradingPeriodId,
                                                             DateTime? startDate = null,
                                                             DateTime? endDate = null,
                                                             double? weight = null) {
            var args = new[] {
                ("start_date", startDate?.ToIso8601Date()),
                ("end_date", endDate?.ToIso8601Date()),
                ("weight", weight?.ToString()),
            }.Select(t => ($"grading_periods[][{t.Item1}]", t.Item2));
            
            var response = await client.PutAsync($"courses/{courseId}/grading_periods/{gradingPeriodId}", BuildHttpArguments(args));
            
            var model = JsonConvert.DeserializeObject<RedundantGradingPeriodResponse>(await response.Content.ReadAsStringAsync());
            return new GradingPeriod(this, model.GradingPeriods.First());
        }

        /// <summary>
        /// Perform a batch update on grading periods within a grading period set.
        /// Existing grading periods can be updated, and new ones created.
        /// </summary>
        /// <param name="gradingPeriodSetId">The grading period set id.</param>
        /// <param name="gradingPeriodUpdates">The list of updates.</param>
        /// <returns>The stream of new and updated grading periods.</returns>
        public IAsyncEnumerable<GradingPeriod> BatchUpdateGradingPeriods(ulong gradingPeriodSetId, 
                                                                         IEnumerable<GradingPeriodUpdate> gradingPeriodUpdates) {
            return RawBatchUpdateGradingPeriods(gradingPeriodUpdates, $"grading_period_sets/{gradingPeriodSetId}/grading_periods/batch_update");
        }

        /// <summary>
        /// Perform a batch update on grading periods within a course.
        /// Existing grading periods can be updated, and new ones created.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="gradingPeriodUpdates">The list of updates.</param>
        /// <returns>The stream of new and updated grading periods.</returns>
        public IAsyncEnumerable<GradingPeriod> BatchUpdateCourseGradingPeriods(ulong courseId,
                                                                               IEnumerable<GradingPeriodUpdate> gradingPeriodUpdates) {
            return RawBatchUpdateGradingPeriods(gradingPeriodUpdates, $"courses/{courseId}/grading_periods/batch_update");
        }

        private async IAsyncEnumerable<GradingPeriod> RawBatchUpdateGradingPeriods(IEnumerable<GradingPeriodUpdate> gradingPeriodUpdates, string uri) {
            var args = gradingPeriodUpdates.SelectMany(upd => new[] {
                ("id", upd.Id?.ToString()),
                ("title", upd.Title),
                ("start_date", upd.StartDate?.ToIso8601Date()),
                ("end_date", upd.EndDate?.ToIso8601Date()),
                ("close_date", upd.CloseDate?.ToIso8601Date()),
                ("weight", upd.Weight?.ToString())
            }, (_, tup) => ($"grading_periods[][{tup.Item1}]", tup.Item2));

            var response = await client.PatchAsync(uri, BuildHttpArguments(args));

            await foreach (var redundantModel in StreamDeserializeObjectPages<RedundantGradingPeriodResponse>(response)) {
                foreach (var model in redundantModel.GradingPeriods) {
                    yield return new GradingPeriod(this, model);
                }
            }
        }

        /// <summary>
        /// Deletes a grading period in a course.
        /// </summary>
        /// <param name="courseId">The id of the course the grading period belongs to.</param>
        /// <param name="gradingPeriodId">The id of the grading period.</param>
        public async Task DeleteGradingPeriod(ulong courseId, ulong gradingPeriodId) {
            await client.DeleteAsync($"courses/{courseId}/grading_periods/{gradingPeriodId}" + BuildQueryString());
        }
        
        /// <summary>
        /// Deletes a grading period in an account.
        /// </summary>
        /// <param name="accountId">The id of the account the grading period belongs to.</param>
        /// <param name="gradingPeriodId">The id of the grading period.</param>
        public async Task DeleteAccountGradingPeriod(ulong accountId, ulong gradingPeriodId) {
            await client.DeleteAsync($"accounts/{accountId}/grading_periods/{gradingPeriodId}" + BuildQueryString());
        }
    }
}
