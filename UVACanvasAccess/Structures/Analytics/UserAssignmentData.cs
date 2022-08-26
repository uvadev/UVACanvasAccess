using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UVACanvasAccess.Model.Analytics;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Analytics {
    
    /// <summary>
    /// Assignment statistics for a single user and single assignment.
    /// </summary>
    [PublicAPI]
    public class UserAssignmentData : IPrettyPrint {
        
        /// <summary>
        /// The assignment id.
        /// </summary>
        public ulong AssignmentId { get; }
        
        /// <summary>
        /// The assignment title.
        /// </summary>
        public string Title { get; }
        
        /// <summary>
        /// The maximum points possible for the assignment.
        /// </summary>
        public uint? PointsPossible { get; }
        
        /// <summary>
        /// When the assignment is due.
        /// </summary>
        public DateTime? DueAt { get; }
        
        /// <summary>
        /// When the assignment unlocks.
        /// </summary>
        public DateTime? UnlockAt { get; }
        
        /// <summary>
        /// Whether the assignment is muted.
        /// </summary>
        public bool? Muted { get; }
        
        /// <summary>
        /// The minimum score ever earned by any student for this assignment.
        /// </summary>
        public uint? MinScore { get; }
        
        /// <summary>
        /// The maximum score ever earned by any student for this assignment.
        /// </summary>
        public uint? MaxScore { get; }
        
        /// <summary>
        /// The median score.
        /// </summary>
        public uint? Median { get; }
        
        /// <summary>
        /// The Q1 score.
        /// </summary>
        public uint? FirstQuartile { get; }
        
        /// <summary>
        /// The Q3 score.
        /// </summary>
        public uint? ThirdQuartile { get; }
        
        /// <summary>
        /// Module ids.
        /// </summary>
        public IEnumerable<ulong> ModuleIds { get; }
        
        /// <summary>
        /// The student's score for this assignment, if it exists.
        /// </summary>
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

        /// <inheritdoc />
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

    /// <summary>
    /// One user score in a <see cref="UserAssignmentData"/>.
    /// </summary>
    [PublicAPI]
    public struct UserAssignmentSubmissionData : IPrettyPrint {
        
        /// <summary>
        /// When the submission was created.
        /// </summary>
        public DateTime? SubmittedAt { get; }
        
        /// <summary>
        /// The score earned, if it exists.
        /// </summary>
        public uint? Score { get; }

        internal UserAssignmentSubmissionData(UserAssignmentSubmissionDataModel model) {
            SubmittedAt = model.SubmittedAt;
            Score = model.Score;
        }

        /// <inheritdoc />
        public string ToPrettyString() {
            return "UserAssignmentSubmissionData {" + 
                   ($"\n{nameof(SubmittedAt)}: {SubmittedAt}," +
                    $"\n{nameof(Score)}: {Score}").Indent(4) + 
                   "\n}";
        }
    }
}
