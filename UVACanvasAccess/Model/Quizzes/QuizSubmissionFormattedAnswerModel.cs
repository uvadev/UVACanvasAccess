using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Quizzes {
    
    internal class QuizSubmissionFormattedAnswerModel {
        
        [JsonProperty("formatted_answer")]
        public string FormattedAnswer { get; set; }
    }
}
