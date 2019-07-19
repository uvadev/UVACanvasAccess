using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Assignments;
using UVACanvasAccess.Structures.Assignments;
using UVACanvasAccess.Structures.Submissions;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Builders {
    
    /// <summary>
    /// A class used to create or edit assignments using the builder pattern.
    /// When all desired fields are set, call <see cref="Post"/> to execute the operation.
    /// </summary>
    public class AssignmentBuilder {
        private readonly Api _api;
        private readonly bool _isEditing;
        private readonly ulong? _id;
        internal ulong CourseId { get; }

        internal Dictionary<string, string> Fields { get; } = new Dictionary<string, string>();
        
        private readonly List<KeyValuePair<string, string>> _arrayFields = new List<KeyValuePair<string, string>>();
        
        internal ILookup<string, string> ArrayFields => _arrayFields.Distinct()
                                                                    .ToLookup(kv => kv.Key,
                                                                              kv => kv.Value);

        internal AssignmentBuilder(Api api, bool isEditing, ulong courseId, ulong? id = null) {
            _api = api;
            _isEditing = isEditing;
            _id = id;
            CourseId = courseId;
        }

        /// <summary>
        /// The name of the assignment.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>This builder.</returns>
        /// <remarks>This field is required.</remarks>
        public AssignmentBuilder WithName(string name) {
            return Put("name", name);
        }

        /// <summary>
        /// The position of this assignment when displaying assignment lists.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns>This builder.</returns>
        public AssignmentBuilder WithPosition(int pos) {
            return Put("position", pos.ToString());
        }
        
        /// <summary>
        /// The list of submission types supported for this assignment.
        /// </summary>
        /// <param name="types"></param>
        /// <returns>This builder.</returns>
        /// <remarks>
        /// If the assignment supports online submissions (not including <see cref="SubmissionTypes.OnlineQuiz"/> but
        /// including <see cref="SubmissionTypes.MediaRecording"/>), then multiple submission types are supported.
        /// Otherwise, only one submission type is supported. (See
        /// <a href="https://canvas.instructure.com/doc/api/assignments.html">Edit an assignment</a> for elaboration on this.)
        /// </remarks>
        public AssignmentBuilder WithSubmissionTypes(SubmissionTypes types) {
            foreach (var type in types.GetFlags()) {
                PutArr("submission_types", type.GetApiRepresentation());
            }
            return this;
        }

        /// <summary>
        /// Allowed file extensions for the <see cref="SubmissionTypes.OnlineUpload"/> submission type, if this assignment
        /// supports it.
        /// </summary>
        /// <param name="extensions"></param>
        /// <returns>This builder.</returns>
        public AssignmentBuilder WithAllowedExtensions(IEnumerable<string> extensions) {
            foreach (var e in extensions) {
                PutArr("allowed_extensions", e);
            }
            return this;
        }

        /// <summary>
        /// Allowed file extensions for the <see cref="SubmissionTypes.OnlineUpload"/> submission type, if this assignment
        /// supports it.
        /// </summary>
        /// <param name="extensions"></param>
        /// <returns>This builder.</returns>
        public AssignmentBuilder WithAllowedExtensions(params string[] extensions) {
            return WithAllowedExtensions(extensions.AsEnumerable());
        }

        /// <summary>
        /// Enables Turnitin.
        /// </summary>
        /// <param name="enabled"></param>
        /// <returns>This builder.</returns>
        /// <remarks>
        /// Enabling Turnitin requires that the assignment support the <see cref="SubmissionTypes.OnlineUpload"/>
        /// submission type, and that Turnitin is available for this course.
        /// </remarks>
        public AssignmentBuilder WithTurnitinEnabled(bool enabled = true) {
            return Put("turnitin_enabled", enabled.ToShortString());
        }

        /// <summary>
        /// Enables Vericite.
        /// </summary>
        /// <param name="enabled"></param>
        /// <returns>This builder.</returns>
        /// <remarks>
        /// Enabling Vericite requires that the assignment support the <see cref="SubmissionTypes.OnlineUpload"/>
        /// submission type, and that Vericite is available for this course.
        /// </remarks>
        public AssignmentBuilder WithVericiteEnabled(bool enabled = true) {
            return Put("vericite_enabled", enabled.ToShortString());
        }

        /// <summary>
        /// Settings for Turnitin, if it is enabled.
        /// </summary>
        /// <param name="settings"></param>
        /// <returns>This builder.</returns>
        public AssignmentBuilder WithTurnitinSettings(TurnitinSettingsModel settings) {
            return Put("turnitin_settings", JsonConvert.SerializeObject(settings));
        }

        /// <summary>
        /// Data for SIS integration.
        /// </summary>
        /// <param name="sis"></param>
        /// <returns>This builder.</returns>
        /// <remarks>Requires that the current user is an admin.</remarks>
        public AssignmentBuilder WithSisIntegrationData(string sis) {
            return Put("integration_data", sis);
        }

        /// <summary>
        /// Unique ID for third-party integrations.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>This builder.</returns>
        public AssignmentBuilder WithIntegrationId(string id) {
            return Put("integration_id", id);
        }

        /// <summary>
        /// Enables peer reviews.
        /// </summary>
        /// <param name="enabled"></param>
        /// <returns>This builder.</returns>
        /// <remarks>
        /// Ignored if this assignment supports any of <see cref="SubmissionTypes.ExternalTool"/>,
        /// <see cref="SubmissionTypes.DiscussionTopic"/>, <see cref="SubmissionTypes.OnlineQuiz"/>, or
        /// <see cref="SubmissionTypes.OnPaper"/>.
        /// </remarks>
        public AssignmentBuilder WithPeerReviews(bool enabled = true) {
            return Put("peer_reviews", enabled.ToShortString());
        }

        /// <summary>
        /// Enables automatically assigned peer reviews.
        /// </summary>
        /// <param name="enabled"></param>
        /// <returns>This builder.</returns>
        /// <remarks>Implies <see cref="WithPeerReviews"/> = true.</remarks>
        public AssignmentBuilder WithAutomaticPeerReviews(bool enabled = true) {
            Put("peer_reviews", "true");
            return Put("automatic_peer_reviews", enabled.ToShortString());
        }

        /// <summary>
        /// Determines if Canvas notifies students when the content of this assignment changes.
        /// </summary>
        /// <param name="enabled"></param>
        /// <returns>This builder.</returns>
        public AssignmentBuilder WithUpdateNotification(bool enabled = true) {
            return Put("notify_of_update", enabled.ToShortString());
        }

        /// <summary>
        /// Assigns this assignment to a group.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>This builder.</returns>
        public AssignmentBuilder WithGroupCategoryId(ulong id) {
            return Put("group_category_id", id.ToString());
        }

        /// <summary>
        /// If this is a group assignment, determines if grades are applied per-student or to the entire group.
        /// </summary>
        /// <param name="enabled"></param>
        /// <returns>This builder.</returns>
        public AssignmentBuilder WithIndividualGroupGrading(bool enabled = true) {
            return Put("grade_group_students_individually", enabled.ToShortString());
        }

        /// <summary>
        /// If this assignment supports <see cref="SubmissionTypes.ExternalTool"/>, sets the parameters of
        /// the external tool.
        /// </summary>
        /// <param name="tool">The hash.</param>
        /// <returns>This builder.</returns>
        public AssignmentBuilder WithExternalToolTagAttribute(string tool) {
            return Put("external_tool_tag_attribute", tool);
        }

        /// <summary>
        /// The maximum amount of points that can be earned from this assignment.
        /// </summary>
        /// <param name="points"></param>
        /// <returns>This builder.</returns>
        public AssignmentBuilder WithPointsPossible(double points) {
            return Put("points_possible", points.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// The <see cref="GradingType"/> used for grading the assignment.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>This builder.</returns>
        /// <remarks>If unspecified, this field defaults to <see cref="GradingType.Points"/>.</remarks>
        public AssignmentBuilder WithGradingType(GradingType type) {
            return Put("grading_type", type.GetApiRepresentation());
        }

        /// <summary>
        /// The date/time when this assignment is due.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>This builder.</returns>
        /// <remarks>
        /// When either <see cref="WithUnlockDate"/> or <see cref="WithLockDate"/> are specified, this must fall between them.
        /// </remarks>
        public AssignmentBuilder WithDueDate(DateTime dateTime) {
            return Put("due_at", dateTime.ToIso8601Date());
        }

        /// <summary>
        /// The date/time when this assignment becomes unavailable.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>This builder.</returns>
        /// <remarks>When <see cref="WithDueDate"/> is specified, this must fall after it.</remarks>
        public AssignmentBuilder WithLockDate(DateTime dateTime) {
            return Put("lock_at", dateTime.ToIso8601Date());
        }

        /// <summary>
        /// The date/time when this assignment becomes available.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>This builder.</returns>
        /// <remarks>When <see cref="WithDueDate"/> is specified, this must fall before it.</remarks>
        public AssignmentBuilder WithUnlockDate(DateTime dateTime) {
            return Put("unlock_at", dateTime.ToIso8601Date());
        }

        /// <summary>
        /// The description of this assignment.
        /// </summary>
        /// <param name="description"></param>
        /// <returns>This builder.</returns>
        public AssignmentBuilder WithDescription(string description) {
            return Put("description", description);
        }

        /// <summary>
        /// The assignment group to put this assignment into.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>This builder.</returns>
        public AssignmentBuilder WithAssignmentGroupId(ulong id) {
            return Put("assignment_group_id", id.ToString());
        }

        /// <summary>
        /// Mutes this assignment. A muted assignment sends no notifications and hides grades from students.
        /// </summary>
        /// <param name="enabled"></param>
        /// <returns>This builder.</returns>
        public AssignmentBuilder Muted(bool enabled = true) {
            return Put("muted", enabled.ToShortString());
        }

        /// <summary>
        /// List of any overrides for this assignment.
        /// </summary>
        /// <param name="overrides"></param>
        /// <returns>This builder.</returns>
        public AssignmentBuilder WithAssignmentOverrides(IEnumerable<AssignmentOverrideModel> overrides) {
            foreach (var o in overrides) {
                PutArr("assignment_overrides", JsonConvert.SerializeObject(o));
            }
            return this;
        }

        /// <summary>
        /// Determines if this assignment is only visible to overrides.
        /// </summary>
        /// <param name="enabled"></param>
        /// <returns>This builder.</returns>
        public AssignmentBuilder OnlyVisibleToOverrides(bool enabled = true) {
            return Put("only_visible_to_overrides", enabled.ToShortString());
        }

        /// <summary>
        /// Determines if the assignment is published.
        /// </summary>
        /// <param name="enabled"></param>
        /// <returns>This builder.</returns>
        public AssignmentBuilder Published(bool enabled = true) {
            return Put("published", enabled.ToShortString());
        }

        /// <summary>
        /// The grading standard id to use.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>This builder.</returns>
        public AssignmentBuilder WithGradingStandard(ulong id) {
            return Put("grading_standard_id", id.ToString());
        }

        /// <summary>
        /// Omits this assignment from final grades.
        /// </summary>
        /// <param name="enabled"></param>
        /// <returns>This builder.</returns>
        public AssignmentBuilder OmitFromFinalGrade(bool enabled = true) {
            return Put("omit_from_final_grade", enabled.ToShortString());
        }

        /// <summary>
        /// Determines if this assignment should use the Quizzes 2 LTI tool.
        /// </summary>
        /// <param name="enabled"></param>
        /// <returns>This builder.</returns>
        public AssignmentBuilder WithQuizLti(bool enabled = true) {
            return Put("quiz_lti", enabled.ToShortString());
        }

        /// <summary>
        /// Determines if this assignment is moderated.
        /// </summary>
        /// <param name="enabled"></param>
        /// <returns>This builder.</returns>
        public AssignmentBuilder WithModeratedGrading(bool enabled = true) {
            return Put("moderated_grading", enabled.ToShortString());
        }

        /// <summary>
        /// The maximum amount of personal graders who may issue grades for this assignment.
        /// </summary>
        /// <param name="count"></param>
        /// <returns>This builder.</returns>
        public AssignmentBuilder WithGraderCount(uint count) {
            return Put("grader_count", count.ToString());
        }

        /// <summary>
        /// If this is a moderated assignment, the grader responsible for choosing the final grade.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>This builder.</returns>
        public AssignmentBuilder WithFinalGrader(ulong id) {
            return Put("final_grader_id", id.ToString());
        }

        /// <summary>
        /// If this is a moderated assignment, makes graders' comments visible to other graders.
        /// </summary>
        /// <param name="enabled"></param>
        /// <returns>This builder.</returns>
        public AssignmentBuilder WithGraderCommentsVisibleToGraders(bool enabled = true) {
            return Put("grader_comments_visible_to_graders", enabled.ToShortString());
        }

        /// <summary>
        /// If this is a moderated assignment, hides graders' names from other graders.
        /// </summary>
        /// <param name="enabled"></param>
        /// <returns>This builder.</returns>
        public AssignmentBuilder WithGradersAnonymousToGraders(bool enabled = true) {
            return Put("graders_anonymous_to_graders", enabled.ToShortString());
        }

        /// <summary>
        /// If this is a moderated assignment, makes graders' names visible to the final grader.
        /// </summary>
        /// <param name="enabled"></param>
        /// <returns>This builder.</returns>
        public AssignmentBuilder WithGraderNamesVisibleToFinalGrader(bool enabled = true) {
            return Put("graders_names_visible_to_final_grader", enabled.ToShortString());
        }

        
        /// <summary>
        /// Hides student identities from graders.
        /// </summary>
        /// <param name="enabled"></param>
        /// <returns>This builder.</returns>
        public AssignmentBuilder WithAnonymousGrading(bool enabled = true) {
            return Put("anonymous_grading", enabled.ToShortString());
        }

        
        /// <summary>
        /// The maximum number of submission attempts for this assignment.
        /// </summary>
        /// <param name="count"></param>
        /// <returns>This builder.</returns>
        /// <remarks>
        /// A value of <c>-1</c> indicates no limit.
        /// </remarks>
        public AssignmentBuilder WithAllowedAttempts(int count) {
            return Put("allowed_attempts", count.ToString());
        }

        /// <summary>
        /// Performs the operation using the fields in this builder.
        /// </summary>
        /// <returns>The newly created or edited assignment.</returns>
        public Task<Assignment> Post() {
            if (!_isEditing) {
                return _api.PostCreateAssignment(this);
            }

            Debug.Assert(_id != null, nameof(_id) + " != null");
            return _api.PutEditAssignment((ulong) _id, this);

        }

        private AssignmentBuilder Put(string key, string s) {
            Fields[$"assignment[{key}]"] = s;
            return this;
        }

        private AssignmentBuilder PutArr(string key, string s) {
            _arrayFields.Add(new KeyValuePair<string, string>($"assignment[{key}][]", s));
            return this;
        }
    }
}