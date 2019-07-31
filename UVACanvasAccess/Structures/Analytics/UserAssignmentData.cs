using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UVACanvasAccess.Model.Analytics;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Analytics {

    [PublicAPI]
    public struct UserAssignmentSubmissionData : IPrettyPrint {
        public DateTime? SubmittedAt { get; }
        public uint? Score { get; }

        internal UserAssignmentSubmissionData(UserAssignmentSubmissionDataModel model) {
            SubmittedAt = model.SubmittedAt;
            Score = model.Score;
        }

        public string ToPrettyString() {
            return "UserAssignmentSubmissionData {" + 
                   ($"\n{nameof(SubmittedAt)}: {SubmittedAt}," +
                   $"\n{nameof(Score)}: {Score}").Indent(4) + 
                   "\n}";
        }
    }

    [PublicAPI]
    public class UserAssignmentData : IPrettyPrint {
        public ulong AssignmentId { get; }
        
        public string Title { get; }
        
        public uint? PointsPossible { get; }
        
        public DateTime? DueAt { get; }
        
        public DateTime? UnlockAt { get; }
        
        public bool? Muted { get; }
        
        public uint? MinScore { get; }
        
        public uint? MaxScore { get; }
        
        public uint? Median { get; }
        
        public uint? FirstQuartile { get; }
        
        public uint? ThirdQuartile { get; }
        
        public IEnumerable<ulong> ModuleIds { get; }
        
        public UserAssignmentSubmissionData? Submission { get; }

        internal UserAssignmentData(UserAssignmentDataModel model) {
            AssignmentId = model.AssignmentId;
            Title = model.Title;
            PointsPossible = model.PointsPossible;
            DueAt = model.DueAt;
            UnlockAt = model.UnlockAt;
            Muted = model.Muted;
            MinScore = model.MinScore;
            MaxScore = model.MaxScore;
            Median = model.Median;
            FirstQuartile = model.FirstQuartile;
            ThirdQuartile = model.ThirdQuartile;
            ModuleIds = model.ModuleIds;
            Submission = model.Submission == null ? (UserAssignmentSubmissionData?) null // i love c#
                                                  : new UserAssignmentSubmissionData(model.Submission.Value);
        }

        public string ToPrettyString() {
            return "UserAssignmentData {" + 
                   ($"\n{nameof(AssignmentId)}: {AssignmentId}," +
                   $"\n{nameof(Title)}: {Title}," +
                   $"\n{nameof(PointsPossible)}: {PointsPossible}," +
                   $"\n{nameof(DueAt)}: {DueAt}," +
                   $"\n{nameof(UnlockAt)}: {UnlockAt}," +
                   $"\n{nameof(Muted)}: {Muted}," +
                   $"\n{nameof(MinScore)}: {MinScore}," +
                   $"\n{nameof(MaxScore)}: {MaxScore}," +
                   $"\n{nameof(Median)}: {Median}," +
                   $"\n{nameof(FirstQuartile)}: {FirstQuartile}," +
                   $"\n{nameof(ThirdQuartile)}: {ThirdQuartile}," +
                   $"\n{nameof(ModuleIds)}: {ModuleIds?.ToPrettyString()}," +
                   $"\n{nameof(Submission)}: {Submission?.ToPrettyString()}").Indent(4) + 
                   "\n}";
        }
    }
}
