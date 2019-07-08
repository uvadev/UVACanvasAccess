using System;
using Newtonsoft.Json;
using UVACanvasAccess.Model.Submissions;

namespace UVACanvasAccess.Model.Gradebook {

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SubmissionVersionModel : SubmissionModel {
        
        [JsonProperty("assignment_name")]
        public string AssignmentName { get; set; }
        
        [JsonProperty("current_grade")]
        public string CurrentGrade { get; set; }
        
        [JsonProperty("current_graded_at")]
        public DateTime? CurrentGradedAt { get; set; }
        
        [JsonProperty("current_grader")]
        public string CurrentGrader { get; set; }
        
        [JsonProperty("new_grade")]
        public string NewGrade { get; set; }
        
        [JsonProperty("new_graded_at")]
        public DateTime? NewGradedAt { get; set; }
        
        [JsonProperty("new_grader")]
        public string NewGrader { get; set; }
        
        [JsonProperty("previous_grade")]
        public string PreviousGrade { get; set; }
        
        [JsonProperty("previous_graded_at")]
        public DateTime? PreviousGradedAt { get; set; }
        
        [JsonProperty("previous_grader")]
        public string PreviousGrader { get; set; }
    }
}