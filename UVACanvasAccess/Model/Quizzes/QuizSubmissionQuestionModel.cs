using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UVACanvasAccess.Model.Quizzes {
    
    internal class QuizSubmissionQuestionModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("flagged")]
        public bool? Flagged { get; set; }
        
        [JsonProperty("answer")]
        public JToken Answer { get; set; }
        
        [JsonProperty("answers")]
        public IEnumerable<QuizQuestionAnswerModel> Answers { get; set; }
        
        [JsonProperty("quiz_question")]
        public QuizQuestionModel QuizQuestion { get; set; }
    }

    internal class QuizSubmissionQuestionListModel {
        
        [JsonProperty("quiz_submission_questions")]
        public IEnumerable<QuizSubmissionQuestionModel> QuizSubmissionQuestions { get; set; }
    }
}
