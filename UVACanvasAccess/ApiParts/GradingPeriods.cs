using System.Collections.Generic;
using UVACanvasAccess.Model.GradingPeriods;
using UVACanvasAccess.Structures.GradingPeriods;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {
        
        /// <summary>
        /// Streams all grading periods in a course.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <returns>The stream of <see cref="GradingPeriod"/>s.</returns>
        public async IAsyncEnumerable<GradingPeriod> StreamGradingPeriods(ulong courseId) {
            var response = await client.GetAsync($"courses/{courseId}/grading_periods");
            
            await foreach (var redundantModel in StreamDeserializeObjectPages<RedundantGradingPeriodResponse>(response)) {
                foreach (var model in redundantModel.GradingPeriods) {
                    yield return new GradingPeriod(this, model);
                }
            }
        }
    }
}
