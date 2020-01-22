using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Exceptions;
using UVACanvasAccess.Model.ContentShares;
using UVACanvasAccess.Structures.Users;
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
    
    /// <summary>
    /// Represents a content share between a sender and one or more receivers.
    /// </summary>
    [PublicAPI]
    public abstract class ContentShare : IPrettyPrint {
        private readonly Api _api;
        
        /// <summary>
        /// The id of the share (for the current user).
        /// </summary>
        public ulong Id { get; }
        
        /// <summary>
        /// The name of the shared content.
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// The type of the shared content.
        /// </summary>
        public ContentShareType ContentType { get; }
        
        /// <summary>
        /// When the content was initially shared.
        /// </summary>
        public DateTime CreatedAt { get; }
        
        /// <summary>
        /// When the content was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; }
        
        /// <summary>
        /// The id of the user who sent or received the content, i.e., the subject of the api call which returned this object.
        /// </summary>
        public ulong? UserId { get; }
        
        /// <summary>
        /// The id of the course this content was originally shared from. 
        /// </summary>
        public ulong? CourseId { get; }
        
        /// <summary>
        /// Whether or not the recipient has viewed the share.
        /// </summary>
        public bool ReadState { get; }
        
        /// <summary>
        /// The content export id associated with this share.
        /// </summary>
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

    /// <summary>
    /// Represents a content share along with the sender.
    /// </summary>
    [PublicAPI]
    public sealed class ContentShareWithSender : ContentShare {
        
        /// <summary>
        /// The sender of this share.
        /// </summary>
        public ShortUser Sender { get; }

        internal ContentShareWithSender(Api api, ContentShareModel model) : base(api, model) {
            Debug.Assert(model.Sender != null, "model.Sender != null");
            Sender = new ShortUser(api, model.Sender);
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
                    $"\n{nameof(Sender)}: {Sender.ToPrettyString()}").Indent(4) + 
                   "\n}";
        }
    }

    /// <summary>
    /// Represents a content share along with the list of receivers.
    /// </summary>
    [PublicAPI]
    public sealed class ContentShareWithReceivers : ContentShare {
        
        /// <summary>
        /// The receivers of this share. 
        /// </summary>
        public IEnumerable<ShortUser> Receivers { get; }

        internal ContentShareWithReceivers(Api api, ContentShareModel model) : base(api, model) {
            Debug.Assert(model.Receivers != null, "model.Receivers != null");
            Receivers = model.Receivers.SelectNotNull(r => new ShortUser(api, r));
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
                    $"\n{nameof(Receivers)}: {Receivers.ToPrettyString()}").Indent(4) + 
                   "\n}";
        }
    }
}
