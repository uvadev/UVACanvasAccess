using System;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.BlueprintCourses;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.BlueprintCourses {
    
    /// <summary>
    /// Represents a blueprint template.
    /// </summary>
    [PublicAPI]
    public class BlueprintTemplate : IPrettyPrint {
        private readonly Api api;
        
        /// <summary>
        /// The template id.
        /// </summary>
        public ulong Id { get; }
        
        /// <summary>
        /// The id of the course this template belongs to.
        /// </summary>
        public ulong CourseId { get; }
        
        /// <summary>
        /// When the last export was completed.
        /// </summary>
        public DateTime? LastExportCompletedAt { get; }
        
        /// <summary>
        /// The number of associated courses for the template.
        /// </summary>
        public uint? AssociatedCourseCount { get; }
        
        /// <summary>
        /// Details of the latest migration.
        /// </summary>
        [CanBeNull]
        public BlueprintMigration LatestMigration { get; }

        internal BlueprintTemplate(Api api, BlueprintTemplateModel model) {
            this.api = api;
            Id = model.Id;
            CourseId = model.CourseId;
            LastExportCompletedAt = model.LastExportCompletedAt;
            AssociatedCourseCount = model.AssociatedCourseCount;
            LatestMigration = model.LatestMigration.ConvertIfNotNull(m => new BlueprintMigration(api, m));
        }

        /// <inheritdoc />
        public string ToPrettyString() {
            return "BlueprintTemplate {" +
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(CourseId)}: {CourseId}," +
                   $"\n{nameof(LastExportCompletedAt)}: {LastExportCompletedAt}," +
                   $"\n{nameof(AssociatedCourseCount)}: {AssociatedCourseCount}," +
                   $"\n{nameof(LatestMigration)}: {LatestMigration?.ToPrettyString()}").Indent(4) + 
                   "\n}";
        }
    }
}
