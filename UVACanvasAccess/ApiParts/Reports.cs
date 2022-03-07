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

        /// <summary>
        /// Get a list of available report types and parameters for a given account.
        /// </summary>
        /// <param name="accountId">The account id. Defaults to self.</param>
        /// <returns>The list of available report types.</returns>
        public async Task<IEnumerable<ReportDescription>> ListAvailableReports(ulong? accountId = null) {
            var response = await RawListAvailableReports(accountId?.ToString() ?? "self");

            var models = await AccumulateDeserializePages<ReportDescriptionModel>(response);

            return models.Select(model => new ReportDescription(this, model));
        }

        private Task<HttpResponseMessage> RawStartReport(string accountId, string reportType, HttpContent args) {
            var url = $"accounts/{accountId}/reports/{reportType}";
            return _client.PostAsync(url, args);
        }

        /// <summary>
        /// Run a report.
        /// </summary>
        /// <param name="reportType">The type of report to run, as given by <see cref="ListAvailableReports"/>.</param>
        /// <param name="parameters">
        /// The optional set of report parameters.
        /// The name, type, and semantics of these parameters are specific to each report type.
        /// </param>
        /// <param name="accountId">The account id. Defaults to self.</param>
        /// <returns>The task containing the initial report instance.</returns>
        /// <seealso cref="ListAvailableReports"/>
        public async Task<Report> StartReport(string reportType,
                                              IEnumerable<(string, object)> parameters,
                                              ulong? accountId = null) {
            var content = BuildMultipartHttpArguments(
                parameters.ValSelect(JsonConvert.SerializeObject)
                          .KeySelect(s => $"parameters[{s}]")
                          .ValSelect(s => s.Replace("\"", string.Empty).Trim())
            );

            var response = await RawStartReport(accountId?.ToString() ?? "self", reportType, content);

            var model = JsonConvert.DeserializeObject<ReportModel>(await response.Content.ReadAsStringAsync());
            return new Report(this, model);
        }

        /// <summary>
        /// Gets the status of a report by id.
        /// </summary>
        /// <param name="reportType">The report type.</param>
        /// <param name="reportId">The report id.</param>
        /// <param name="accountId">The account id the report was run under; defaults to self.</param>
        /// <returns>The task containing the report.</returns>
        public async Task<Report> GetReportStatus(string reportType, ulong reportId, ulong? accountId = null) {
            var response = await _client.GetAsync($"accounts/{accountId?.ToString() ?? "self"}/reports/{reportType}/{reportId}");
            var model = JsonConvert.DeserializeObject<ReportModel>(await response.Content.ReadAsStringAsync());
            return new Report(this, model);
        }

        [PaginatedResponse]
        private Task<HttpResponseMessage> RawGetReportIndex(string accountId, string reportType) {
            var url = $"accounts/{accountId}/reports/{reportType}";
            return _client.GetAsync(url);
        }

        public async Task<IEnumerable<Report>> GetReportIndex(string reportType, ulong? accountId = null) {
            var response = await RawGetReportIndex(accountId?.ToString() ?? "self", reportType);

            var models = await AccumulateDeserializePages<ReportModel>(response);
            return from model in models
                   select new Report(this, model);
        }
    }
}