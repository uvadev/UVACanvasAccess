using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Pages;
using UVACanvasAccess.Structures.Assignments;
using UVACanvasAccess.Structures.Users;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Pages {
    
    [PublicAPI]
    public class Page : IPrettyPrint {
        private readonly Api _api;
        private readonly string _type;
        private readonly ulong _courseId;

        public string Url { get; }
        
        public string Title { get; }
        
        public DateTime CreatedAt { get; }
        
        public DateTime UpdatedAt { get; }
        
        public IEnumerable<string> EditingRoles { get; }
        
        [CanBeNull]
        public UserDisplay LastEditedBy { get; }

        [CanBeNull]
        private string _body;
        
        [NotNull]
        public string Body {
            get {
                if (_body == null) {
                    Debug.Assert(_type == "courses", nameof(_type) + " == " + _type + " != " + "courses");
                    Debug.Print($"DEBUG: The body of {Url} #{GetHashCode()} needs to be fetched.");
                    var specific = _api.GetCoursePage(_courseId, Url).Result;
                    _body = specific._body;
                }
                Debug.Assert(_body != null, nameof(_body) + " != null");
                return _body;
            }
        }

        public bool Published { get; }
        
        public bool FrontPage { get; }
        
        public bool LockedForUser { get; }
        
        [CanBeNull]
        public LockInfo LockInfo { get; }
        
        [CanBeNull]
        public string LockExplanation { get; }

        internal Page(Api api, PageModel model, [NotNull] string type, ulong courseId) {
            _api = api;
            _type = type;
            _body = model.Body;
            _courseId = courseId;
            Url = model.Url;
            Title = model.Title;
            CreatedAt = model.CreatedAt;
            UpdatedAt = model.UpdatedAt;
            LastEditedBy = model.LastEditedBy.ConvertIfNotNull(m => new UserDisplay(api, m));
            Published = model.Published;
            FrontPage = model.FrontPage;
            LockedForUser = model.LockedForUser;
            LockInfo = model.LockInfo.ConvertIfNotNull(m => new LockInfo(api, m));
            LockExplanation = model.LockExplanation;
            EditingRoles = model.EditingRoles.Split(',');
        }

        public string ToPrettyString() {
            return "Page {" +
                   ($"\n{nameof(Url)}: {Url}," +
                   $"\n{nameof(Title)}: {Title}," +
                   $"\n{nameof(CreatedAt)}: {CreatedAt}," +
                   $"\n{nameof(UpdatedAt)}: {UpdatedAt}," +
                   $"\n{nameof(EditingRoles)}: {EditingRoles.ToPrettyString()}," +
                   $"\n{nameof(LastEditedBy)}: {LastEditedBy?.ToPrettyString()}," +
                   $"\n{nameof(Body)}: {Body}," +
                   $"\n{nameof(Published)}: {Published}," +
                   $"\n{nameof(FrontPage)}: {FrontPage}," +
                   $"\n{nameof(LockedForUser)}: {LockedForUser}," +
                   $"\n{nameof(LockInfo)}: {LockInfo}," +
                   $"\n{nameof(LockExplanation)}: {LockExplanation}").Indent(4) +
                   "\n}";
        }
    }
}