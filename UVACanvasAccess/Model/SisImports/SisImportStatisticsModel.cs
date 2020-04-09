using JetBrains.Annotations;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.SisImports {

    internal class SisImportStatisticsModel {
        
        [JsonProperty("total_state_changes")]
        public ulong TotalStateChanges { get; set; }
        
        [JsonProperty]
        [CanBeNull]
        public SisImportStatisticModel Account { get; set; }
        
        [JsonProperty]
        [CanBeNull]
        public SisImportStatisticModel EnrollmentTerm { get; set; }
        
        [JsonProperty]
        [CanBeNull]
        public SisImportStatisticModel CommunicationChannel { get; set; }
        
        [JsonProperty]
        [CanBeNull]
        public SisImportStatisticModel AbstractCourse { get; set; }
        
        [JsonProperty]
        [CanBeNull]
        public SisImportStatisticModel Course { get; set; }
        
        [JsonProperty]
        [CanBeNull]
        public SisImportStatisticModel CourseSection { get; set; }
        
        [JsonProperty]
        [CanBeNull]
        public SisImportStatisticModel Enrollment { get; set; }
        
        [JsonProperty]
        [CanBeNull]
        public SisImportStatisticModel GroupCategory { get; set; }
        
        [JsonProperty]
        [CanBeNull]
        public SisImportStatisticModel Group { get; set; }
        
        [JsonProperty]
        [CanBeNull]
        public SisImportStatisticModel GroupMembership { get; set; }
        
        [JsonProperty]
        [CanBeNull]
        public SisImportStatisticModel Pseudonym { get; set; }
        
        [JsonProperty]
        [CanBeNull]
        public SisImportStatisticModel UserObserver { get; set; }
        
        [JsonProperty]
        [CanBeNull]
        public SisImportStatisticModel AccountUser { get; set; }
    }
}
