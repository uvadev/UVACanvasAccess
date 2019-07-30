using JetBrains.Annotations;
using UVACanvasAccess.Model.Analytics;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Analytics {
    
    [PublicAPI]
    public class DepartmentStatistics : IPrettyPrint {
        
        public ulong Subaccounts { get; }

        public ulong Teachers { get; }

        public ulong Students { get; }

        public ulong DiscussionTopics { get; }
        
        public ulong MediaObjects { get; }
        
        public ulong Attachments { get; }

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
