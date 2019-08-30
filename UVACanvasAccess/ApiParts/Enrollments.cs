using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UVACanvasAccess.Model.Enrollments;
using UVACanvasAccess.Structures.Enrollments;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {

        private Task<HttpResponseMessage> RawCreateEnrollment(ulong courseId, 
                                                              ulong userId, 
                                                              string enrollmentType,
                                                              ulong? roleId,
                                                              string enrollmentState,
                                                              ulong? courseSectionId,
                                                              bool? limitPrivilegesToSection,
                                                              bool? notify,
                                                              string selfEnrollmentCode,
                                                              bool? selfEnrolled,
                                                              ulong? associatedUserId) {
            var args = new[] {
                                 ("user_id", userId.ToString()),
                                 ("type", enrollmentType),
                                 ("role_id", roleId?.ToString()),
                                 ("enrollment_state", enrollmentState),
                                 ("course_section_id", courseSectionId?.ToString()),
                                 ("limit_privileges_to_course_section", limitPrivilegesToSection?.ToShortString()),
                                 ("notify", notify?.ToShortString()),
                                 ("self_enrollment_code", selfEnrollmentCode),
                                 ("self_enrolled", selfEnrolled?.ToShortString()),
                                 ("associated_user_id", associatedUserId?.ToString())
                             }.KeySelect(k => $"enrollment[{k}]");

            return _client.PostAsync($"courses/{courseId}/enrollments", BuildHttpArguments(args));
        }

        /// <summary>
        /// The types of enrollment a user can have in a course.
        /// </summary>
        [PublicAPI]
        public enum CourseEnrollmentType {
            [ApiRepresentation("StudentEnrollment")]
            StudentEnrollment,
            [ApiRepresentation("TeacherEnrollment")]
            TeacherEnrollment,
            [ApiRepresentation("TaEnrollment")]
            TaEnrollment,
            [ApiRepresentation("ObserverEnrollment")]
            ObserverEnrollment,
            [ApiRepresentation("DesignerEnrollment")]
            DesignerEnrollment
        }

        /// <summary>
        /// The states of enrollment a user can have in a course.
        /// </summary>
        [PublicAPI]
        public enum CourseEnrollmentState {
            /// <summary>
            /// The user can participate in the course.
            /// </summary>
            [ApiRepresentation("active")]
            Active,
            /// <summary>
            /// The user has been sent a course invitation, and will become <see cref="Active"/> once they accept.
            /// </summary>
            [ApiRepresentation("invited")]
            Invited,
            /// <summary>
            /// The student appears in the course roster, but is unable to participate in the course.
            /// </summary>
            [ApiRepresentation("inactive")]
            Inactive
        }

        /// <summary>
        /// Enrolls a user in a course.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="enrollmentType">The enrollment type.</param>
        /// <param name="roleId">An optional course-level role to assign to the user.</param>
        /// <param name="enrollmentState">
        /// The enrollment state. <see cref="CourseEnrollmentState.Invited"/> by default. <br/>
        /// If <see cref="CourseEnrollmentState.Invited"/>, the user will be sent a course invite.
        /// </param>
        /// <param name="courseSectionId">Optionally, the course section to enroll in.</param>
        /// <param name="limitPrivilegesToSection">Optionally, only allow this user to interact with users in the same section.</param>
        /// <param name="notify">Whether or not this user should be notified to changes in the course. Disabled by default.</param>
        /// <param name="selfEnrollmentCode">If self-enrolling, the self-enrollment code.</param>
        /// <param name="selfEnrolled">Whether or not this is a self-enrollment.</param>
        /// <param name="associatedUserId">If this is an observer enrollment, the id of the target student.</param>
        /// <returns>The new enrollment.</returns>
        public async Task<Enrollment> CreateEnrollment(ulong courseId,
                                                       ulong userId,
                                                       CourseEnrollmentType enrollmentType,
                                                       ulong? roleId = null,
                                                       CourseEnrollmentState? enrollmentState = null,
                                                       ulong? courseSectionId = null,
                                                       bool? limitPrivilegesToSection = null,
                                                       bool? notify = null,
                                                       string selfEnrollmentCode = null,
                                                       bool? selfEnrolled = null,
                                                       ulong? associatedUserId = null) {
            
            var response = await RawCreateEnrollment(courseId,
                                                     userId,
                                                     enrollmentType.GetApiRepresentation(),
                                                     roleId,
                                                     enrollmentState?.GetApiRepresentation(),
                                                     courseSectionId,
                                                     limitPrivilegesToSection,
                                                     notify,
                                                     selfEnrollmentCode,
                                                     selfEnrolled,
                                                     associatedUserId);
            var model = JsonConvert.DeserializeObject<EnrollmentModel>(await response.Content.ReadAsStringAsync());
            return new Enrollment(this, model);
        }

        private async Task<Enrollment> RawDeleteEnrollment(ulong courseId, ulong enrollmentId, string task) {
            var response = await _client.DeleteAsync($"courses/{courseId}/enrollments/{enrollmentId}" + 
                                                     BuildQueryString(("task", task))).AssertSuccess();

            var model = JsonConvert.DeserializeObject<EnrollmentModel>(await response.Content.ReadAsStringAsync());
            return new Enrollment(this, model);
        }

        /// <summary>
        /// Concludes an enrollment without deleting it. <br/>
        /// This is the same action that occurs automatically when the user reaches the end of their time in the course,
        /// such as at the end of the school year.
        /// </summary>
        /// <param name="courseId">The course.</param>
        /// <param name="enrollmentId">The enrollment.</param>
        /// <returns>The concluded enrollment.</returns>
        public Task<Enrollment> ConcludeEnrollment(ulong courseId, ulong enrollmentId) {
            return RawDeleteEnrollment(courseId, enrollmentId, "conclude");
        }
        
        /// <summary>
        /// Irrecoverably deletes an enrollment.
        /// </summary>
        /// <param name="courseId">The course.</param>
        /// <param name="enrollmentId">The enrollment.</param>
        /// <returns>The deleted enrollment.</returns>
        public Task<Enrollment> DeleteEnrollment(ulong courseId, ulong enrollmentId) {
            return RawDeleteEnrollment(courseId, enrollmentId, "delete");
        }
        
        /// <summary>
        /// Sets an enrollment to <see cref="CourseEnrollmentState.Inactive"/>.
        /// </summary>
        /// <param name="courseId">The course.</param>
        /// <param name="enrollmentId">The enrollment.</param>
        /// <returns>The inactivated enrollment.</returns>
        public Task<Enrollment> DeactivateEnrollment(ulong courseId, ulong enrollmentId) {
            return RawDeleteEnrollment(courseId, enrollmentId, "deactivate");
        }

        /// <summary>
        /// If the current user has a pending enrollment invitation, accepts it.
        /// </summary>
        /// <param name="courseId">The course.</param>
        /// <param name="enrollmentId">The enrollment to accept.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public async Task<bool> AcceptEnrollmentInvitation(ulong courseId, ulong enrollmentId) {
            var response = await _client.PostAsync($"courses/{courseId}/enrollments/{enrollmentId}/accept", null);

            return JObject.Parse(await response.Content.ReadAsStringAsync()).Value<bool>("success");
        }
        
        /// <summary>
        /// If the current user has a pending enrollment invitation, decline it.
        /// </summary>
        /// <param name="courseId">The course.</param>
        /// <param name="enrollmentId">The enrollment to accept.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public async Task<bool> DeclineEnrollmentInvitation(ulong courseId, ulong enrollmentId) {
            var response = await _client.PostAsync($"courses/{courseId}/enrollments/{enrollmentId}/reject", null);

            return JObject.Parse(await response.Content.ReadAsStringAsync()).Value<bool>("success");
        }

        /// <summary>
        /// <see cref="CourseEnrollmentState.Active">Reactivate</see> an enrollment that is
        /// <see cref="CourseEnrollmentState.Inactive">inactive</see>.
        /// </summary>
        /// <param name="courseId">The course.</param>
        /// <param name="enrollmentId">The enrollment.</param>
        /// <returns>The reactivated enrollment.</returns>
        public async Task<Enrollment> ReactivateEnrollment(ulong courseId, ulong enrollmentId) {
            var response = await _client.PutAsync($"courses/{courseId}/enrollments/{enrollmentId}/reactivate", null);
            
            var model = JsonConvert.DeserializeObject<EnrollmentModel>(await response.Content.ReadAsStringAsync());
            return new Enrollment(this, model);
        }

        [PublicAPI]
        [Flags]
        public enum CourseEnrollmentIncludes : byte {
            [ApiRepresentation("avatar_url")]
            AvatarUrl = 1 << 0,
            [ApiRepresentation("group_ids")]
            GroupIds = 1 << 1,
            [ApiRepresentation("locked")]
            Locked = 1 << 2,
            [ApiRepresentation("observed_users")]
            ObservedUsers = 1 << 3,
            [ApiRepresentation("can_be_removed")]
            CanBeRemoved = 1 << 4,
            [ApiRepresentation("uuid")]
            Uuid = 1 << 5
        }
        
        /// <summary>
        /// Streams all enrollments for the course.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="types">Optionally, the set of enrollment types to filter by.</param>
        /// <param name="states">Optionally, the set of enrollment states to filter by.</param>
        /// <param name="includes">Optional data to include in the result.</param>
        /// <returns>The stream of enrollments.</returns>
        // todo params
        // todo {types, states} should probably be flags
        public async IAsyncEnumerable<Enrollment> StreamCourseEnrollments(ulong courseId,
                                                                          IEnumerable<CourseEnrollmentType> types = null,
                                                                          IEnumerable<CourseEnrollmentState> states = null,
                                                                          CourseEnrollmentIncludes? includes = null) {
            var args = new List<(string, string)>();
            if (types != null) {
                args.AddRange(types.Select(t => t.GetApiRepresentation())
                                   .Select(a => ("type[]", a)));
            }
            if (states != null) {
                args.AddRange(states.Select(s => s.GetApiRepresentation())
                                    .Select(a => ("state[]", a)));
            }
            if (includes != null) {
                args.AddRange(includes.GetFlagsApiRepresentations()
                                      .Select(a => ("include[]", a)));
            }
            
            var response = await _client.GetAsync($"courses/{courseId}/enrollments" + BuildDuplicateKeyQueryString(args.ToArray()));

            await foreach (var model in StreamDeserializePages<EnrollmentModel>(response)) {
                yield return new Enrollment(this, model);
            }
        }

        /// <summary>
        /// Streams all enrollments for the user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="types">Optionally, the set of enrollment types to filter by.</param>
        /// <param name="states">Optionally, the set of enrollment states to filter by.</param>
        /// <param name="includes">Optional data to include in the result.</param>
        /// <returns>The stream of enrollments.</returns>
        // todo params
        // todo {types, states} should probably be flags
        public async IAsyncEnumerable<Enrollment> StreamUserEnrollments(ulong userId,
                                                                        IEnumerable<CourseEnrollmentType> types = null,
                                                                        IEnumerable<CourseEnrollmentState> states = null,
                                                                        CourseEnrollmentIncludes? includes = null) {
            var args = new List<(string, string)>();
            if (types != null) {
                args.AddRange(types.Select(t => t.GetApiRepresentation())
                                   .Select(a => ("type[]", a)));
            }
            if (states != null) {
                args.AddRange(states.Select(s => s.GetApiRepresentation())
                                    .Select(a => ("state[]", a)));
            }
            if (includes != null) {
                args.AddRange(includes.GetFlagsApiRepresentations()
                                      .Select(a => ("include[]", a)));
            }
            
            var response = await _client.GetAsync($"users/{userId}/enrollments" + BuildDuplicateKeyQueryString(args.ToArray()));

            await foreach (var model in StreamDeserializePages<EnrollmentModel>(response)) {
                yield return new Enrollment(this, model);
            }
        }
    }
}