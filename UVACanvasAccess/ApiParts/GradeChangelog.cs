using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UVACanvasAccess.Model.GradeChangelog;
using UVACanvasAccess.Structures.GradeChangelog;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.ApiParts {
    
    public partial class Api {

        private Task<HttpResponseMessage> RawGetAssignmentGradeChangelog(string assignmentId, 
                                                                         DateTime? start, 
                                                                         DateTime? end) {
            var url = $"audit/grade_change/assignments/{assignmentId}";
            return client.GetAsync(url + BuildQueryString(("start_time", start?.ToIso8601Date()), 
                                                           ("end_time", end?.ToIso8601Date())));
        }

        /// <summary>
        /// Streams the grade changelog for a single assignment.
        /// </summary>
        /// <param name="assignmentId">The assignment id.</param>
        /// <param name="start">The beginning of the range to search.</param>
        /// <param name="end">The end of the range to search.</param>
        /// <returns>The changelog.</returns>
        public async IAsyncEnumerable<GradeChangeEvent> StreamAssignmentGradeChangelog(ulong assignmentId,
                                                                                       DateTime? start = null, 
                                                                                       DateTime? end = null) {
            var response = await RawGetAssignmentGradeChangelog(assignmentId.ToString(), start, end);

            await foreach (var redundantModel in StreamDeserializeObjectPages<RedundantGradeChangeEventResponse>(response)) {
                foreach (var model in redundantModel.Events) {
                    yield return new GradeChangeEvent(this, model);
                }
            }
        }
        
        private Task<HttpResponseMessage> RawGetCourseGradeChangelog(string courseId, 
                                                                     DateTime? start, 
                                                                     DateTime? end) {
            var url = $"audit/grade_change/courses/{courseId}";
            return client.GetAsync(url + BuildQueryString(("start_time", start?.ToIso8601Date()), 
                                                           ("end_time", end?.ToIso8601Date())));
        }

        /// <summary>
        /// Streams the grade changelog for a single course.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="start">The beginning of the range to search.</param>
        /// <param name="end">The end of the range to search.</param>
        /// <returns>The changelog.</returns>
        public async IAsyncEnumerable<GradeChangeEvent> StreamCourseGradeChangelog(ulong courseId,
                                                                                   DateTime? start = null,
                                                                                   DateTime? end = null) {
            var response = await RawGetCourseGradeChangelog(courseId.ToString(), start, end);
            
            await foreach (var redundantModel in StreamDeserializeObjectPages<RedundantGradeChangeEventResponse>(response)) {
                foreach (var model in redundantModel.Events) {
                    yield return new GradeChangeEvent(this, model);
                }
            }
        }

        private Task<HttpResponseMessage> RawGetStudentGradeChangelog(string studentId,
                                                                      DateTime? start,
                                                                      DateTime? end) {
            var url = $"audit/grade_change/students/{studentId}";
            return client.GetAsync(url + BuildQueryString(("start_time", start?.ToIso8601Date()), 
                                                           ("end_time", end?.ToIso8601Date())));
        }

        /// <summary>
        /// Streams the grade changelog for a single student.
        /// </summary>
        /// <param name="studentId">The student id.</param>
        /// <param name="start">The beginning of the range to search.</param>
        /// <param name="end">The end of the range to search.</param>
        /// <returns>The changelog.</returns>
        public async IAsyncEnumerable<GradeChangeEvent> StreamStudentGradeChangelog(ulong studentId,
                                                                                    DateTime? start = null,
                                                                                    DateTime? end = null) {
            var response = await RawGetStudentGradeChangelog(studentId.ToString(), start, end);
            
            await foreach (var redundantModel in StreamDeserializeObjectPages<RedundantGradeChangeEventResponse>(response)) {
                foreach (var model in redundantModel.Events) {
                    yield return new GradeChangeEvent(this, model);
                }
            }
        }

        private Task<HttpResponseMessage> RawGetGraderGradeChangelog(string graderId,
                                                                     DateTime? start,
                                                                     DateTime? end) {
            var url = $"audit/grade_change/graders/{graderId}";
            return client.GetAsync(url + BuildQueryString(("start_time", start?.ToIso8601Date()), 
                                                           ("end_time", end?.ToIso8601Date())));
        }

        /// <summary>
        /// Streams the grade changelog for a single grader.
        /// </summary>
        /// <param name="graderId">The grader id.</param>
        /// <param name="start">The beginning of the range to search.</param>
        /// <param name="end">The end of the range to search.</param>
        /// <returns>The changelog.</returns>
        public async IAsyncEnumerable<GradeChangeEvent> StreamGraderGradeChangelog(ulong graderId,
                                                                                   DateTime? start = null,
                                                                                   DateTime? end = null) {
            var response = await RawGetGraderGradeChangelog(graderId.ToString(), start, end);
            
            await foreach (var redundantModel in StreamDeserializeObjectPages<RedundantGradeChangeEventResponse>(response)) {
                foreach (var model in redundantModel.Events) {
                    yield return new GradeChangeEvent(this, model);
                }
            }
        }
    }
}