using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UVACanvasAccess.Builders;
using UVACanvasAccess.Model.Courses;
using UVACanvasAccess.Structures.Courses;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {
        
        /// <summary>
        /// Categories of optional data that can be included with <see cref="Course"/> objects, when fetched
        /// individually by ID.
        /// </summary>
        /// <remarks>These options are generally unavailable when fetching courses in bulk, such as by
        /// <see cref="Api.StreamCourses"/>.</remarks>
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
    }
}