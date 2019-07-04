using System;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Submissions;
using UVACanvasAccess.Model.Users;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Submissions {
    
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
    // ReSharper disable MemberCanBePrivate.Global
    public class SubmissionComment : IPrettyPrint {
        private readonly Api _api;
        
        public ulong Id { get; }
        
        public ulong AuthorId { get; }
        
        public string AuthorName { get; }
        
        public UserDisplayModel Author { get; }
        
        public string Comment { get; }
        
        public DateTime CreatedAt { get; }
        
        public DateTime? EditedAt { get; }
        
        [CanBeNull]
        public MediaComment MediaComment { get; }

        public SubmissionComment(Api api, SubmissionCommentModel model) {
            _api = api;
            Id = model.Id;
            AuthorId = model.AuthorId;
            AuthorName = model.AuthorName;
            Author = model.Author;
            Comment = model.Comment;
            CreatedAt = model.CreatedAt;
            EditedAt = model.EditedAt;
            MediaComment = model.MediaComment == null ? null
                                                      : new MediaComment(api, model.MediaComment);
        }

        public string ToPrettyString() {
            return "SubmissionComment {" + 
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(AuthorId)}: {AuthorId}," +
                   $"\n{nameof(AuthorName)}: {AuthorName}," +
                   $"\n{nameof(Author)}: {Author}," +
                   $"\n{nameof(Comment)}: {Comment}," +
                   $"\n{nameof(CreatedAt)}: {CreatedAt}," +
                   $"\n{nameof(EditedAt)}: {EditedAt}," +
                   $"\n{nameof(MediaComment)}: {MediaComment}").Indent(4) +
                   "\n}";
        }
    }
}