using System.Collections.Generic;
using Newtonsoft.Json;
using UVACanvasAccess.Model.Assignments;

namespace UVACanvasAccess.Model.Quizzes {
    
    internal class QuizAssignmentOverridesResponseModel {
        
        [JsonProperty("quiz_assignment_overrides")]
        public QuizAssignmentOverridesModel QuizAssignmentOverrides { get; set; }
    }

    internal class QuizAssignmentOverridesModel {
        
        [JsonProperty("quiz_id")]
        public ulong QuizId { get; set; }
        
        [JsonProperty("due_dates")]
        public IEnumerable<AssignmentDateModel> DueDates { get; set; }
        
        [JsonProperty("all_dates")]
        public IEnumerable<AssignmentDateModel> AllDates { get; set; }
    }
}
