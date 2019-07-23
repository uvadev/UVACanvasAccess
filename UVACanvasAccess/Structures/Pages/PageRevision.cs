using System;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Pages;
using UVACanvasAccess.Structures.Users;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Pages {
    
    [PublicAPI]
    public class PageRevision : IPrettyPrint {
        private readonly Api _api;
        
        public ulong RevisionId { get; }
        
        public DateTime UpdatedAt { get; }
        
        public bool Latest { get; }
        
        [CanBeNull]
        public UserDisplay EditedBy { get; }
        
        // todo computed get for Url, Title, and Body, which may be absent
        
        [CanBeNull]
        public string Url { get; }
        
        [CanBeNull]
        public string Title { get; }
        
        [CanBeNull]
        public string Body { get; }

        internal PageRevision(Api api, PageRevisionModel model) {
            _api = api;
            RevisionId = model.RevisionId;
            UpdatedAt = model.UpdatedAt;
            Latest = model.Latest;
            EditedBy = model.EditedBy.ConvertIfNotNull(m => new UserDisplay(api, m));
            Url = model.Url;
            Title = model.Title;
            Body = model.Body;
        }

        public string ToPrettyString() {
            return "PageRevision {" + 
                   ($"\n{nameof(RevisionId)}: {RevisionId}," +
                   $"\n{nameof(UpdatedAt)}: {UpdatedAt}," +
                   $"\n{nameof(Latest)}: {Latest}," +
                   $"\n{nameof(EditedBy)}: {EditedBy?.ToPrettyString()}," +
                   $"\n{nameof(Url)}: {Url}," +
                   $"\n{nameof(Title)}: {Title}," +
                   $"\n{nameof(Body)}: {Body}").Indent(4) + 
                   "\n}";
        }
    }
}