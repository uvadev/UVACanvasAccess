using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Quizzes {
    
    internal class QuizQuestionGroupModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("quiz_id")]
        public ulong QuizId { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("position")]
        public uint? Position { get; set; }
        
        [JsonProperty("pick_count")]
        public uint? PickCount { get; set; }
        
        [JsonProperty("question_points")]
        public decimal? QuestionPoints { get; set; }
        
        [JsonProperty("assessment_question_bank_id")]
        public ulong? AssessmentQuestionBankId { get; set; }
        
        [JsonProperty("question_bank_name")]
        public string QuestionBankName { get; set; }
    }
}
