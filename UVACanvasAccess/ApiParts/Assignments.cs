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
        
        [PaginatedResponse]
        private Task<HttpResponseMessage> RawListAssignmentOverrides(string courseId, string assignmentId) {
            var url = $"courses/{courseId}/assignments/{assignmentId}/overrides";
            return _client.GetAsync(url);
        }

        public async Task<IEnumerable<AssignmentOverride>> ListAssignmentOverrides(ulong courseId, ulong assignmentId) {
            var response = await RawListAssignmentOverrides(courseId.ToString(), assignmentId.ToString());

            var models = await AccumulateDeserializePages<AssignmentOverrideModel>(response);

            return from model in models
                   select new AssignmentOverride(this, model);
        }

        private Task<HttpResponseMessage> RawGetAssignmentOverride(string courseId, 
                                                                   string assignmentId,
                                                                   string overrideId) {
            var url = $"courses/{courseId}/assignments/{assignmentId}/overrides/{overrideId}";
            return _client.GetAsync(url);
        }

        public async Task<AssignmentOverride> GetAssignmentOverride(ulong courseId, 
                                                                    ulong assignmentId,
                                                                    ulong overrideId) {
            var response =
                await RawGetAssignmentOverride(courseId.ToString(), assignmentId.ToString(), overrideId.ToString());
            response.AssertSuccess();

            var model =
                JsonConvert.DeserializeObject<AssignmentOverrideModel>(await response.Content.ReadAsStringAsync());
            return new AssignmentOverride(this, model);
        }

        [PaginatedResponse]
        private Task<HttpResponseMessage> RawBatchGetAssignmentOverrides(string courseId, string args) {
            return _client.GetAsync($"/api/v1/courses/{courseId}/assignments/overrides" + args);
        }

        public async Task<IEnumerable<AssignmentOverride>> BatchGetAssignmentOverrides(ulong courseId,
                                                                                       ILookup<ulong, ulong> assignmentsToOverrides) {
            var args = assignmentsToOverrides.Flatten()
                                             .Select(t => (("assignment_overrides[][id]", t.Item2.ToString()),
                                                           ("assignment_overrides[][assignment_id]", t.Item1.ToString())
                                                          ))
                                             .Interleave();
            
            // mind the .Interleave() and the order of id and assignment_id; this endpoint is very specific about 
            // the order of parameters

            
            // endpoint expects duplicate keys in a GET because of course it does,
            // c#'s standard query string builder (underlying NameValueCollection) does not support this nonstandard
            // so we have to hack in our own. luckily we don't have to encode anything because our values
            // are all just integers.

            var q = "?" + string.Join("&", args.Select(kv => $"{kv.Item1}={kv.Item2}"));

            var response = await RawBatchGetAssignmentOverrides(courseId.ToString(), q);

            var models = await AccumulateDeserializePages<AssignmentOverrideModel>(response);
            
            return models.DiscardNull()
                         .Select(model => new AssignmentOverride(this, model));
        }

        private Task<HttpResponseMessage> RawCreateAssignmentOverride(string courseId,
                                                                      string assignmentId,
                                                                      HttpContent content) {
            var url = $"courses/{courseId}/assignments/{assignmentId}/overrides";
            return _client.PostAsync(url, content);
        }

        internal async Task<AssignmentOverride> PostAssignmentOverride(AssignmentOverrideBuilder builder) {
            var args = builder.Fields
                              .Select(kv => (kv.Key, kv.Value))
                              .Concat(builder.ArrayFields
                                             .SelectMany(k => k, (k, v) => (k.Key, v)));
            
            var response = await RawCreateAssignmentOverride(builder.CourseId.ToString(), 
                                                             builder.AssignmentId.ToString(),
                                                             BuildMultipartHttpArguments(args));
            response.AssertSuccess();

            var model =
                JsonConvert.DeserializeObject<AssignmentOverrideModel>(await response.Content.ReadAsStringAsync());
            return new AssignmentOverride(this, model);
        }

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
            
            return _client.PostAsync(url, BuildHttpArguments(args));
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

        private Task<HttpResponseMessage> RawCreateAssignment(string courseId, HttpContent content) {
            return _client.PostAsync($"courses/{courseId}/assignments", content);
        }

        internal async Task<Assignment> PostCreateAssignment(AssignmentBuilder builder) {
            var args = builder.Fields
                              .Select(kv => (kv.Key, kv.Value))
                              .Concat(builder.ArrayFields
                                             .SelectMany(k => k, (k, v) => (k.Key, v)));
            
            var response = await RawCreateAssignment(builder.CourseId.ToString(), BuildMultipartHttpArguments(args));
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
            return _client.PutAsync($"courses/{courseId}/assignments/{assignmentId}", content);
        }
        
        /// <summary>
        /// Flags representing optional data that can be included as part of an <see cref="Assignment"/>.
        /// </summary>
        [Flags]
        public enum AssignmentInclusions {
            Default = 0,
            [ApiRepresentation("submission")]
            Submission = 1 << 0,
            [ApiRepresentation("assignment_visibility")]
            AssignmentVisibility = 1 << 1,
            [ApiRepresentation("overrides")]
            Overrides = 1 << 2,
            [ApiRepresentation("observed_users")]
            ObservedUsers = 1 << 3,
            [ApiRepresentation("all_dates")]
            AllDates = 1 << 4
        }

        /// <summary>
        /// Buckets that assignments can be sorted into.
        /// </summary>
        public enum AssignmentBucket {
            [ApiRepresentation("past")]
            Past,
            [ApiRepresentation("overdue")]
            Overdue,
            [ApiRepresentation("undated")]
            Undated,
            [ApiRepresentation("ungraded")]
            Ungraded,
            [ApiRepresentation("ubsubmitted")]
            Unsubmitted,
            [ApiRepresentation("upcoming")]
            Upcoming,
            [ApiRepresentation("future")]
            Future
        }

        [PaginatedResponse]
        private Task<HttpResponseMessage> RawListAssignmentsGeneric(string url,
                                                                    AssignmentInclusions inclusions,
                                                                    [CanBeNull] string searchTerm,
                                                                    bool? overrideAssignmentDates,
                                                                    bool? needsGradingCountBySection, 
                                                                    AssignmentBucket? bucket,
                                                                    [CanBeNull] IEnumerable<ulong> assignmentIds,
                                                                    [CanBeNull] string orderBy) {
            var args = inclusions.GetTuples()
                                 .Append(("search_term", searchTerm))
                                 .Append(("override_assignment_dates", overrideAssignmentDates?.ToString().ToLower()))
                                 .Append(("needs_grading_count_by_section", needsGradingCountBySection?.ToString().ToLower()))
                                 .Append(("bucket", bucket?.GetApiRepresentation()))
                                 .Append(("order_by", orderBy));
            
            if (assignmentIds != null) {
                args = args.Concat(assignmentIds.Select(id => id.ToString())
                                                .Select(s => ("assignment_ids[]", s)));
            }

            return _client.GetAsync(url + BuildQueryString(args.ToArray()));
        }
        
        [PaginatedResponse]
        private Task<HttpResponseMessage> RawListCourseAssignments(string courseId,
                                                                   AssignmentInclusions inclusions,
                                                                   [CanBeNull] string searchTerm,
                                                                   bool? overrideAssignmentDates,
                                                                   bool? needsGradingCountBySection, 
                                                                   AssignmentBucket? bucket,
                                                                   [CanBeNull] IEnumerable<ulong> assignmentIds,
                                                                   [CanBeNull] string orderBy) {
            return RawListAssignmentsGeneric($"courses/{courseId}/assignments",
                                             inclusions,
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
                                                                        AssignmentInclusions inclusions,
                                                                        [CanBeNull] string searchTerm,
                                                                        bool? overrideAssignmentDates,
                                                                        bool? needsGradingCountBySection, 
                                                                        AssignmentBucket? bucket,
                                                                        [CanBeNull] IEnumerable<ulong> assignmentIds,
                                                                        [CanBeNull] string orderBy) {
            return RawListAssignmentsGeneric($"courses/{courseId}/assignment_groups/{assignmentGroupId}/assignments",
                                             inclusions,
                                             searchTerm,
                                             overrideAssignmentDates,
                                             needsGradingCountBySection,
                                             bucket,
                                             assignmentIds,
                                             orderBy);
        }

        /// <summary>
        /// Returns a list of all assignments in this course that are visible to the current user.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="inclusions">Optional extra data to include in the assignment response.</param>
        /// <param name="searchTerm">An optional search term.</param>
        /// <param name="overrideAssignmentDates">Apply assignment overrides to dates. Defaults to true.</param>
        /// <param name="needsGradingCountBySection">Split the NeedsGradingCount key into sections. Defaults to false.</param>
        /// <param name="bucket">An optional bucket to filter the returned assignments by.</param>
        /// <param name="assignmentIds">An optional list of ids to filter the returned assignments by.</param>
        /// <param name="orderBy">An optional string determining the order of assignments.</param>
        /// <returns>The list of assignments.</returns>
        /// <seealso cref="AssignmentInclusions"/>
        /// <seealso cref="AssignmentBucket"/>
        public async Task<IEnumerable<Assignment>> ListCourseAssignments(ulong courseId,
                                                                         AssignmentInclusions inclusions = AssignmentInclusions.Default,
                                                                         string searchTerm = null,
                                                                         bool? overrideAssignmentDates = null,
                                                                         bool? needsGradingCountBySection = null,
                                                                         AssignmentBucket? bucket = null,
                                                                         IEnumerable<ulong> assignmentIds = null,
                                                                         string orderBy = null) {

            var response = await RawListCourseAssignments(courseId.ToString(),
                                                          inclusions,
                                                          searchTerm,
                                                          overrideAssignmentDates,
                                                          needsGradingCountBySection,
                                                          bucket,
                                                          assignmentIds,
                                                          orderBy);

            var models = await AccumulateDeserializePages<AssignmentModel>(response);

            return from model in models
                   select new Assignment(this, model);
        }

        private Task<HttpResponseMessage> RawGetSingleAssignment(string courseId,
                                                                 string assignmentId,
                                                                 AssignmentInclusions inclusions,
                                                                 bool? overrideAssignmentDates,
                                                                 bool? needsGradingCountBySection,
                                                                 bool? allDates) {
            var args = inclusions.GetTuples()
                                 .Append(("override_assignment_dates", overrideAssignmentDates?.ToString().ToLower()))
                                 .Append(("needs_grading_count_by_section", needsGradingCountBySection?.ToString().ToLower()))
                                 .Append(("all_dates", allDates?.ToString().ToLower()));
            
            var url = $"courses/{courseId}/assignments/{assignmentId}" + BuildQueryString(args.ToArray());

            return _client.GetAsync(url);
        }

        /// <summary>
        /// Gets a single assignment from this course.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="assignmentId">The assignment id.</param>
        /// <param name="inclusions">Optional extra data to include in the assignment response.</param>
        /// <param name="overrideAssignmentDates">Apply assignment overrides to dates. Defaults to true.</param>
        /// <param name="needsGradingCountBySection">Split the NeedsGradingCount key into sections. Defaults to false.</param>
        /// <param name="allDates">Return all dates associated with the assignment, if applicable.</param>
        /// <returns>The assignment.</returns>
        /// <seealso cref="AssignmentInclusions"/>
        public async Task<Assignment> GetAssignment(ulong courseId, 
                                                    ulong assignmentId, 
                                                    AssignmentInclusions inclusions = AssignmentInclusions.Default,
                                                    bool? overrideAssignmentDates = null,
                                                    bool? needsGradingCountBySection = null,
                                                    bool? allDates = null) {
            
            var response = await RawGetSingleAssignment(courseId.ToString(),
                                                        assignmentId.ToString(),
                                                        inclusions,
                                                        overrideAssignmentDates,
                                                        needsGradingCountBySection,
                                                        allDates);
            response.AssertSuccess();
            
            var model = JsonConvert.DeserializeObject<AssignmentModel>(await response.Content.ReadAsStringAsync());
            return new Assignment(this, model);
        }
    }
}