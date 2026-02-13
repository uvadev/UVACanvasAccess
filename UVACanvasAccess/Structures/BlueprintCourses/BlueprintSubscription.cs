using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.BlueprintCourses;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.BlueprintCourses {
    
    /// <summary>
    /// Associates a course with a blueprint.
    /// </summary>
    [PublicAPI]
    public class BlueprintSubscription : IPrettyPrint {
        private readonly Api api;
        
        /// <summary>
        /// The blueprint course subscription id.
        /// </summary>
        public ulong Id { get; }
        
        /// <summary>
        /// The blueprint template id the associated course is subscribed to.
        /// </summary>
        public ulong TemplateId { get; }
        
        /// <summary>
        /// The blueprint course subscribed to.
        /// </summary>
        [CanBeNull]
        public BlueprintCourse BlueprintCourse { get; }

        internal BlueprintSubscription(Api api, BlueprintSubscriptionModel model) {
            this.api = api;
            Id = model.Id;
            TemplateId = model.TemplateId;
            BlueprintCourse = model.BlueprintCourse.ConvertIfNotNull(m => new BlueprintCourse(api, m));
        }

        /// <inheritdoc />
        public string ToPrettyString() {
            return "BlueprintSubscription {" +
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(TemplateId)}: {TemplateId}," +
                   $"\n{nameof(BlueprintCourse)}: {BlueprintCourse?.ToPrettyString()}").Indent(4) + 
                   "\n}";
        }
    }
}
