using System.Linq;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.BlueprintCourses;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.BlueprintCourses {
    
    /// <summary>
    /// Lists an associated course that did not receive a change propagated from a blueprint.
    /// </summary>
    [PublicAPI]
    public class ExceptionRecord : IPrettyPrint {
        private readonly Api api;
        
        /// <summary>
        /// The id of the associated course.
        /// </summary>
        public ulong CourseId { get; }
        
        /// <summary>
        /// A list of change classes in the associated course's copy of the item that prevented a blueprint change.
        /// </summary>
        public BlueprintRestrictionTypes ConflictingChanges { get; }

        internal ExceptionRecord(Api api, ExceptionRecordModel model) {
            this.api = api;
            CourseId = model.CourseId;
            ConflictingChanges = (model.ConflictingChanges ?? Enumerable.Empty<string>()).ToApiRepresentedFlagsEnum<BlueprintRestrictionTypes>();
        }

        /// <inheritdoc />
        public string ToPrettyString() {
            return "ExceptionRecord {" +
                   ($"\n{nameof(CourseId)}: {CourseId}," +
                   $"\n{nameof(ConflictingChanges)}: {ConflictingChanges.GetFlagsApiRepresentations().ToPrettyString()}").Indent(4) + 
                   "\n}";
        }
    }
}
