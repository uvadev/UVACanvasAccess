using System.Collections.Generic;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Quizzes {
    
    internal class QuizSubmissionFileUploadResponseModel {
        
        [JsonProperty("attachments")]
        public IEnumerable<QuizSubmissionFileUploadAttachmentModel> Attachments { get; set; }
    }
    
    internal class QuizSubmissionFileUploadAttachmentModel {
        
        [JsonProperty("upload_url")]
        public string UploadUrl { get; set; }
        
        [JsonProperty("upload_params")]
        public Dictionary<string, string> UploadParams { get; set; }
    }
}
