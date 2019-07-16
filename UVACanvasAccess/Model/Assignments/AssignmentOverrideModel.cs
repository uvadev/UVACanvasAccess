using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Assignments {
    
    public class AssignmentOverrideModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("assignment_id")]
        public ulong AssignmentId { get; set; }
        
        [CanBeNull]
        [JsonProperty("student_ids")]
        public IEnumerable<ulong> StudentIds { get; set; }
        
        [JsonProperty("group_id")]
        public ulong? GroupId { get; set; }
        
        [JsonProperty("course_section_ids")]
        public ulong CourseSectionId { get; set; }
        
        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("due_at")]
        public DateTime? DueAt { get; set; }
        
        [JsonProperty("all_day")]
        public bool? AllDay { get; set; }
        
        [JsonProperty("all_day_date")]
        public DateTime? AllDayDate { get; set; }
        
        [JsonProperty("unlock_at")]
        public DateTime? UnlockAt { get; set; }
        
        [JsonProperty("lock_at")]
        public DateTime? LockAt { get; set; }
    }
}