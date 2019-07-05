using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
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

        private Task<HttpResponseMessage> RawStartReport(string accountId, string reportType, HttpContent args) {
            var url = $"accounts/{accountId}/reports/{reportType}";
            return _client.PostAsync(url, args);
        }

        public async Task<Report> StartReport(string reportType,
                                              IEnumerable<(string, object)> parameters,
                                              ulong? accountId = null) {
            var content = BuildMultipartHttpArguments(parameters.ValSelect(JsonConvert.SerializeObject)
                                                                .ValSelect(s => s.Replace("\"", string.Empty).Trim()));

            var response = await RawStartReport(accountId?.ToString() ?? "self", reportType, content);

            var model = JsonConvert.DeserializeObject<ReportModel>(await response.Content.ReadAsStringAsync());
            return new Report(this, model);
        }
    }
}