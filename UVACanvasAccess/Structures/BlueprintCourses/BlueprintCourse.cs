using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.BlueprintCourses;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.BlueprintCourses {
    
    /// <summary>
    /// Represents a blueprint course summary.
    /// </summary>
    [PublicAPI]
    public class BlueprintCourse : IPrettyPrint {
        private readonly Api api;
        
        /// <summary>
        /// The course id.
        /// </summary>
        public ulong Id { get; }
        
        /// <summary>
        /// The course name.
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// The course code.
        /// </summary>
        public string CourseCode { get; }
        
        /// <summary>
        /// The term name.
        /// </summary>
        [CanBeNull]
        public string TermName { get; }

        internal BlueprintCourse(Api api, BlueprintCourseModel model) {
            this.api = api;
            Id = model.Id;
            Name = model.Name;
            CourseCode = model.CourseCode;
            TermName = model.TermName;
        }

        /// <inheritdoc />
        public string ToPrettyString() {
            return "BlueprintCourse {" +
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(Name)}: {Name}," +
                   $"\n{nameof(CourseCode)}: {CourseCode}," +
                   $"\n{nameof(TermName)}: {TermName}").Indent(4) + 
                   "\n}";
        }
    }
}
