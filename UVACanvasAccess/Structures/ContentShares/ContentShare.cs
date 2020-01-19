using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Exceptions;
using UVACanvasAccess.Model.ContentShares;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.ContentShares {

    [PublicAPI]
    public enum ContentShareType : byte {
        [ApiRepresentation("assignment")]
        Assignment,
        [ApiRepresentation("discussion_topic")]
        DiscussionTopic,
        [ApiRepresentation("page")]
        Page,
        [ApiRepresentation("quiz")]
        Quiz,
        [ApiRepresentation("module")]
        Module,
        [ApiRepresentation("module_item")]
        ModuleItem
    }
    
    [PublicAPI]
    public abstract class ContentShare : IPrettyPrint {
        private readonly Api _api;
        
        public ulong Id { get; }
        
        public string Name { get; }
        
        public ContentShareType ContentType { get; }
        
        public DateTime CreatedAt { get; }
        
        public DateTime? UpdatedAt { get; }
        
        public ulong? UserId { get; }
        
        public ulong? CourseId { get; }
        
        public bool ReadState { get; }
        
        public ulong? ContentExportId { get; }
        
        public abstract string ToPrettyString();

        private protected ContentShare(Api api, ContentShareModel model) {
            _api = api;
            Id = model.Id;
            Name = model.Name;
            ContentType = model.ContentType.ToApiRepresentedEnum<ContentShareType>()
                                           .Expect(() => new BadApiStateException($"ContentShare.ContentType was an unexpected value: {model.ContentType}"));
            CreatedAt = model.CreatedAt;
            UpdatedAt = model.UpdatedAt;
            UserId = model.UserId;
            CourseId = model.SourceCourse?.Id;
            ReadState = (model.ReadState ?? "") == "read";
            ContentExportId = model.ContentExport?.Id;
        }

        internal static ContentShare NewContentShare(Api api, ContentShareModel model) {
            if (model.Sender != null) {
                return new ContentShareWithSender(api, model);
            } else if (model.Receivers != null) {
                return new ContentShareWithReceivers(api, model);
            } else {
                throw new BadApiStateException("ContentShare.Sender & ContentShare.Receivers were both null.");
            }
        }
    }

    [PublicAPI]
    public sealed class ContentShareWithSender : ContentShare {
        public ulong SenderId { get; }

        internal ContentShareWithSender(Api api, ContentShareModel model) : base(api, model) {
            Debug.Assert(model.Sender != null, "model.Sender != null");
            SenderId = model.Sender.Id;
        }

        public override string ToPrettyString() {
            return "ContentShareWithSender {" +
                   ($"\n{nameof(Id)}: {Id}," +
                    $"\n{nameof(Name)}: {Name}," +
                    $"\n{nameof(ContentType)}: {ContentType.GetApiRepresentation()}," +
                    $"\n{nameof(CreatedAt)}: {CreatedAt}," +
                    $"\n{nameof(UpdatedAt)}: {UpdatedAt}," +
                    $"\n{nameof(UserId)}: {UserId}," +
                    $"\n{nameof(CourseId)}: {CourseId}," +
                    $"\n{nameof(ReadState)}: {ReadState}," +
                    $"\n{nameof(ContentExportId)}: {ContentExportId}," +
                    $"\n{nameof(SenderId)}: {SenderId}").Indent(4) + 
                   "\n}";
        }
    }

    [PublicAPI]
    public sealed class ContentShareWithReceivers : ContentShare {
        public IEnumerable<ulong> ReceiverIds { get; }

        internal ContentShareWithReceivers(Api api, ContentShareModel model) : base(api, model) {
            Debug.Assert(model.Receivers != null, "model.Receivers != null");
            ReceiverIds = model.Receivers.SelectNotNull(r => r.Id);
        }

        public override string ToPrettyString() {
            return "ContentShareWithReceivers {" +
                   ($"\n{nameof(Id)}: {Id}," +
                    $"\n{nameof(Name)}: {Name}," +
                    $"\n{nameof(ContentType)}: {ContentType.GetApiRepresentation()}," +
                    $"\n{nameof(CreatedAt)}: {CreatedAt}," +
                    $"\n{nameof(UpdatedAt)}: {UpdatedAt}," +
                    $"\n{nameof(UserId)}: {UserId}," +
                    $"\n{nameof(CourseId)}: {CourseId}," +
                    $"\n{nameof(ReadState)}: {ReadState}," +
                    $"\n{nameof(ContentExportId)}: {ContentExportId}," +
                    $"\n{nameof(ReceiverIds)}: {ReceiverIds.ToPrettyString()}").Indent(4) + 
                   "\n}";
        }
    }
}
