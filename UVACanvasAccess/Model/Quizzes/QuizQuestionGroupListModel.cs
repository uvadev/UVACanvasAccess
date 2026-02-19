using System.Collections.Generic;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Quizzes {
    
    internal class QuizQuestionGroupListModel {
        
        [JsonProperty("quiz_groups")]
        public IEnumerable<QuizQuestionGroupModel> QuizGroups { get; set; }
    }
}
