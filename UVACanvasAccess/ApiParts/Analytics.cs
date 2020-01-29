using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
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
            return BaseGetDepartmentParticipationData($"terms/{termId}", accountId.IdOrSelf());
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
                                                      accountId.IdOrSelf());
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
            return BaseGetDepartmentStatistics($"terms/{termId}", accountId.IdOrSelf());
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
            return BaseGetDepartmentStatistics(currentCourses ? "current" : "completed", accountId.IdOrSelf());
        }

        /// <summary>
        /// Gets participation details for the user for the entire history of the course.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="courseId">The course id.</param>
        /// <returns>The data.</returns>
        public async Task<UserParticipation> GetUserCourseParticipationData(ulong userId, ulong courseId) {
            var response = await _client.GetAsync($"courses/{courseId}/analytics/users/{userId}/activity");

            return new UserParticipation(JsonConvert.DeserializeObject<UserParticipationModel>(await response.Content.ReadAsStringAsync()));
        }

        /// <summary>
        /// Returns a list of assignments for the course sorted by due date, along with assignment information,
        /// grade breakdown, and submission information if relevant.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns>The data.</returns>
        public async IAsyncEnumerable<UserAssignmentData> GetUserCourseAssignmentData(ulong courseId, ulong userId) {
            var response = await _client.GetAsync($"courses/{courseId}/analytics/users/{userId}/assignments");

            await foreach (var m in StreamDeserializePages<UserAssignmentDataModel>(response)) {
                yield return new UserAssignmentData(m);
            }
        }

        /// <summary>
        /// The columns which the data from <see cref="Api.StreamCourseStudentSummary"/> can be sorted by.
        /// </summary>
        [PublicAPI]
        public enum StudentCourseSummarySortColumn : byte {
            [ApiRepresentation("name")]
            Name,
            [ApiRepresentation("name_descending")]
            NameDescending,
            [ApiRepresentation("score")]
            Score,
            [ApiRepresentation("score_descending")]
            ScoreDescending,
            [ApiRepresentation("participations")]
            Participations,
            [ApiRepresentation("participations_descending")]
            ParticipationsDescending,
            [ApiRepresentation("page_views")]
            PageViews,
            [ApiRepresentation("page_views_descending")]
            PageViewsDescending
        }

        /// <summary>
        /// Gets a summary of per-user access information for students in this course, including total page views,
        /// total participations, and a breakdown of late statuses for all submissions.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="studentId">Optionally, a student id to filter by.</param>
        /// <param name="sortBy">An optional <see cref="StudentCourseSummarySortColumn">column</see> to sort by.</param>
        /// <returns>The data.</returns>
        public async IAsyncEnumerable<CourseStudentSummary> StreamCourseStudentSummary(ulong courseId, 
                                                                                       ulong? studentId = null, 
                                                                                       StudentCourseSummarySortColumn? sortBy = null) {
            var response = await _client.GetAsync($"courses/{courseId}/analytics/student_summaries" + 
                                                  BuildQueryString(("student_id", studentId.ToString()),
                                                                   ("sort_column", sortBy?.GetApiRepresentation())));

            var models = StreamDeserializePages<CourseStudentSummaryModel>(response);

            await foreach (var model in models) {
                yield return new CourseStudentSummary(model);
            }
        }

        public async IAsyncEnumerable<CourseAssignmentSummary> StreamCourseAssignmentSummary(ulong courseId) {
            var response = await _client.GetAsync($"courses/{courseId}/analytics/assignments");
            var models = StreamDeserializePages<CourseAssignmentSummaryModel>(response);

            await foreach (var model in models) {
                yield return new CourseAssignmentSummary(model);
            }
        }
    }
}
