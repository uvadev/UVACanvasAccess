using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UVACanvasAccess.Model.Courses;
using UVACanvasAccess.Model.Users;

namespace UVACanvasAccess.Model.ContentShares {
    
    internal class ContentShareModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("content_type")]
        public string ContentType { get; set; }
        
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        
        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        
        [JsonProperty("user_id")]
        public ulong? UserId { get; set; }

        [CanBeNull] 
        [JsonProperty("sender")]
        public ShortUserModel Sender { get; set; }
        
        [CanBeNull]
        [JsonProperty("receivers")]
        public IEnumerable<ShortUserModel> Receivers { get; set; }
        
        [CanBeNull]
        [JsonProperty("source_course")]
        public ShortCourseModel SourceCourse { get; set; }
        
        [JsonProperty("read_state")]
        public string ReadState { get; set; }
        
        [JsonProperty("content_export")]
        public ContentExportIdModel? ContentExport { get; set; }
    }
}
