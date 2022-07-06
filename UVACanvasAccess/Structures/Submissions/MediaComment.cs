using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Submissions;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Submissions {
    
    /// <summary>
    /// Represents a media/attachment comment.
    /// </summary>
    [PublicAPI]
    public class MediaComment : IPrettyPrint {
        private readonly Api api;
        
        /// <summary>
        /// The content type.
        /// </summary>
        public string ContentType { get; }
        
        /// <summary>
        /// The display name.
        /// </summary>
        public string DisplayName { get; }
        
        /// <summary>
        /// The media ID.
        /// </summary>
        public string MediaId { get; }
        
        /// <summary>
        /// The media type.
        /// </summary>
        public string MediaType { get; }
        
        /// <summary>
        /// The URL.
        /// </summary>
        public string Url { get; }

        internal MediaComment(Api api, MediaCommentModel model) {
            this.api = api;
            ContentType = model.ContentType;
            DisplayName = model.DisplayName;
            MediaId = model.MediaId;
            MediaType = model.MediaType;
            Url = model.Url;
        }

        /// <inheritdoc />
        public string ToPrettyString() {
            return "MediaComment {" +
                   ($"\n{nameof(ContentType)}: {ContentType}," +
                   $"\n{nameof(DisplayName)}: {DisplayName}," +
                   $"\n{nameof(MediaId)}: {MediaId}," +
                   $"\n{nameof(MediaType)}: {MediaType}," +
                   $"\n{nameof(Url)}: {Url}").Indent(4) + 
                   "\n}";
        }
    }
}