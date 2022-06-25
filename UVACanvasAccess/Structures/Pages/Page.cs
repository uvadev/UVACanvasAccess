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
        private readonly Api api;
        private readonly string type;
        private readonly ulong courseId;

        public string Url { get; }
        
        public string Title { get; }
        
        public DateTime CreatedAt { get; }
        
        public DateTime UpdatedAt { get; }
        
        public PageRoles EditingRoles { get; }
        
        [CanBeNull]
        public UserDisplay LastEditedBy { get; }

        [CanBeNull]
        private string body;
        
        [NotNull]
        public string Body {
            get {
                if (body == null) {
                    Debug.Assert(type == "courses", nameof(type) + " == " + type + " != " + "courses");
                    Debug.Print($"DEBUG: The body of {Url} #{GetHashCode()} needs to be fetched.");
                    var specific = api.GetCoursePage(courseId, Url).Result;
                    body = specific.body;
                }
                Debug.Assert(body != null, nameof(body) + " != null");
                return body;
            }
        }

        public bool Published { get; }
        
        public bool FrontPage { get; }
        
        public bool LockedForUser { get; }
        
        [CanBeNull]
        public LockInfo LockInfo { get; }
        
        [CanBeNull]
        public string LockExplanation { get; }

        public IAsyncEnumerable<PageRevision> StreamRevisionHistory() {
            Debug.Assert(type == "courses", nameof(type) + " == " + type + " != " + "courses");
            return api.StreamCoursePageRevisionHistory(courseId, Url);
        }

        internal Page(Api api, PageModel model, [NotNull] string type, ulong courseId) {
            this.api = api;
            this.type = type;
            body = model.Body;
            this.courseId = courseId;
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
            EditingRoles = model.EditingRoles?.Split(',')
                                              .ToApiRepresentedFlagsEnum<PageRoles>() 
                                             ?? default;
        }

        /// <inheritdoc />
        public string ToPrettyString() {
            return "Page {" +
                   ($"\n{nameof(Url)}: {Url}," +
                   $"\n{nameof(Title)}: {Title}," +
                   $"\n{nameof(CreatedAt)}: {CreatedAt}," +
                   $"\n{nameof(UpdatedAt)}: {UpdatedAt}," +
                   $"\n{nameof(EditingRoles)}: {EditingRoles.GetFlagsApiRepresentations().ToPrettyString()}," +
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