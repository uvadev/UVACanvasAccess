using JetBrains.Annotations;
using UVACanvasAccess.Model.Analytics;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Analytics {
    
    /// <summary>
    /// Represents numerical department-level statistics.
    /// </summary>
    [PublicAPI]
    public class DepartmentStatistics : IPrettyPrint {
        
        /// <summary>
        /// The number of subaccounts.
        /// </summary>
        public ulong Subaccounts { get; }

        /// <summary>
        /// The number of teachers.
        /// </summary>
        public ulong Teachers { get; }

        /// <summary>
        /// The number of students.
        /// </summary>
        public ulong Students { get; }

        /// <summary>
        /// The number of discussion topics.
        /// </summary>
        public ulong DiscussionTopics { get; }
        
        /// <summary>
        /// The number of media objects.
        /// </summary>
        public ulong MediaObjects { get; }
        
        /// <summary>
        /// The number of attachments.
        /// </summary>
        public ulong Attachments { get; }

        /// <summary>
        /// The number of assignments.
        /// </summary>
        public ulong Assignments { get; }

        internal DepartmentStatistics(DepartmentStatisticsModel model) {
            Subaccounts = model.Subaccounts;
            Teachers = model.Teachers;
            Students = model.Students;
            DiscussionTopics = model.DiscussionTopics;
            MediaObjects = model.MediaObjects;
            Attachments = model.Attachments;
            Assignments = model.Assignments;
        }

        /// <inheritdoc />
        public string ToPrettyString() {
            return "DepartmentStatistics { " +
                   ($"\n{nameof(Subaccounts)}: {Subaccounts}," +
                   $"\n{nameof(Teachers)}: {Teachers}," +
                   $"\n{nameof(Students)}: {Students}," +
                   $"\n{nameof(DiscussionTopics)}: {DiscussionTopics}," +
                   $"\n{nameof(MediaObjects)}: {MediaObjects}," +
                   $"\n{nameof(Attachments)}: {Attachments}," +
                   $"\n{nameof(Assignments)}: {Assignments}").Indent(4) + 
                   "\n}";
        }
    }
}
