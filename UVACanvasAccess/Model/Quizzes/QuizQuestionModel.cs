using System.Collections.Generic;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Quizzes {
    
    internal class QuizQuestionModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("quiz_id")]
        public ulong QuizId { get; set; }
        
        [JsonProperty("quiz_group_id")]
        public ulong? QuizGroupId { get; set; }
        
        [JsonProperty("position")]
        public uint? Position { get; set; }
        
        [JsonProperty("question_name")]
        public string QuestionName { get; set; }
        
        [JsonProperty("question_type")]
        public string QuestionType { get; set; }
        
        [JsonProperty("question_text")]
        public string QuestionText { get; set; }
        
        [JsonProperty("points_possible")]
        public decimal? PointsPossible { get; set; }
        
        [JsonProperty("correct_comments")]
        public string CorrectComments { get; set; }
        
        [JsonProperty("incorrect_comments")]
        public string IncorrectComments { get; set; }
        
        [JsonProperty("neutral_comments")]
        public string NeutralComments { get; set; }
        
        [JsonProperty("text_after_answers")]
        public string TextAfterAnswers { get; set; }
        
        [JsonProperty("answers")]
        public IEnumerable<QuizQuestionAnswerModel> Answers { get; set; }
    }
}
