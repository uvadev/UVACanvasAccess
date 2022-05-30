using System.Collections.Generic;
using UVACanvasAccess.Model.GradingPeriods;
using UVACanvasAccess.Structures.GradingPeriods;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {
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
