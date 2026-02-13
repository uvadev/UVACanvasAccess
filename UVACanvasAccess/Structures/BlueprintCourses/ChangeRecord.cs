using System.Collections.Generic;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.BlueprintCourses;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.BlueprintCourses {
    
    /// <summary>
    /// Describes a learning object change propagated to associated courses from a blueprint course.
    /// </summary>
    [PublicAPI]
    public class ChangeRecord : IPrettyPrint {
        private readonly Api api;
        
        /// <summary>
        /// The id of the learning object changed in the blueprint course.
        /// </summary>
        public ulong AssetId { get; }
        
        /// <summary>
        /// The type of the learning object changed in the blueprint course.
        /// </summary>
        public BlueprintAssetType? AssetType { get; }
        
        /// <summary>
        /// The name of the learning object changed in the blueprint course.
        /// </summary>
        public string AssetName { get; }
        
        /// <summary>
        /// The type of change.
        /// </summary>
        public BlueprintChangeType? ChangeType { get; }
        
        /// <summary>
        /// The URL of the changed object.
        /// </summary>
        public string HtmlUrl { get; }
        
        /// <summary>
        /// Whether the object is locked in the blueprint.
        /// </summary>
        public bool? Locked { get; }
        
        /// <summary>
        /// Exception records for linked courses that did not receive this update.
        /// </summary>
        public IEnumerable<ExceptionRecord> Exceptions { get; }

        internal ChangeRecord(Api api, ChangeRecordModel model) {
            this.api = api;
            AssetId = model.AssetId;
            AssetType = model.AssetType?.ToApiRepresentedEnum<BlueprintAssetType>();
            AssetName = model.AssetName;
            ChangeType = model.ChangeType?.ToApiRepresentedEnum<BlueprintChangeType>();
            HtmlUrl = model.HtmlUrl;
            Locked = model.Locked;
            Exceptions = model.Exceptions.SelectNotNull(m => new ExceptionRecord(api, m));
        }

        /// <inheritdoc />
        public string ToPrettyString() {
            return "ChangeRecord {" +
                   ($"\n{nameof(AssetId)}: {AssetId}," +
                   $"\n{nameof(AssetType)}: {AssetType}," +
                   $"\n{nameof(AssetName)}: {AssetName}," +
                   $"\n{nameof(ChangeType)}: {ChangeType}," +
                   $"\n{nameof(HtmlUrl)}: {HtmlUrl}," +
                   $"\n{nameof(Locked)}: {Locked}," +
                   $"\n{nameof(Exceptions)}: {Exceptions.ToPrettyString()}").Indent(4) + 
                   "\n}";
        }
    }
}
