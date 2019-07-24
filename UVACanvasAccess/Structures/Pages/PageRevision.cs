using System;
using System.Diagnostics;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Pages;
using UVACanvasAccess.Structures.Users;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Pages {
    
    [PublicAPI]
    public class PageRevision : IPrettyPrint {
        private readonly Api _api;
        private readonly string _type;
        private readonly ulong _baseId;

        public ulong RevisionId { get; }
        
        public DateTime UpdatedAt { get; }
        
        public bool Latest { get; }
        
        [CanBeNull]
        public UserDisplay EditedBy { get; }
        
        [NotNull] 
        public string Url { get; }

        private void UpdateAbsentFields() {
            Debug.Assert(_type == "courses", nameof(_type) + " == " + _type + " != " + "courses");
            Debug.Print($"DEBUG: The absent fields of revision {RevisionId} of {Url} #{GetHashCode()} need to be fetched.");

            var specific = _api.GetCoursePageRevision(_baseId, Url, RevisionId).Result;

            if (specific._title != null) {
                Debug.Print($"DEBUG: Got Title for revision {RevisionId} of {Url} #{GetHashCode()}.");
                _title = specific._title;
            }

            if (specific._body != null) {
                Debug.Print($"DEBUG: Got Body for revision {RevisionId} of {Url} #{GetHashCode()}.");
                _body = specific._body;
            }
        }

        [CanBeNull] 
        private string _title;
        
        [NotNull]
        public string Title {
            get {
                if (_title == null) {
                    UpdateAbsentFields();
                }
                Debug.Assert(_title != null);
                return _title;
            }
        }

        [CanBeNull] 
        private string _body;
        
        [NotNull]
        public string Body {
            get {
                if (_body == null) {
                    UpdateAbsentFields();
                }
                Debug.Assert(_body != null);
                return _body;
            }
        }

        internal PageRevision(Api api, PageRevisionModel model, string type, ulong baseId, string url) {
            _api = api;
            RevisionId = model.RevisionId;
            UpdatedAt = model.UpdatedAt;
            Latest = model.Latest;
            EditedBy = model.EditedBy.ConvertIfNotNull(m => new UserDisplay(api, m));
            if (url != null) {
                Url = url;
            } else if (model.Url != null) {
                Url = model.Url;
            } else {
                throw new ArgumentNullException(nameof(url), "One of url param or model Url field must be non-null.");
            }
            _title = model.Title;
            _body = model.Body;
            _type = type;
            _baseId = baseId;
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