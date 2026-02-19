using System;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Quizzes {
    
    internal class QuizQuestionAnswerModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("answer_text")]
        public string AnswerText { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
        
        [JsonProperty("answer_weight")]
        public decimal? AnswerWeight { get; set; }
        
        [JsonProperty("answer_comments")]
        public string AnswerComments { get; set; }
        
        [JsonProperty("text_after_answers")]
        public string TextAfterAnswers { get; set; }
        
        [JsonProperty("answer_match_left")]
        public string AnswerMatchLeft { get; set; }
        
        [JsonProperty("answer_match_right")]
        public string AnswerMatchRight { get; set; }
        
        [JsonProperty("matching_answer_incorrect_matches")]
        public string MatchingAnswerIncorrectMatches { get; set; }
        
        [JsonProperty("numerical_answer_type")]
        public string NumericalAnswerType { get; set; }
        
        [JsonProperty("exact")]
        public decimal? Exact { get; set; }
        
        [JsonProperty("margin")]
        public decimal? Margin { get; set; }
        
        [JsonProperty("approximate")]
        public decimal? Approximate { get; set; }
        
        [JsonProperty("precision")]
        public decimal? Precision { get; set; }
        
        [JsonProperty("start")]
        public decimal? Start { get; set; }
        
        [JsonProperty("end")]
        public decimal? End { get; set; }
        
        [JsonProperty("blank_id")]
        public ulong? BlankId { get; set; }
    }
}
