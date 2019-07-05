using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UVACanvasAccess.Model.Reports;
using UVACanvasAccess.Structures.Reports;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.ApiParts {
    
    public partial class Api {

        [PaginatedResponse]
        private Task<HttpResponseMessage> RawListAvailableReports(string accountId) {
            var url = $"accounts/{accountId}/reports";
            return _client.GetAsync(url);
        }

        public async Task<IEnumerable<ReportDescription>> ListAvailableReports(ulong? accountId = null) {
            var response = await RawListAvailableReports(accountId?.ToString() ?? "self");

            var models = await AccumulateDeserializePages<ReportDescriptionModel>(response);

            return from model in models
                   select new ReportDescription(this, model);
        }
    }
}