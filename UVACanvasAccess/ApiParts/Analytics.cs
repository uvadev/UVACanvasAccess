using System.Threading.Tasks;
using Newtonsoft.Json;
using UVACanvasAccess.Model.Analytics;
using UVACanvasAccess.Structures.Analytics;

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
    }
}
