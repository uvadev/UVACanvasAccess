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

        /// <summary>
        /// Gets the page view and participation counts across all courses in the department during the given term.
        /// </summary>
        /// <param name="termId">The term to get data for.</param>
        /// <param name="accountId">The account. <c>self</c> by default.</param>
        /// <returns>The participation data.</returns>
        public Task<DepartmentParticipation> GetDepartmentParticipationData(ulong termId, ulong? accountId = null) {
            return BaseGetDepartmentParticipationData($"terms/{termId}", accountId?.ToString() ?? "self");
        }

        /// <summary>
        /// Gets the page view and participation counts across all current or completed courses in the department during the default term.
        /// </summary>
        /// <param name="currentCourses">
        /// If true, the data returned will be across current courses. Otherwise, it will be across completed courses.
        /// True by default.
        /// </param>
        /// <param name="accountId">The account. <c>self</c> by default.</param>
        /// <returns>The participation data.</returns>
        public Task<DepartmentParticipation> GetDefaultTermDepartmentParticipationData(bool currentCourses = true, 
                                                                                       ulong? accountId = null) {
            return BaseGetDepartmentParticipationData(currentCourses ? "current" : "completed",
                                                      accountId?.ToString() ?? "self");
        }

        private async Task<Dictionary<byte, ulong>> BaseGetDepartmentGradeData(string infix, string accountId) {
            var response = await _client.GetAsync($"accounts/{accountId}/analytics/{infix}/grades");
            var dict =
                JsonConvert.DeserializeObject<Dictionary<string, ulong>>(await response.Content.ReadAsStringAsync());
            return dict.KeySelect(byte.Parse);
        }

        /// <summary>
        /// Gets the distribution of grades for students across all courses in the department during the given term.
        /// </summary>
        /// <param name="termId">The term to get data for.</param>
        /// <param name="accountId">The account. <c>self</c> by default.</param>
        /// <returns>The grade data.</returns>
        public Task<Dictionary<byte, ulong>> GetDepartmentGradeData(ulong termId, ulong? accountId = null) {
            return BaseGetDepartmentGradeData($"terms/{termId}", accountId?.ToString() ?? "self");
        }
        
        /// <summary>
        /// Gets the distribution of grades for students across all current or completed courses in the department during the default term.
        /// </summary>
        /// <param name="currentCourses">
        /// If true, the data returned will be across current courses. Otherwise, it will be across completed courses.
        /// True by default.
        /// </param>
        /// <param name="accountId">The account. <c>self</c> by default.</param>
        /// <returns>The grade data.</returns>
        public Task<Dictionary<byte, ulong>> GetDefaultTermDepartmentGradeData(bool currentCourses = true,
                                                                               ulong? accountId = null) {
            return BaseGetDepartmentGradeData(currentCourses ? "current" : "completed", 
                                              accountId?.ToString() ?? "self");
        }

        private async Task<DepartmentStatistics> BaseGetDepartmentStatistics(string infix, string accountId) {
            var response = await _client.GetAsync($"accounts/{accountId}/analytics/{infix}/statistics");
            var model =
                JsonConvert.DeserializeObject<DepartmentStatisticsModel>(await response.Content.ReadAsStringAsync());
            return new DepartmentStatistics(model);
        }

        /// <summary>
        /// Gets some numeric statistics about the department relative to the given term.
        /// </summary>
        /// <param name="termId">The term to get data for.</param>
        /// <param name="accountId">The account. <c>self</c> by default.</param>
        /// <returns>The statistics.</returns>
        public Task<DepartmentStatistics> GetDepartmentStatistics(ulong termId, ulong? accountId = null) {
            return BaseGetDepartmentStatistics($"terms/{termId}", accountId?.ToString() ?? "self");
        }

        /// <summary>
        /// Gets some numeric statistics about the department relative to current ot completed courses in the default term.
        /// </summary>
        /// <param name="currentCourses">
        /// If true, the data returned will be across current courses. Otherwise, it will be across completed courses.
        /// True by default.
        /// </param>
        /// <param name="accountId">The account. <c>self</c> by default.</param>
        /// <returns>The statistics.</returns>
        public Task<DepartmentStatistics> GetDefaultTermDepartmentStatistics(bool currentCourses = true,
                                                                             ulong? accountId = null) {
            return BaseGetDepartmentStatistics(currentCourses ? "current" : "completed",
                                               accountId?.ToString() ?? "self");
        }

        public async Task<UserParticipation> GetUserCourseParticipationData(ulong userId, ulong courseId) {
            var response = await _client.GetAsync($"courses/{courseId}/analytics/users/{userId}/activity");

            return new UserParticipation(JsonConvert.DeserializeObject<UserParticipationModel>(await response.Content.ReadAsStringAsync()));
        }

        public async IAsyncEnumerable<UserAssignmentData> GetUserCourseAssignmentData(ulong courseId, ulong userId) {
            var response = await _client.GetAsync($"courses/{courseId}/analytics/users/{userId}/assignments");

            var models = StreamDeserializePages<UserAssignmentDataModel>(response);

            await foreach (var m in models) {
                yield return new UserAssignmentData(m);
            }
        }
    }
}
