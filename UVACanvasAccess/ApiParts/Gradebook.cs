using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UVACanvasAccess.Model.Gradebook;
using UVACanvasAccess.Structures.Gradebook;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {

        [PaginatedResponse]
        private Task<HttpResponseMessage> RawGetGradebookDays(string courseId) {
            var url = $"courses/{courseId}/gradebook_history/days";
            return _client.GetAsync(url);
        }

        public async Task<IEnumerable<Day>> GetGradebookDays(ulong courseId) {
            var response = await RawGetGradebookDays(courseId.ToString());

            var models = await AccumulateDeserializePages<DayModel>(response);
            return from m in models
                   select new Day(this, m);
        }
    }
}