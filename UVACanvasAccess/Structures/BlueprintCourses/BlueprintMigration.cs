using System;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.BlueprintCourses;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.BlueprintCourses {
    
    /// <summary>
    /// Represents a blueprint migration.
    /// </summary>
    [PublicAPI]
    public class BlueprintMigration : IPrettyPrint {
        private readonly Api api;
        
        /// <summary>
        /// The migration id.
        /// </summary>
        public ulong Id { get; }
        
        /// <summary>
        /// The template id, when querying a blueprint course.
        /// </summary>
        public ulong? TemplateId { get; }
        
        /// <summary>
        /// The subscription id, when querying a course associated with a blueprint.
        /// </summary>
        public ulong? SubscriptionId { get; }
        
        /// <summary>
        /// The id of the user who queued the migration.
        /// </summary>
        public ulong UserId { get; }
        
        /// <summary>
        /// The current state of the migration.
        /// </summary>
        public BlueprintMigrationState? WorkflowState { get; }
        
        /// <summary>
        /// When the migration was queued.
        /// </summary>
        public DateTime? CreatedAt { get; }
        
        /// <summary>
        /// When the exports began.
        /// </summary>
        public DateTime? ExportsStartedAt { get; }
        
        /// <summary>
        /// When the exports were completed and imports were queued.
        /// </summary>
        public DateTime? ImportsQueuedAt { get; }
        
        /// <summary>
        /// When the imports were completed.
        /// </summary>
        public DateTime? ImportsCompletedAt { get; }
        
        /// <summary>
        /// The user-specified comment describing changes in this migration.
        /// </summary>
        [CanBeNull]
        public string Comment { get; }

        internal BlueprintMigration(Api api, BlueprintMigrationModel model) {
            this.api = api;
            Id = model.Id;
            TemplateId = model.TemplateId;
            SubscriptionId = model.SubscriptionId;
            UserId = model.UserId;
            WorkflowState = model.WorkflowState?.ToApiRepresentedEnum<BlueprintMigrationState>();
            CreatedAt = model.CreatedAt;
            ExportsStartedAt = model.ExportsStartedAt;
            ImportsQueuedAt = model.ImportsQueuedAt;
            ImportsCompletedAt = model.ImportsCompletedAt;
            Comment = model.Comment;
        }

        /// <inheritdoc />
        public string ToPrettyString() {
            return "BlueprintMigration {" +
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(TemplateId)}: {TemplateId}," +
                   $"\n{nameof(SubscriptionId)}: {SubscriptionId}," +
                   $"\n{nameof(UserId)}: {UserId}," +
                   $"\n{nameof(WorkflowState)}: {WorkflowState}," +
                   $"\n{nameof(CreatedAt)}: {CreatedAt}," +
                   $"\n{nameof(ExportsStartedAt)}: {ExportsStartedAt}," +
                   $"\n{nameof(ImportsQueuedAt)}: {ImportsQueuedAt}," +
                   $"\n{nameof(ImportsCompletedAt)}: {ImportsCompletedAt}," +
                   $"\n{nameof(Comment)}: {Comment}").Indent(4) + 
                   "\n}";
        }
    }
}
