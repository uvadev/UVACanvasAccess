using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UVACanvasAccess.Model.Analytics;
using UVACanvasAccess.Structures.Analytics;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {

        private async Task<DepartmentParticipation> BaseGetDepartmentParticipationData(string infix, string accountId) {
            var response = await _client.GetAsync($"accounts/{accountId}/analytics/{infix}/activity");
            var model =
                JsonConvert.DeserializeObject<DepartmentParticipationModel>(await response.Content.ReadAsStringAsync());
            return new DepartmentParticipation(model);
        }

        public Task<DepartmentParticipation> GetDepartmentParticipationData(ulong termId, ulong? accountId = null) {
            return BaseGetDepartmentParticipationData($"terms/{termId}", accountId?.ToString() ?? "self");
        }

        public Task<DepartmentParticipation> GetDefaultTermDepartmentParticipationData(bool currentCourses = true, 
                                                                                       ulong? accountId = null) {
            return BaseGetDepartmentParticipationData(currentCourses ? "current"
                                                                          : "completed",
                                                      accountId?.ToString() ?? "self");
        }

        private async Task<Dictionary<byte, ulong>> BaseGetDepartmentGradeData(string infix, string accountId) {
            var response = await _client.GetAsync($"accounts/{accountId}/analytics/{infix}/grades");
            var dict =
                JsonConvert.DeserializeObject<Dictionary<string, ulong>>(await response.Content.ReadAsStringAsync());
            return dict.KeySelect(byte.Parse);
        }

        public Task<Dictionary<byte, ulong>> GetDepartmentGradeData(ulong termId, ulong? accountId = null) {
            return BaseGetDepartmentGradeData($"terms/{termId}", accountId?.ToString() ?? "self");
        }
        
        public Task<Dictionary<byte, ulong>> GetDefaultTermDepartmentGradeData(bool currentCourses = true,
                                                                               ulong? accountId = null) {
            return BaseGetDepartmentGradeData(currentCourses ? "current"
                                                                  : "completed", 
                                              accountId?.ToString() ?? "self");
        }

        private async Task<DepartmentStatistics> BaseGetDepartmentStatistics(string infix, string accountId) {
            var response = await _client.GetAsync($"accounts/{accountId}/analytics/{infix}/statistics");
            var model =
                JsonConvert.DeserializeObject<DepartmentStatisticsModel>(await response.Content.ReadAsStringAsync());
            return new DepartmentStatistics(model);
        }

        public Task<DepartmentStatistics> GetDepartmentStatistics(ulong termId, ulong? accountId = null) {
            return BaseGetDepartmentStatistics($"terms/{termId}", accountId?.ToString() ?? "self");
        }

        public Task<DepartmentStatistics> GetDefaultTermDepartmentStatistics(bool currentCourses = true,
                                                                             ulong? accountId = null) {
            return BaseGetDepartmentStatistics(currentCourses ? "current"
                                                                   : "completed",
                                               accountId?.ToString() ?? "self");
        }
    }
}
