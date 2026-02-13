using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.BlueprintCourses;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.BlueprintCourses {
    
    /// <summary>
    /// Represents a set of restrictions on editing for copied objects in associated courses.
    /// </summary>
    [PublicAPI]
    public class BlueprintRestriction : IPrettyPrint {
        private readonly Api api;
        
        /// <summary>
        /// Restriction on main content (e.g. title, description).
        /// </summary>
        public bool? Content { get; }
        
        /// <summary>
        /// Restriction on points possible for assignments and graded learning objects.
        /// </summary>
        public bool? Points { get; }
        
        /// <summary>
        /// Restriction on due dates for assignments and graded learning objects.
        /// </summary>
        public bool? DueDates { get; }
        
        /// <summary>
        /// Restriction on availability dates for an object.
        /// </summary>
        public bool? AvailabilityDates { get; }

        internal BlueprintRestriction(Api api, BlueprintRestrictionModel model) {
            this.api = api;
            Content = model.Content;
            Points = model.Points;
            DueDates = model.DueDates;
            AvailabilityDates = model.AvailabilityDates;
        }

        /// <inheritdoc />
        public string ToPrettyString() {
            return "BlueprintRestriction {" +
                   ($"\n{nameof(Content)}: {Content}," +
                   $"\n{nameof(Points)}: {Points}," +
                   $"\n{nameof(DueDates)}: {DueDates}," +
                   $"\n{nameof(AvailabilityDates)}: {AvailabilityDates}").Indent(4) + 
                   "\n}";
        }
    }
}
