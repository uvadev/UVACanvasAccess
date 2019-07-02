using UVACanvasAccess.Model.Submissions;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Submissions {
    
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
    // ReSharper disable MemberCanBePrivate.Global
    public class MediaComment : IPrettyPrint {
        private readonly Api _api;
        
        public string ContentType { get; }
        
        public string DisplayName { get; }
        
        public string MediaId { get; }
        
        public string MediaType { get; }
        
        public string Url { get; }

        public MediaComment(Api api, MediaCommentModel model) {
            _api = api;
            ContentType = model.ContentType;
            DisplayName = model.DisplayName;
            MediaId = model.MediaId;
            MediaType = model.MediaType;
            Url = model.Url;
        }

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