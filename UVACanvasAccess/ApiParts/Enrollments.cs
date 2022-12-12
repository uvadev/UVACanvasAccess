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

            return client.PostAsync($"courses/{courseId}/enrollments", BuildHttpArguments(args));
        }

        /// <summary>
        /// The types of enrollment a user can have in a course, in terms of enrollment role.
        /// </summary>
        /// <remarks>
        /// These are distinct, in Canvas, from the values of <see cref="CourseEnrollmentTypes"/>.
        /// </remarks>
        [PublicAPI]
        public enum CourseEnrollmentRoleTypes {
            /// <summary>
            /// The user is a student.
            /// </summary>
            [ApiRepresentation("StudentEnrollment")]
            StudentEnrollment,
            /// <summary>
            /// The user is a teacher.
            /// </summary>
            [ApiRepresentation("TeacherEnrollment")]
            TeacherEnrollment,
            /// <summary>
            /// The user is a TA.
            /// </summary>
            [ApiRepresentation("TaEnrollment")]
            TaEnrollment,
            /// <summary>
            /// The user is an observer.
            /// </summary>
            [ApiRepresentation("ObserverEnrollment")]
            ObserverEnrollment,
            /// <summary>
            /// The user is a designer.
            /// </summary>
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
            Inactive,
            [ApiRepresentation("rejected")]
            Rejected,
            [ApiRepresentation("completed")]
            Completed
        }

        /// <summary>
        /// An alternate version of <see cref="CourseEnrollmentState"/> used by <see cref="Api.StreamUserCourses">StreamUserCourses()</see>.
        /// </summary>
        [PublicAPI]
        public enum CourseEnrollmentStateAlt {
            [ApiRepresentation("active")]
            Active,
            [ApiRepresentation("invited_or_pending")]
            InvitedOrPending,
            [ApiRepresentation("completed")]
            Completed
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
                                                       CourseEnrollmentRoleTypes enrollmentType,
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
            var response = await client.DeleteAsync($"courses/{courseId}/enrollments/{enrollmentId}" + 
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
            var response = await client.PostAsync($"courses/{courseId}/enrollments/{enrollmentId}/accept", null);

            return JObject.Parse(await response.Content.ReadAsStringAsync()).Value<bool>("success");
        }
        
        /// <summary>
        /// If the current user has a pending enrollment invitation, decline it.
        /// </summary>
        /// <param name="courseId">The course.</param>
        /// <param name="enrollmentId">The enrollment to accept.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public async Task<bool> DeclineEnrollmentInvitation(ulong courseId, ulong enrollmentId) {
            var response = await client.PostAsync($"courses/{courseId}/enrollments/{enrollmentId}/reject", null);

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
            var response = await client.PutAsync($"courses/{courseId}/enrollments/{enrollmentId}/reactivate", null);
            
            var model = JsonConvert.DeserializeObject<EnrollmentModel>(await response.Content.ReadAsStringAsync());
            return new Enrollment(this, model);
        }

        /// <summary>
        /// Categories of optional data that can be requested for inclusion within <see cref="Enrollment"/> objects.
        /// </summary>
        [PublicAPI]
        [Flags]
        public enum CourseEnrollmentIncludes : byte {
            /// <summary>
            /// Include avatar URLs.
            /// </summary>
            [ApiRepresentation("avatar_url")]
            AvatarUrl = 1 << 0,
            /// <summary>
            /// Include group ids.
            /// </summary>
            [ApiRepresentation("group_ids")]
            GroupIds = 1 << 1,
            /// <summary>
            /// Include locked status.
            /// </summary>
            [ApiRepresentation("locked")]
            Locked = 1 << 2,
            /// <summary>
            /// Include observation information.
            /// </summary>
            [ApiRepresentation("observed_users")]
            ObservedUsers = 1 << 3,
            /// <summary>
            /// Include whether the enrollment is removable.
            /// </summary>
            [ApiRepresentation("can_be_removed")]
            CanBeRemoved = 1 << 4,
            /// <summary>
            /// Include UUIDs.
            /// </summary>
            [ApiRepresentation("uuid")]
            Uuid = 1 << 5
        }

        /// <summary>
        /// Streams all enrollments for the course.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="types">(Optional) The set of enrollment types to filter by.</param>
        /// <param name="states">(Optional) The set of enrollment states to filter by.</param>
        /// <param name="includes">(Optional) Data to include in the result.</param>
        /// <param name="userId">(Optional) The user id to filter by.</param>
        /// <param name="gradingPeriodId">(Optional) The grading period id to filter by.</param>
        /// <param name="enrollmentTermId">(Optional) The enrollment term id to filter by. Cannot be used with <c>enrollmentTermSisId</c>.</param>
        /// <param name="enrollmentTermSisId">(Optional) The enrollment term SIS id to filter by. Cannot be used with <c>enrollmentTermId</c>.</param>
        /// <param name="sisAccountIds">(Optional) The set of account SIS ids to filter by.</param>
        /// <param name="sisCourseIds">(Optional) The set of course SIS ids to filter by.</param>
        /// <param name="sisSectionIds">(Optional) The set of section SIS ids to filter by.</param>
        /// <param name="sisUserIds">(Optional) The set of user SIS ids to filter by.</param>
        /// <param name="createdForSisId">(Optional) If using <c>sisUserIds</c>, restrict the filtering per SIS id to
        /// only include enrollments made for that exact SIS id. This is relevant when a user has multiple SIS ids.</param>
        /// <returns>The stream of enrollments.</returns>
        public async IAsyncEnumerable<Enrollment> StreamCourseEnrollments(ulong courseId,
                                                                          IEnumerable<CourseEnrollmentRoleTypes> types = null,
                                                                          IEnumerable<CourseEnrollmentState> states = null,
                                                                          CourseEnrollmentIncludes? includes = null,
                                                                          ulong? userId = null,
                                                                          ulong? gradingPeriodId = null,
                                                                          ulong? enrollmentTermId = null,
                                                                          string enrollmentTermSisId = null,
                                                                          IEnumerable<string> sisAccountIds = null,
                                                                          IEnumerable<string> sisCourseIds = null,
                                                                          IEnumerable<string> sisSectionIds = null,
                                                                          IEnumerable<string> sisUserIds = null,
                                                                          IEnumerable<bool> createdForSisId = null) {
            var args = new List<(string, string)> {
                ("user_id", userId?.ToString()),
                ("grading_period_id", gradingPeriodId?.ToString())
            };

            if (enrollmentTermId != null) {
                args.Add(("enrollment_term_id", enrollmentTermId!.ToString()));
            } else if (enrollmentTermSisId != null) {
                args.Add(("enrollment_term_id", $"sis_term_id:{enrollmentTermSisId}"));
            }

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

            if (sisAccountIds != null) {
                args.AddRange(sisAccountIds.Select(id => ("sis_account_id[]", id)));
            }
            
            if (sisCourseIds != null) {
                args.AddRange(sisCourseIds.Select(id => ("sis_course_id[]", id)));
            }
            
            if (sisSectionIds != null) {
                args.AddRange(sisSectionIds.Select(id => ("sis_section_id[]", id)));
            }
            
            if (sisUserIds != null) {
                args.AddRange(sisUserIds.Select(id => ("sis_user_id[]", id)));
            }
            
            if (createdForSisId != null) {
                args.AddRange(createdForSisId.Select(id => ("created_for_sis_id[]", id.ToShortString())));
            }
            
            
            var response = await client.GetAsync($"courses/{courseId}/enrollments" + BuildDuplicateKeyQueryString(args.ToArray()));

            await foreach (var model in StreamDeserializePages<EnrollmentModel>(response)) {
                yield return new Enrollment(this, model);
            }
        }

        /// <summary>
        /// Streams all enrollments for the user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="types">(Optional) The set of enrollment types to filter by.</param>
        /// <param name="states">(Optional) The set of enrollment states to filter by.</param>
        /// <param name="includes">(Optional) Data to include in the result.</param>
        /// <param name="gradingPeriodId">(Optional) The grading period id to filter grades by.</param>
        /// <param name="enrollmentTermId">(Optional) The enrollment term id to filter by. Cannot be used with <c>enrollmentTermSisId</c>.</param>
        /// <param name="enrollmentTermSisId">(Optional) The enrollment term SIS id to filter by. Cannot be used with <c>enrollmentTermId</c>.</param>
        /// <param name="sisAccountIds">(Optional) The set of account SIS ids to filter by.</param>
        /// <param name="sisCourseIds">(Optional) The set of course SIS ids to filter by.</param>
        /// <param name="sisSectionIds">(Optional) The set of section SIS ids to filter by.</param>
        /// <param name="sisUserIds">(Optional) The set of user SIS ids to filter by.</param>
        /// <param name="createdForSisId">(Optional) If using <c>sisUserIds</c>, restrict the filtering per SIS id to
        /// only include enrollments made for that exact SIS id. This is relevant when a user has multiple SIS ids.</param>
        /// <returns>The stream of enrollments.</returns>
        public async IAsyncEnumerable<Enrollment> StreamUserEnrollments(ulong userId,
                                                                        IEnumerable<CourseEnrollmentRoleTypes> types = null,
                                                                        IEnumerable<CourseEnrollmentState> states = null,
                                                                        CourseEnrollmentIncludes? includes = null,
                                                                        ulong? gradingPeriodId = null,
                                                                        ulong? enrollmentTermId = null,
                                                                        string enrollmentTermSisId = null,
                                                                        IEnumerable<string> sisAccountIds = null,
                                                                        IEnumerable<string> sisCourseIds = null,
                                                                        IEnumerable<string> sisSectionIds = null,
                                                                        IEnumerable<string> sisUserIds = null,
                                                                        IEnumerable<bool> createdForSisId = null) {
            var args = new List<(string, string)> {
                ("per_page", "250")
            };
            
            if (enrollmentTermId != null) {
                args.Add(("enrollment_term_id", enrollmentTermId!.ToString()));
            } else if (enrollmentTermSisId != null) {
                args.Add(("enrollment_term_id", $"sis_term_id:{enrollmentTermSisId}"));
            }
            
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

            if (gradingPeriodId != null) {
                args.Add(("grading_period_id", gradingPeriodId.ToString()));
            }
            
            if (sisAccountIds != null) {
                args.AddRange(sisAccountIds.Select(id => ("sis_account_id[]", id)));
            }
            
            if (sisCourseIds != null) {
                args.AddRange(sisCourseIds.Select(id => ("sis_course_id[]", id)));
            }
            
            if (sisSectionIds != null) {
                args.AddRange(sisSectionIds.Select(id => ("sis_section_id[]", id)));
            }
            
            if (sisUserIds != null) {
                args.AddRange(sisUserIds.Select(id => ("sis_user_id[]", id)));
            }
            
            if (createdForSisId != null) {
                args.AddRange(createdForSisId.Select(id => ("created_for_sis_id[]", id.ToShortString())));
            }
            
            var response = await client.GetAsync($"users/{userId}/enrollments" + BuildDuplicateKeyQueryString(args.ToArray()));

            await foreach (var model in StreamDeserializePages<EnrollmentModel>(response)) {
                yield return new Enrollment(this, model);
            }
        }
    }
}