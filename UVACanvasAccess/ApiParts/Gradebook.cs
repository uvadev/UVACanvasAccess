using System;
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
            return client.GetAsync(url);
        }

        /// <summary>
        /// Returns a list of days in the gradebook history for this course.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <returns>The list of days.</returns>
        public async Task<IEnumerable<Day>> GetGradebookDays(ulong courseId) {
            var response = await RawGetGradebookDays(courseId.ToString());

            var models = await AccumulateDeserializePages<DayModel>(response);
            return from m in models
                   select new Day(this, m);
        }

        [PaginatedResponse]
        private Task<HttpResponseMessage> RawGetDailyGraders(string courseId, DateTime date) {
            var url = $"courses/{courseId}/gradebook_history/{date.ToIso8601Date()}";
            return client.GetAsync(url);
        }

        /// <summary>
        /// Returns a list of graders who worked this day, along with the assignments they worked on.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="date">The day.</param>
        /// <returns>The list of graders.</returns>
        public async Task<IEnumerable<Grader>> GetDailyGraders(ulong courseId, DateTime date) {
            var response = await RawGetDailyGraders(courseId.ToString(), date);

            var models = await AccumulateDeserializePages<GraderModel>(response);
            return from m in models
                   select new Grader(this, m);
        }

        [PaginatedResponse]
        private Task<HttpResponseMessage> RawGetDailySubmissionHistories(string courseId, DateTime date, string graderId, string assignmentId) {
            var url = $"courses/{courseId}/gradebook_history/{date.ToIso8601Date()}/graders/{graderId}/assignments/{assignmentId}/submissions";
            return client.GetAsync(url);
        }

        /// <summary>
        /// Returns a list of submission histories for this day.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="date">The date.</param>
        /// <param name="graderId">The grader id.</param>
        /// <param name="assignmentId">The assignment id.</param>
        /// <returns>The list of submission histories.</returns>
        public async Task<IEnumerable<SubmissionHistory>> GetDailySubmissionHistories(ulong courseId,
                                                                                      DateTime date,
                                                                                      ulong graderId,
                                                                                      ulong assignmentId) {
            var response = await RawGetDailySubmissionHistories(courseId.ToString(),
                                                                date,
                                                                graderId.ToString(),
                                                                assignmentId.ToString());
            
            var models = await AccumulateDeserializePages<SubmissionHistoryModel>(response);
            return from m in models
                   select new SubmissionHistory(this, m);
        }

        [PaginatedResponse]
        private Task<HttpResponseMessage> RawGetSubmissionVersions(string courseId,
                                                                   string assignmentId,
                                                                   string userId,
                                                                   string ascending) {
            var url = $"courses/{courseId}/gradebook_history/feed";
            return client.GetAsync(url + BuildQueryString(("assignment_id", assignmentId), 
                                                           ("user_id", userId), 
                                                           ("ascending", ascending)
                                                           ));
        }

        /// <summary>
        /// Streams an uncollated list of submission versions for all matching submissions in the context.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="assignmentId">An optional assignment id to filter by.</param>
        /// <param name="userId">An optional user id to filter by.</param>
        /// <param name="ascending">Sorts the list in ascending order by date.</param>
        /// <returns>The stream of submission versions.</returns>
        /// <remarks>The returned objects will be missing the properties <c>NewGrade</c> and <c>PreviousGrade</c>.</remarks>
        public async IAsyncEnumerable<SubmissionVersion> StreamSubmissionVersions(ulong courseId,
                                                                                  ulong? assignmentId = null,
                                                                                  ulong? userId = null,
                                                                                  bool? ascending = null) {
            var response = await RawGetSubmissionVersions(courseId.ToString(),
                                                          assignmentId?.ToString(),
                                                          userId?.ToString(),
                                                          ascending?.ToShortString());

            await foreach (var model in StreamDeserializePages<SubmissionVersionModel>(response)) {
                yield return new SubmissionVersion(this, model);
            }
        }
    }
}