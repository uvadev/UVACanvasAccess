using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Pages;
using UVACanvasAccess.Structures.Assignments;
using UVACanvasAccess.Structures.Roles;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Pages {
    
    [PublicAPI]
    public class Page : IPrettyPrint {
        private readonly Api _api;

        public string Url { get; }
        
        public string Title { get; }
        
        public DateTime CreatedAt { get; }
        
        public DateTime UpdatedAt { get; }
        
        public IEnumerable<Role> EditingRoles { get; }
        
        public ulong? LastEditedBy { get; }

        private string? _body;
        public string Body {
            get {
                if (_body == null) {
                    // todo
                }
                return _body;
            }
        }

        public bool Published { get; }
        
        public bool FrontPage { get; }
        
        public bool LockedForUser { get; }
        
        public LockInfo? LockInfo { get; }
        
        public string? LockExplanation { get; }

        internal Page(Api api, PageModel model) {
            _api = api;
            _body = model.Body;
            Url = model.Url;
            Title = model.Title;
            CreatedAt = model.CreatedAt;
            UpdatedAt = model.UpdatedAt;
            LastEditedBy = model.LastEditedBy;
            Published = model.Published;
            FrontPage = model.FrontPage;
            LockedForUser = model.LockedForUser;
            LockInfo = model.LockInfo.ConvertIfNotNull(m => new LockInfo(api, m));
            LockExplanation = model.LockExplanation;
            
            var roles = api.ListRoles().Result;
            EditingRoles = model.EditingRoles.Split(',')
                                             .Select(n => roles.First(r => r.Label == n));
        }

        public string ToPrettyString() {
            return "Page {" +
                   ($"\n{nameof(Url)}: {Url}," +
                   $"\n{nameof(Title)}: {Title}," +
                   $"\n{nameof(CreatedAt)}: {CreatedAt}," +
                   $"\n{nameof(UpdatedAt)}: {UpdatedAt}," +
                   $"\n{nameof(EditingRoles)}: {EditingRoles}," +
                   $"\n{nameof(LastEditedBy)}: {LastEditedBy}," +
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