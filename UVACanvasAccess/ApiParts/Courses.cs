using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UVACanvasAccess.Builders;
using UVACanvasAccess.Model.Courses;
using UVACanvasAccess.Model.Users;
using UVACanvasAccess.Structures.Courses;
using UVACanvasAccess.Structures.Users;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {
        
        /// <summary>
        /// Categories of optional data that can be included with <see cref="Course"/> objects, when fetched
        /// individually by ID.
        /// </summary>
        /// <remarks>These options are generally unavailable when fetching courses in bulk, such as by
        /// <see cref="Api.StreamCourses">StreamCourses()</see>.</remarks>
        [Flags]
        [PublicAPI]
        public enum IndividualLevelCourseIncludes : uint {
            /// <summary>
            /// Include the syllabus body.
            /// </summary>
            [ApiRepresentation("syllabus_body")]
            SyllabusBody = 1 << 0,
            /// <summary>
            /// Include the course term.
            /// </summary>
            [ApiRepresentation("term")]
            Term = 1 << 1,
            /// <summary>
            /// Include the course progress.
            /// </summary>
            [ApiRepresentation("course_progress")]
            CourseProgress = 1 << 2,
            /// <summary>
            /// Include the amount of storage quota this course has used.
            /// </summary>
            [ApiRepresentation("storage_quota_used_mb")]
            StorageQuotaUsedMb = 1 << 3,
            /// <summary>
            /// Include the total number of students in the course.
            /// </summary>
            [ApiRepresentation("total_students")]
            TotalStudents = 1 << 4,
            /// <summary>
            /// Include the teachers of the course.
            /// </summary>
            [ApiRepresentation("teachers")]
            Teachers = 1 << 5,
            /// <summary>
            /// Include the name of the account or subaccount the course is associated with.
            /// </summary>
            [ApiRepresentation("account_name")]
            AccountName = 1 << 6,
            /// <summary>
            /// Include whether the course is concluded.
            /// </summary>
            [ApiRepresentation("concluded")]
            Concluded = 1 << 7,
            /// <summary>
            /// Include related courses.
            /// </summary>
            [ApiRepresentation("all_courses")]
            AllCourses = 1 << 8,
            /// <summary>
            /// Include course permissions.
            /// </summary>
            [ApiRepresentation("permissions")]
            Permissions = 1 << 9,
            /// <summary>
            /// Include observed users.
            /// </summary>
            [ApiRepresentation("observed_users")]
            ObservedUsers = 1 << 10,
            /// <summary>
            /// Include the course image.
            /// </summary>
            [ApiRepresentation("course_image")]
            CourseImage = 1 << 11,
            /// <summary>
            /// Include the count of how many assignments need grading.
            /// </summary>
            [ApiRepresentation("needs_grading_count")]
            NeedsGradingCount = 1 << 12,
            /// <summary>
            /// Include the course's public description.
            /// </summary>
            [ApiRepresentation("public_description")]
            PublicDescription = 1 << 13,
            /// <summary>
            /// Include the course's overall scores.
            /// </summary>
            [ApiRepresentation("total_scores")]
            TotalScores = 1 << 14,
            /// <summary>
            /// Include the course's scores for the current grading period.
            /// </summary>
            [ApiRepresentation("current_grading_period_scores")]
            CurrentGradingPeriodScores = 1 << 15,
            /// <summary>
            /// Include the account or subaccount the course is associated with.
            /// </summary>
            [ApiRepresentation("account")]
            Account = 1 << 16,
            /// <summary>
            /// Include the course's sections.
            /// </summary>
            [ApiRepresentation("sections")]
            Sections = 1 << 17,
            /// <summary>
            /// Include the course's passback status.
            /// </summary>
            [ApiRepresentation("passback_status")]
            PassbackStatus = 1 << 18,
            /// <summary>
            /// Include the course's favorites.
            /// </summary>
            [ApiRepresentation("favorites")]
            Favorites = 1 << 19,
            /// <summary>
            /// Include all possible data.
            /// </summary>
            Everything = uint.MaxValue
        }

        private Task<HttpResponseMessage> RawGetCourse(string accountId,
                                                       string courseId,
                                                       IndividualLevelCourseIncludes? includes,
                                                       uint? teacherLimit) {
            var url = $"accounts/{accountId}/courses/{courseId}";

            var args = new List<(string, string)> {
                                                      ("teacher_limit", teacherLimit?.ToString())
                                                  };

            includes?.GetFlagsApiRepresentations()
                     .Select(r => ("include[]", r))
                     .Peek(t => args.Add(t));

            return client.GetAsync(url + BuildQueryString(args.ToArray()));
        }

        /// <summary>
        /// Get a single course by id.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="accountId">(Optional) The account id.</param>
        /// <param name="includes">Additional data to include.</param>
        /// <param name="teacherLimit"></param>
        /// <returns></returns>
        public async Task<Course> GetCourse(ulong courseId,
                                            ulong? accountId = null,
                                            IndividualLevelCourseIncludes? includes = null,
                                            uint? teacherLimit = null) {
            var response = await RawGetCourse(accountId?.ToString() ?? "self", courseId.ToString(), includes, teacherLimit);

            var model = JsonConvert.DeserializeObject<CourseModel>(await response.AssertSuccess().Content.ReadAsStringAsync());
            
            return new Course(this, model);
        }

        internal async Task<Course> PostCreateCourse(CourseBuilder builder) {
            var url = $"accounts/{builder.AccountId.IdOrSelf()}/courses";
            var args = BuildHttpArguments(builder.Fields.Select(kv => (kv.Key, kv.Value)));

            var response = await client.PostAsync(url, args);

            var model = JsonConvert.DeserializeObject<CourseModel>(await response.Content.ReadAsStringAsync());
            return new Course(this, model);
        }
        
        internal async Task<Course> PutEditCourse(ulong id, CourseBuilder builder) {
            var url = $"courses/{id}";
            var args = BuildHttpArguments(builder.Fields.Select(kv => (kv.Key, kv.Value)));

            var response = await client.PutAsync(url, args);

            var model = JsonConvert.DeserializeObject<CourseModel>(await response.Content.ReadAsStringAsync());
            return new Course(this, model);
        }
        
        /// <summary>
        /// Return a new <see cref="Builders.CourseBuilder"/>.
        /// </summary>
        /// <param name="accountId">(Optional) The account id.</param>
        /// <returns>The course builder.</returns>
        public CourseBuilder CreateCourse(ulong? accountId = null) {
            return new CourseBuilder(this, false, accountId);
        }

        /// <summary>
        /// Return a new <see cref="Builders.CourseBuilder"/> for editing.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="accountId">(Optional) The account id.</param>
        /// <returns>The course builder.</returns>
        public CourseBuilder EditCourse(ulong courseId, ulong? accountId = null) {
            return new CourseBuilder(this, true, accountId, courseId);
        }

        private Task<HttpResponseMessage> RawDeleteCourse(string id, [NotNull] string action) {
            return client.DeleteAsync($"courses/{id}" + BuildQueryString(("event", action)));
        }

        /// <summary>
        /// Delete a course.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <returns>The deleted course.</returns>
        public async Task DeleteCourse(ulong courseId) {
            await RawDeleteCourse(courseId.ToString(), "delete").AssertSuccess();
        }
        
        /// <summary>
        /// End a course without deleting it.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <returns>The concluded course.</returns>
        public async Task ConcludeCourse(ulong courseId) {
            await RawDeleteCourse(courseId.ToString(), "conclude").AssertSuccess();
        }

        /// <summary>
        /// Get a course's settings.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <returns>The course settings.</returns>
        public async Task<CourseSettings> GetCourseSettings(ulong courseId) {
            var response = await client.GetAsync($"courses/{courseId}/settings");
            var model = JsonConvert.DeserializeObject<CourseSettingsModel>(await response.Content.ReadAsStringAsync());
            return new CourseSettings(model);
        }

        /// <summary>
        /// Update a course's settings.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="cs">The new settings.</param>
        /// <returns>A void task.</returns>
        public async Task UpdateCourseSettings(ulong courseId, CourseSettings cs) {
            await client.PutAsync($"courses/{courseId}/settings", BuildHttpArguments(cs.GetTuples()));
        }

        /// <summary>
        /// Streams the list of users enrolled in a course. <br/> All arguments except for <c>courseId</c> are optional,
        /// and serve to narrow (or broaden) the results.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="searchTerm">(Optional) The string or numeric id to search by.</param>
        /// <param name="enrollmentTypes">(Optional) The enrollment types to filter by.</param>
        /// <param name="enrollmentRole">(Optional) The role id to filter by.</param>
        /// <param name="includes">Optional data to include with the results. See <see cref="CourseUserIncludes"/>.</param>
        /// <param name="userIds">(Optional) User ids to filter by.</param>
        /// <param name="enrollmentStates">(Optional) The enrollment states to filter by.</param>
        /// <returns>The stream of users.</returns>
        public async IAsyncEnumerable<User> StreamCourseUsers(ulong courseId, 
                                                              string searchTerm = null,
                                                              IEnumerable<CourseEnrollmentTypes> enrollmentTypes = null,
                                                              ulong? enrollmentRole = null,
                                                              CourseUserIncludes? includes = null,
                                                              IEnumerable<ulong> userIds = null,
                                                              IEnumerable<CourseEnrollmentState> enrollmentStates = null) {
            var args = new List<(string, string)> {
                ("search_term", searchTerm),
                ("enrollment_role_id", enrollmentRole?.ToString())
            };

            if (enrollmentTypes != null) {
                args.AddRange(enrollmentTypes.Select(et => ("enrollment_type[]", et.GetApiRepresentation())));
            }
            
            if (enrollmentStates != null) {
                args.AddRange(enrollmentStates.Select(et => ("enrollment_state[]", et.GetApiRepresentation())));
            }

            if (includes != null) {
                args.AddRange(includes!.GetFlagsApiRepresentations().Select(f => ("include[]", f)));
            }

            if (userIds != null) {
                args.AddRange(userIds.Select(u => ("user_ids[]", u.ToString())));
            }

            var response = await client.GetAsync($"courses/{courseId}/users" + BuildDuplicateKeyQueryString(args.ToArray()));

            await foreach (var model in StreamDeserializePages<UserModel>(response)) {
                yield return new User(this, model);
            }
        }

        /// <summary>
        /// Streams the list of courses a user is enrolled in. All arguments except <c>userId</c> are optional,
        /// and serve to narrow (or broaden) the results.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="includes">Optional data to include with the results. See <see cref="IndividualLevelCourseIncludes"/>.</param>
        /// <param name="courseStates">(Optional) The course states to filter by.</param>
        /// <param name="enrollmentState">(Optional) The enrollment state to filter by.</param>
        /// <param name="homeroomCoursesOnly">(Optional) If true, only return homeroom courses.</param>
        /// <returns></returns>
        public async IAsyncEnumerable<Course> StreamUserCourses(ulong userId, 
                                                                IndividualLevelCourseIncludes? includes = null,
                                                                CourseStatesAlt? courseStates = null,
                                                                CourseEnrollmentStateAlt? enrollmentState = null,
                                                                bool? homeroomCoursesOnly = null) {
            var args = new List<(string, string)> {
                ("homeroom", homeroomCoursesOnly?.ToShortString()),
                ("enrollment_state", enrollmentState?.GetApiRepresentation())
            };
            
            if (includes != null) {
                args.AddRange(includes!.GetFlagsApiRepresentations().Select(f => ("include[]", f)));
            }

            if (courseStates != null) {
                args.AddRange(courseStates!.GetFlagsApiRepresentations().Select(f => ("state[]", f)));
            }
            
            var response = await client.GetAsync($"users/{userId}/courses" + BuildDuplicateKeyQueryString(args.ToArray()));

            await foreach (var model in StreamDeserializePages<CourseModel>(response)) {
                yield return new Course(this, model);
            }
        }

        /// <summary>
        /// Streams the list of users enrolled in a course who have recently logged in.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <returns>The stream of users, sorted by <see cref="User.LastLogin"/>.</returns>
        public async IAsyncEnumerable<User> StreamRecentCourseUsers(ulong courseId) {
            var response = await client.GetAsync($"courses/{courseId}/recent_students" + BuildQueryString());

            await foreach (var model in StreamDeserializePages<UserModel>(response)) {
                yield return new User(this, model);
            }
        }

        /// <summary>
        /// Returns the test student associated with a course.
        /// If the course does not already have a test student, a new one is created.
        /// The test student is used for the 'Student View' function on the webpage.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <returns>The test student.</returns>
        public async Task<User> GetCourseTestStudent(ulong courseId) {
            var response = await client.GetAsync($"courses/{courseId}/student_view_student" + BuildQueryString());
            var model = JsonConvert.DeserializeObject<UserModel>(await response.Content.ReadAsStringAsync());
            return new User(this, model);
        }

        /// <summary>
        /// Returns progress information for a user in a course.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="userId">The user id. Defaults to 'self'.</param>
        /// <returns>The course progress.</returns>
        public async Task<CourseProgress> GetUserCourseProgress(ulong courseId, ulong? userId = null) {
            var response = await client.GetAsync($"courses/{courseId}/users/{userId.IdOrSelf()}/progress" + BuildQueryString());
            var model = JsonConvert.DeserializeObject<CourseProgressModel>(await response.Content.ReadAsStringAsync());
            return new CourseProgress(this, model);
        }

        /// <summary>
        /// Optional data that can be included with results from <see cref="Api.StreamCourseUsers">StreamCourseUsers()</see>.
        /// </summary>
        [PublicAPI]
        [Flags]
        public enum CourseUserIncludes {
            /// <summary>
            /// Include each user's <see cref="Structures.Enrollments.Enrollment">course enrollment</see>.
            /// </summary>
            [ApiRepresentation("enrollments")]
            Enrollments = 1,
            /// <summary>
            /// Include whether the enrollment is locked.
            /// </summary>
            [ApiRepresentation("locked")]
            Locked = 1 << 1,
            /// <summary>
            /// Include each user's avatar url.
            /// </summary>
            [ApiRepresentation("avatar_url")]
            AvatarUrl = 1 << 2,
            /// <summary>
            /// Include the course's test student with the overall list.
            /// </summary>
            [ApiRepresentation("test_student")]
            TestStudent = 1 << 3,
            /// <summary>
            /// Include each user's bio.
            /// </summary>
            [ApiRepresentation("bio")]
            Bio = 1 << 4,
            /// <summary>
            /// Include each user's custom links supplied by plugins.
            /// </summary>
            [ApiRepresentation("custom_links")]
            CustomLinks = 1 << 5,
            /// <summary>
            /// Depends on <see cref="CourseUserIncludes.Enrollments"/>. Modifies the grades and scores returned with
            /// the enrollment objects to be for the current grading period.
            /// </summary>
            [ApiRepresentation("current_grading_period_scores")]
            CurrentGradingPeriodScores = 1 << 6,
            /// <summary>
            /// Include each user's UUID.
            /// </summary>
            [ApiRepresentation("uuid")]
            Uuid = 1 << 7
        }
    }
}