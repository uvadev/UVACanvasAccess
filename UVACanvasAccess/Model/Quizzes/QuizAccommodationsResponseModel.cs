using System.Collections.Generic;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Quizzes {
    internal class QuizAccommodationsResponseModel {
        [JsonProperty("message")]
        public string Message { get; set; }
        
        [JsonProperty("successful")]
        public IEnumerable<SuccessfulQuizAccommodationsResponseItemModel> Successful { get; set; }
        
        [JsonProperty("failed")]
        public IEnumerable<FailedQuizAccommodationsResponseItemModel> Failed { get; set; }
    }

    internal class SuccessfulQuizAccommodationsResponseItemModel {
        [JsonProperty("user_id")]
        public ulong UserId { get; set; }
    }
    
    internal class FailedQuizAccommodationsResponseItemModel {
        [JsonProperty("user_id")]
        public ulong UserId { get; set; }
        
        [JsonProperty("error")]
        public string Error { get; set; }
    }
}
