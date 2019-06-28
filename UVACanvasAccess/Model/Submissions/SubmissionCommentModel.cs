using System;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UVACanvasAccess.Model.Users;

namespace UVACanvasAccess.Model.Submissions {
    
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SubmissionCommentModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("author_id")]
        public ulong AuthorId { get; set; }
        
        [JsonProperty("author_name")]
        public string AuthorName { get; set; }
        
        [JsonProperty("author")]
        public UserDisplayModel Author { get; set; }
        
        [JsonProperty("comment")]
        public string Comment { get; set; }
        
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        
        [JsonProperty("edited_at")]
        public DateTime? EditedAt { get; set; }
        
        [CanBeNull] 
        [JsonProperty("media_comment")]
        public MediaCommentModel MediaComment { get; set; }
    }
}