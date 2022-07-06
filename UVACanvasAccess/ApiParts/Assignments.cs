using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UVACanvasAccess.Builders;
using UVACanvasAccess.Model.Assignments;
using UVACanvasAccess.Model.Submissions;
using UVACanvasAccess.Structures.Assignments;
using UVACanvasAccess.Structures.Submissions;
using UVACanvasAccess.Structures.Submissions.NewSubmission;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.ApiParts {
    
    public partial class Api {
        
        /// <summary>
        /// Lists the assignment overrides for a given assignment.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="assignmentId">The assignment id.</param>
        /// <returns>The list of overrides.</returns>
        public async Task<IEnumerable<AssignmentOverride>> ListAssignmentOverrides(ulong courseId, ulong assignmentId) {
            var response = await client.GetAsync($"courses/{courseId}/assignments/{assignmentId}/overrides");

            var models = await AccumulateDeserializePages<AssignmentOverrideModel>(response);

            return from model in models
                   select new AssignmentOverride(this, model);
        }

        /// <summary>
        /// Streams the assignment overrides for a given assignment.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="assignmentId">The assignment id.</param>
        /// <returns>The stream of overrides.</returns>
        public async IAsyncEnumerable<AssignmentOverride> StreamAssignmentOverrides(ulong courseId, ulong assignmentId) {
            var response = await client.GetAsync($"courses/{courseId}/assignments/{assignmentId}/overrides");

            await foreach (var model in StreamDeserializePages<AssignmentOverrideModel>(response)) {
                yield return new AssignmentOverride(this, model);
            }
        }

        /// <summary>
        /// Gets an <see cref="AssignmentOverride"/> by the course, assignment, and override IDs.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="assignmentId">The assignment id.</param>
        /// <param name="overrideId">The override id.</param>
        /// <returns>The <see cref="AssignmentOverride"/>.</returns>
        public async Task<AssignmentOverride> GetAssignmentOverride(ulong courseId, 
                                                                    ulong assignmentId,
                                                                    ulong overrideId) {
            var response =
                await client.GetAsync($"courses/{courseId}/assignments/{assignmentId}/overrides/{overrideId}");
            response.AssertSuccess();

            var model =
                JsonConvert.DeserializeObject<AssignmentOverrideModel>(await response.Content.ReadAsStringAsync());
            return new AssignmentOverride(this, model);
        }

        /// <summary>
        /// Gets a collection of <see cref="AssignmentOverride"/> objects, given a course id, a set of override ids,
        /// and the assignments they belong to.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="assignmentsToOverrides">A mapping of assignment ids to override ids.</param>
        /// <returns>The collection of <see cref="AssignmentOverride"/>s.</returns>
        public async Task<IEnumerable<AssignmentOverride>> BatchGetAssignmentOverrides(ulong courseId,
                                                                                       ILookup<ulong, ulong> assignmentsToOverrides) {
            var args = assignmentsToOverrides.Flatten()
                                             .Select(t => (("assignment_overrides[][id]", t.Item2.ToString()),
                                                           ("assignment_overrides[][assignment_id]", t.Item1.ToString())
                                                          ))
                                             .Interleave();
            
            // mind the .Interleave() and the order of id and assignment_id; this endpoint is very specific about 
            // the order of parameters

            var response = 
                await client.GetAsync($"courses/{courseId}/assignments/overrides" + BuildDuplicateKeyQueryString(args.ToArray()));

            var models = await AccumulateDeserializePages<AssignmentOverrideModel>(response);
            
            return models.DiscardNull()
                         .Select(model => new AssignmentOverride(this, model));
        }

        internal async Task<AssignmentOverride> PostAssignmentOverride(AssignmentOverrideBuilder builder) {
            var args = builder.Fields
                              .Select(kv => (kv.Key, kv.Value))
                              .Concat(builder.ArrayFields
                                             .SelectMany(k => k, (k, v) => (k.Key, v)));

            var response = await client.PostAsync($"courses/{builder.CourseId}/assignments/{builder.AssignmentId}/overrides",
                                                   BuildMultipartHttpArguments(args));
            response.AssertSuccess();

            var model =
                JsonConvert.DeserializeObject<AssignmentOverrideModel>(await response.Content.ReadAsStringAsync());
            return new AssignmentOverride(this, model);
        }

        /// <summary>
        /// Returns an <see cref="AssignmentOverrideBuilder"/> for creating a new assignment override.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="assignmentId">The assignment id.</param>
        /// <returns>The <see cref="AssignmentOverrideBuilder"/>.</returns>
        public AssignmentOverrideBuilder CreateAssignmentOverride(ulong courseId, ulong assignmentId) {
            return new AssignmentOverrideBuilder(this, courseId, assignmentId);
        }

        private Task<HttpResponseMessage> RawSubmitAssignment([NotNull] string type,
                                                              [NotNull] string baseId,
                                                              [NotNull] string assignmentId,
                                                              [CanBeNull] string comment,
                                                              [NotNull] INewSubmissionContent submissionContent) {
            var url = $"{type}/{baseId}/assignments/{assignmentId}/submissions";

            var args = submissionContent.GetTuples()
                                        .Append(("submission[submission_type]", submissionContent.Type.GetApiRepresentation()))
                                        .Append(("comment[text_comment]", comment));
            
            return client.PostAsync(url, BuildHttpArguments(args));
        }

        /// <summary>
        /// Make a submission under the current user for this assignment.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="assignmentId">The assignment id.</param>
        /// <param name="submissionContent">The content to submit.</param>
        /// <param name="comment">An optional comment to include with the submission.</param>
        /// <returns>The completed submission.</returns>
        /// <seealso cref="OnlineTextEntrySubmission"/>
        /// <seealso cref="OnlineUrlSubmission"/>
        /// <seealso cref="OnlineUploadSubmission"/>
        public async Task<Submission> SubmitCourseAssignment(ulong courseId,
                                                             ulong assignmentId,
                                                             [NotNull] INewSubmissionContent submissionContent,
                                                             string comment = null) {
            var response = await RawSubmitAssignment("courses",
                                                     courseId.ToString(),
                                                     assignmentId.ToString(),
                                                     comment,
                                                     submissionContent);
            response.AssertSuccess();

            var model = JsonConvert.DeserializeObject<SubmissionModel>(await response.Content.ReadAsStringAsync());
            return new Submission(this, model);
        }

        internal async Task<Assignment> PostCreateAssignment(AssignmentBuilder builder) {
            IEnumerable<(string Key, string)> args = builder.Fields
                                                            .Select(kv => (kv.Key, kv.Value))
                                                            .Concat(builder.ArrayFields
                                                                           .SelectMany(k => k, (k, v) => (k.Key, v)));

            var response = await client.PostAsync($"courses/{builder.CourseId}/assignments",
                                                   BuildMultipartHttpArguments(args));
            response.AssertSuccess();

            var model = JsonConvert.DeserializeObject<AssignmentModel>(await response.Content.ReadAsStringAsync());
            return new Assignment(this, model);
        }

        /// <summary>
        /// Returns a <see cref="AssignmentBuilder"/> for creating a new assignment.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <returns>The builder.</returns>
        public AssignmentBuilder CreateAssignment(ulong courseId) {
            return new AssignmentBuilder(this, false, courseId);
        }

        internal async Task<Assignment> PutEditAssignment(ulong id, AssignmentBuilder builder) {
            var args = builder.Fields
                              .Select(kv => (kv.Key, kv.Value))
                              .Concat(builder.ArrayFields
                                             .SelectMany(k => k, (k, v) => (k.Key, v)));
            
            var response = await RawEditAssignment(builder.CourseId.ToString(), 
                                                   id.ToString(), 
                                                   BuildMultipartHttpArguments(args));
            response.AssertSuccess();

            var model = JsonConvert.DeserializeObject<AssignmentModel>(await response.Content.ReadAsStringAsync());
            return new Assignment(this, model);
        }

        /// <summary>
        /// Returns a <see cref="AssignmentBuilder"/> for editing an existing assignment.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="assignmentId">The assignment id.</param>
        /// <returns>The builder.</returns>
        public AssignmentBuilder EditAssignment(ulong courseId, ulong assignmentId) {
            return new AssignmentBuilder(this, true, courseId, assignmentId);
        }
        
        private Task<HttpResponseMessage> RawEditAssignment(string courseId, string assignmentId, HttpContent content) {
            return client.PutAsync($"courses/{courseId}/assignments/{assignmentId}", content);
        }
        
        /// <summary>
        /// Categories of optional data that can be requested for inclusion within <see cref="Assignment"/> objects.
        /// </summary>
        [Flags]
        [PublicAPI]
        public enum AssignmentIncludes {
            /// <summary>
            /// Include no extra data.
            /// </summary>
            Default = 0,
            /// <summary>
            /// Include the assignment's submissions.
            /// </summary>
            [ApiRepresentation("submission")]
            Submission = 1 << 0,
            /// <summary>
            /// Include the assignment's visibility information.
            /// </summary>
            [ApiRepresentation("assignment_visibility")]
            AssignmentVisibility = 1 << 1,
            /// <summary>
            /// Include any applicable assignment overrides.
            /// </summary>
            [ApiRepresentation("overrides")]
            Overrides = 1 << 2,
            /// <summary>
            /// Include any observed users.
            /// </summary>
            [ApiRepresentation("observed_users")]
            ObservedUsers = 1 << 3,
            /// <summary>
            /// Include all relevant dates.
            /// </summary>
            [ApiRepresentation("all_dates")]
            AllDates = 1 << 4
        }

        /// <summary>
        /// Buckets that assignments can be sorted into.
        /// </summary>
        [PublicAPI]
        public enum AssignmentBucket {
            /// <summary>
            /// Past assignments.
            /// </summary>
            [ApiRepresentation("past")]
            Past,
            /// <summary>
            /// Overdue assignments.
            /// </summary>
            [ApiRepresentation("overdue")]
            Overdue,
            /// <summary>
            /// Undated assignments.
            /// </summary>
            [ApiRepresentation("undated")]
            Undated,
            /// <summary>
            /// Ungraded assignments.
            /// </summary>
            [ApiRepresentation("ungraded")]
            Ungraded,
            /// <summary>
            /// Unsubmitted assignments.
            /// </summary>
            [ApiRepresentation("unsubmitted")]
            Unsubmitted,
            /// <summary>
            /// Upcoming assignments.
            /// </summary>
            [ApiRepresentation("upcoming")]
            Upcoming,
            /// <summary>
            /// Future assignments.
            /// </summary>
            [ApiRepresentation("future")]
            Future
        }

        [PaginatedResponse]
        private Task<HttpResponseMessage> RawListAssignmentsGeneric(string url,
                                                                    AssignmentIncludes includes,
                                                                    [CanBeNull] string searchTerm,
                                                                    bool? overrideAssignmentDates,
                                                                    bool? needsGradingCountBySection, 
                                                                    AssignmentBucket? bucket,
                                                                    [CanBeNull] IEnumerable<ulong> assignmentIds,
                                                                    [CanBeNull] string orderBy) {
            var args = includes.GetTuples()
                                 .Append(("search_term", searchTerm))
                                 .Append(("override_assignment_dates", overrideAssignmentDates?.ToShortString()))
                                 .Append(("needs_grading_count_by_section", needsGradingCountBySection?.ToShortString()))
                                 .Append(("bucket", bucket?.GetApiRepresentation()))
                                 .Append(("order_by", orderBy));
            
            if (assignmentIds != null) {
                args = args.Concat(assignmentIds.Select(id => id.ToString())
                                                .Select(s => ("assignment_ids[]", s)));
            }

            return client.GetAsync(url + BuildQueryString(args.ToArray()));
        }
        
        [PaginatedResponse]
        private Task<HttpResponseMessage> RawListCourseAssignments(string courseId,
                                                                   AssignmentIncludes includes,
                                                                   [CanBeNull] string searchTerm,
                                                                   bool? overrideAssignmentDates,
                                                                   bool? needsGradingCountBySection, 
                                                                   AssignmentBucket? bucket,
                                                                   [CanBeNull] IEnumerable<ulong> assignmentIds,
                                                                   [CanBeNull] string orderBy) {
            return RawListAssignmentsGeneric($"courses/{courseId}/assignments",
                                             includes,
                                             searchTerm,
                                             overrideAssignmentDates,
                                             needsGradingCountBySection,
                                             bucket,
                                             assignmentIds,
                                             orderBy);
        }
        
        [PaginatedResponse]
        private Task<HttpResponseMessage> RawListCourseGroupAssignments(string courseId,
                                                                        string assignmentGroupId,
                                                                        AssignmentIncludes includes,
                                                                        [CanBeNull] string searchTerm,
                                                                        bool? overrideAssignmentDates,
                                                                        bool? needsGradingCountBySection, 
                                                                        AssignmentBucket? bucket,
                                                                        [CanBeNull] IEnumerable<ulong> assignmentIds,
                                                                        [CanBeNull] string orderBy) {
            return RawListAssignmentsGeneric($"courses/{courseId}/assignment_groups/{assignmentGroupId}/assignments",
                                             includes,
                                             searchTerm,
                                             overrideAssignmentDates,
                                             needsGradingCountBySection,
                                             bucket,
                                             assignmentIds,
                                             orderBy);
        }

        /// <summary>
        /// Streams all assignments in this course that are visible to the current user.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="includes">Optional extra data to include in the assignment response.</param>
        /// <param name="searchTerm">An optional search term.</param>
        /// <param name="overrideAssignmentDates">Apply assignment overrides to dates. Defaults to true.</param>
        /// <param name="needsGradingCountBySection">Split the NeedsGradingCount key into sections. Defaults to false.</param>
        /// <param name="bucket">An optional bucket to filter the returned assignments by.</param>
        /// <param name="assignmentIds">An optional list of ids to filter the returned assignments by.</param>
        /// <param name="orderBy">An optional string determining the order of assignments.</param>
        /// <returns>The stream of assignments.</returns>
        /// <seealso cref="AssignmentIncludes"/>
        /// <seealso cref="AssignmentBucket"/>
        public async IAsyncEnumerable<Assignment> StreamCourseAssignments(ulong courseId,
                                                                         AssignmentIncludes includes = AssignmentIncludes.Default,
                                                                         string searchTerm = null,
                                                                         bool? overrideAssignmentDates = null,
                                                                         bool? needsGradingCountBySection = null,
                                                                         AssignmentBucket? bucket = null,
                                                                         IEnumerable<ulong> assignmentIds = null,
                                                                         string orderBy = null) {

            var response = await RawListCourseAssignments(courseId.ToString(),
                                                          includes,
                                                          searchTerm,
                                                          overrideAssignmentDates,
                                                          needsGradingCountBySection,
                                                          bucket,
                                                          assignmentIds,
                                                          orderBy);

            await foreach (var model in StreamDeserializePages<AssignmentModel>(response)) {
                yield return new Assignment(this, model);
            }
        }

        private Task<HttpResponseMessage> RawGetSingleAssignment(string courseId,
                                                                 string assignmentId,
                                                                 AssignmentIncludes includes,
                                                                 bool? overrideAssignmentDates,
                                                                 bool? needsGradingCountBySection,
                                                                 bool? allDates) {
            var args = includes.GetTuples()
                                 .Append(("override_assignment_dates", overrideAssignmentDates?.ToShortString()))
                                 .Append(("needs_grading_count_by_section", needsGradingCountBySection?.ToShortString()))
                                 .Append(("all_dates", allDates?.ToShortString()));
            
            var url = $"courses/{courseId}/assignments/{assignmentId}" + BuildQueryString(args.ToArray());

            return client.GetAsync(url);
        }

        /// <summary>
        /// Gets a single assignment from this course.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="assignmentId">The assignment id.</param>
        /// <param name="includes">Optional extra data to include in the assignment response.</param>
        /// <param name="overrideAssignmentDates">Apply assignment overrides to dates. Defaults to true.</param>
        /// <param name="needsGradingCountBySection">Split the NeedsGradingCount key into sections. Defaults to false.</param>
        /// <param name="allDates">Return all dates associated with the assignment, if applicable.</param>
        /// <returns>The assignment.</returns>
        /// <seealso cref="AssignmentIncludes"/>
        public async Task<Assignment> GetAssignment(ulong courseId, 
                                                    ulong assignmentId, 
                                                    AssignmentIncludes includes = AssignmentIncludes.Default,
                                                    bool? overrideAssignmentDates = null,
                                                    bool? needsGradingCountBySection = null,
                                                    bool? allDates = null) {
            
            var response = await RawGetSingleAssignment(courseId.ToString(),
                                                        assignmentId.ToString(),
                                                        includes,
                                                        overrideAssignmentDates,
                                                        needsGradingCountBySection,
                                                        allDates);
            response.AssertSuccess();
            
            var model = JsonConvert.DeserializeObject<AssignmentModel>(await response.Content.ReadAsStringAsync());
            return new Assignment(this, model);
        }
    }
}