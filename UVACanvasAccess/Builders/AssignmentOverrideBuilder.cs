using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Structures.Assignments;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Builders {

    /// <summary>
    /// A class used to create or edit assignment overrides using the builder pattern.
    /// When all desired fields are set, call <see cref="Post"/> to execute the operation.
    /// </summary>
    public class AssignmentOverrideBuilder {
        private readonly Api _api;
        
        internal ulong CourseId { get; }
        internal ulong AssignmentId { get; }

        internal Dictionary<string, string> Fields { get; } = new Dictionary<string, string>();
        
        private readonly List<KeyValuePair<string, string>> _arrayFields = new List<KeyValuePair<string, string>>();
        
        internal ILookup<string, string> ArrayFields => _arrayFields.Distinct()
                                                                    .ToLookup(kv => kv.Key,
                                                                              kv => kv.Value);

        internal AssignmentOverrideBuilder(Api api, ulong courseId, ulong assignmentId) {
            _api = api;
            CourseId = courseId;
            AssignmentId = assignmentId;
        }

        /// <summary>
        /// The individual students that this override targets.
        /// </summary>
        /// <param name="studentIds"></param>
        /// <returns>This builder.</returns>
        public AssignmentOverrideBuilder WithStudents(IEnumerable<ulong> studentIds) {
            foreach (var id in studentIds) {
                PutArr("student_ids", id.ToString());
            }
            return this;
        }
        
        /// <summary>
        /// The individual students that this override targets.
        /// </summary>
        /// <param name="studentIds"></param>
        /// <returns>This builder.</returns>
        public AssignmentOverrideBuilder WithStudents(params ulong[] studentIds) {
            foreach (var id in studentIds) {
                PutArr("student_ids", id.ToString());
            }
            return this;
        }

        /// <summary>
        /// The title of this override.
        /// </summary>
        /// <param name="title"></param>
        /// <returns>This builder.</returns>
        public AssignmentOverrideBuilder WithTitle(string title) {
            return Put("title", title);
        }

        /// <summary>
        /// The group that this override targets.
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns>This builder.</returns>
        public AssignmentOverrideBuilder WithGroup(ulong groupId) {
            return Put("group_id", groupId.ToString());
        }

        /// <summary>
        /// The section that this override targets.
        /// </summary>
        /// <param name="sectionId"></param>
        /// <returns>This builder.</returns>
        public AssignmentOverrideBuilder WithCourseSection(ulong sectionId) {
            return Put("course_section_id", sectionId.ToString());
        }

        /// <summary>
        /// The due date of the overridden assignment.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>This builder.</returns>
        public AssignmentOverrideBuilder WithDueDate(DateTime dateTime) {
            return Put("due_at", dateTime.ToIso8601Date());
        }
        
        /// <summary>
        /// The lock date of the overridden assignment.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>This builder.</returns>
        public AssignmentOverrideBuilder WithLockDate(DateTime dateTime) {
            return Put("lock_at", dateTime.ToIso8601Date());
        }
        
        /// <summary>
        /// The unlock date of the overridden assignment.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>This builder.</returns>
        public AssignmentOverrideBuilder WithUnlockDate(DateTime dateTime) {
            return Put("unlock_at", dateTime.ToIso8601Date());
        }
        
        private AssignmentOverrideBuilder Put(string key, string s) {
            Fields[$"assignment_override[{key}]"] = s;
            return this;
        }

        private AssignmentOverrideBuilder PutArr(string key, string s) {
            _arrayFields.Add(new KeyValuePair<string, string>($"assignment_override[{key}][]", s));
            return this;
        }

        public Task<AssignmentOverride> Post() {
            return _api.PostAssignmentOverride(this);
        }
    }
}