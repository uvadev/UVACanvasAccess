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
        
        [Flags]
        [PublicAPI]
        public enum IndividualLevelCourseIncludes : uint {
            [ApiRepresentation("syllabus_body")]
            SyllabusBody = 1 << 0,
            [ApiRepresentation("term")]
            Term = 1 << 1,
            [ApiRepresentation("course_progress")]
            CourseProgress = 1 << 2,
            [ApiRepresentation("storage_quota_used_mb")]
            StorageQuotaUsedMb = 1 << 3,
            [ApiRepresentation("total_students")]
            TotalStudents = 1 << 4,
            [ApiRepresentation("teachers")]
            Teachers = 1 << 5,
            [ApiRepresentation("account_name")]
            AccountName = 1 << 6,
            [ApiRepresentation("concluded")]
            Concluded = 1 << 7,
            [ApiRepresentation("all_courses")]
            AllCourses = 1 << 8,
            [ApiRepresentation("permissions")]
            Permissions = 1 << 9,
            [ApiRepresentation("observed_users")]
            ObservedUsers = 1 << 10,
            [ApiRepresentation("course_image")]
            CourseImage = 1 << 11,
            [ApiRepresentation("needs_grading_count")]
            NeedsGradingCount = 1 << 12,
            [ApiRepresentation("public_description")]
            PublicDescription = 1 << 13,
            [ApiRepresentation("total_scores")]
            TotalScores = 1 << 14,
            [ApiRepresentation("current_grading_period_scores")]
            CurrentGradingPeriodScores = 1 << 15,
            [ApiRepresentation("account")]
            Account = 1 << 16,
            [ApiRepresentation("sections")]
            Sections = 1 << 17,
            [ApiRepresentation("passback_status")]
            PassbackStatus = 1 << 18,
            [ApiRepresentation("favorites")]
            Favorites = 1 << 19,
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