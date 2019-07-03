using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UVACanvasAccess.Model.Assignments;
using UVACanvasAccess.Structures.Assignments;
using UVACanvasAccess.Structures.Submissions;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Builders {
    public class AssignmentBuilder {
        private readonly Api _api;
        private readonly bool _isEditing;
        private readonly ulong? _id;
        public ulong CourseId { get; }

        public Dictionary<string, string> Fields { get; } = new Dictionary<string, string>();
        
        private List<KeyValuePair<string, string>> _arrayFields = new List<KeyValuePair<string, string>>();
        
        public ILookup<string, string> ArrayFields => _arrayFields.Distinct()
                                                                  .ToLookup(kv => kv.Key,
                                                                            kv => kv.Value);

        public AssignmentBuilder(Api api, bool isEditing, ulong courseId, ulong? id = null) {
            _api = api;
            _isEditing = isEditing;
            _id = id;
            CourseId = courseId;
        }

        public AssignmentBuilder WithName(string name) {
            return Put("name", name);
        }

        public AssignmentBuilder WithPosition(int pos) {
            return Put("position", pos.ToString());
        }
        
        public AssignmentBuilder WithSubmissionTypes(SubmissionTypes types) {
            foreach (var type in types.GetFlags()) {
                PutArr("submission_types", type.GetApiRepresentation());
            }
            return this;
        }

        public AssignmentBuilder WithAllowedExtensions(IEnumerable<string> extensions) {
            foreach (var e in extensions) {
                PutArr("allowed_extensions", e);
            }
            return this;
        }

        public AssignmentBuilder WithAllowedExtensions(params string[] extensions) {
            return WithAllowedExtensions(extensions.AsEnumerable());
        }

        public AssignmentBuilder WithTurnitinEnabled(bool enabled = true) {
            return Put("turnitin_enabled", enabled.ToString().ToLower());
        }

        public AssignmentBuilder WithVericiteEnabled(bool enabled = true) {
            return Put("vericite_enabled", enabled.ToString().ToLower());
        }

        public AssignmentBuilder WithTurnitinSettings(TurnitinSettingsModel settings) {
            return Put("turnitin_settings", JsonConvert.SerializeObject(settings));
        }

        public AssignmentBuilder WithSisIntegrationData(string sis) {
            return Put("integration_data", sis);
        }

        public AssignmentBuilder WithIntegrationId(string id) {
            return Put("integration_id", id);
        }

        public AssignmentBuilder WithPeerReviews(bool enabled = true) {
            return Put("peer_reviews", enabled.ToString().ToLower());
        }

        public AssignmentBuilder WithAutomaticPeerReviews(bool enabled = true) {
            if (enabled) {
                WithPeerReviews();
            }
            return Put("automatic_peer_reviews", enabled.ToString().ToLower());
        }

        public AssignmentBuilder WithUpdateNotification(bool enabled = true) {
            return Put("notify_of_update", enabled.ToString().ToLower());
        }

        public AssignmentBuilder WithGroupCategoryId(ulong id) {
            return Put("group_category_id", id.ToString());
        }

        public AssignmentBuilder WithIndividualGroupGrading(bool enabled = true) {
            return Put("grade_group_students_individually", enabled.ToString().ToLower());
        }

        public AssignmentBuilder WithExternalToolTagAttribute(string tool) {
            return Put("external_tool_tag_attribute", tool);
        }

        public AssignmentBuilder WithPointsPossible(double points) {
            return Put("points_possible", points.ToString(CultureInfo.InvariantCulture));
        }

        public AssignmentBuilder WithGradingType(GradingType type) {
            return Put("grading_type", type.GetApiRepresentation());
        }

        public AssignmentBuilder WithDueDate(DateTime dateTime) {
            return Put("due_at", dateTime.ToIso8601Date());
        }

        public AssignmentBuilder WithLockDate(DateTime dateTime) {
            return Put("lock_at", dateTime.ToIso8601Date());
        }

        public AssignmentBuilder WithUnlockDate(DateTime dateTime) {
            return Put("unlock_at", dateTime.ToIso8601Date());
        }

        public AssignmentBuilder WithDescription(string description) {
            return Put("description", description);
        }

        public AssignmentBuilder WithAssignmentGroupId(ulong id) {
            return Put("assignment_group_id", id.ToString());
        }

        public AssignmentBuilder Muted(bool enabled = true) {
            return Put("muted", enabled.ToString().ToLower());
        }

        public AssignmentBuilder WithAssignmentOverrides(IEnumerable<AssignmentOverrideModel> overrides) {
            foreach (var o in overrides) {
                PutArr("assignment_overrides", JsonConvert.SerializeObject(o));
            }
            return this;
        }

        public AssignmentBuilder OnlyVisibleToOverrides(bool enabled = true) {
            return Put("only_visible_to_overrides", enabled.ToString().ToLower());
        }

        public AssignmentBuilder Published(bool enabled = true) {
            return Put("published", enabled.ToString().ToLower());
        }

        public AssignmentBuilder WithGradingStandard(ulong id) {
            return Put("grading_standard_id", id.ToString());
        }

        public AssignmentBuilder OmitFromFinalGrade(bool enabled = true) {
            return Put("omit_from_final_grade", enabled.ToString().ToLower());
        }

        public AssignmentBuilder WithQuizLti(bool enabled = true) {
            return Put("quiz_lti", enabled.ToString().ToLower());
        }

        public AssignmentBuilder WithModeratedGrading(bool enabled = true) {
            return Put("moderated_grading", enabled.ToString().ToLower());
        }

        public AssignmentBuilder WithGraderCount(uint count) {
            return Put("grader_count", count.ToString());
        }

        public AssignmentBuilder WithFinalGrader(ulong id) {
            return Put("final_grader_id", id.ToString());
        }

        public AssignmentBuilder WithGraderCommentsVisibleToGraders(bool enabled = true) {
            return Put("grader_comments_visible_to_graders", enabled.ToString().ToLower());
        }

        public AssignmentBuilder WithGradersAnonymousToGraders(bool enabled = true) {
            return Put("graders_anonymous_to_graders", enabled.ToString().ToLower());
        }

        public AssignmentBuilder WithGraderNamesVisibleToFinalGrader(bool enabled = true) {
            return Put("graders_names_visible_to_final_grader", enabled.ToString().ToLower());
        }

        public AssignmentBuilder WithAnonymousGrading(bool enabled = true) {
            return Put("anonymous_grading", enabled.ToString().ToLower());
        }

        public AssignmentBuilder WithAllowedAttempts(int count) {
            return Put("allowed_attempts", count.ToString());
        }

        public Task<Assignment> Post() {
            return _isEditing ? _api.PutEditAssignment((ulong) _id, this) 
                              : _api.PostCreateAssignment(this);
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