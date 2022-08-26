using System;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Gradebook;
using UVACanvasAccess.Structures.Submissions;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Gradebook {
    
    /// <summary>
    /// Represents one version of a <see cref="Submission"/> as part of an overall version history.
    /// </summary>
    [PublicAPI]
    public class SubmissionVersion : Submission {
        
        /// <summary>
        /// The assignment name.
        /// </summary>
        public string AssignmentName { get; }

        /// <summary>
        /// The current grade as of the most recent version.
        /// </summary>
        public string CurrentGrade { get; }

        /// <summary>
        /// When the most recent version of the grade was assigned.
        /// </summary>
        public DateTime? CurrentGradedAt { get; }

        /// <summary>
        /// The identity of the grader as of the most recent version.
        /// </summary>
        public string CurrentGrader { get; }

        /// <summary>
        /// The current grade as of this version.
        /// </summary>
        public string NewGrade { get; }

        /// <summary>
        /// When this version of the grade was assigned.
        /// </summary>
        public DateTime? NewGradedAt { get; }

        /// <summary>
        /// The identity of the grader as of this version.
        /// </summary>
        public string NewGrader { get; }

        /// <summary>
        /// The current grade as of the version preceding this one.
        /// </summary>
        public string PreviousGrade { get; }

        /// <summary>
        /// When the preceding version of the grade was assigned.
        /// </summary>
        public DateTime? PreviousGradedAt { get; }

        /// <summary>
        /// The identity of the grader as of the version preceding this one.
        /// </summary>
        public string PreviousGrader { get; }

        internal SubmissionVersion(Api api, SubmissionVersionModel model) : base(api, model) {
            AssignmentName = model.AssignmentName;
            CurrentGrade = model.CurrentGrade;
            CurrentGradedAt = model.CurrentGradedAt;
            CurrentGrader = model.CurrentGrader;
            NewGrade = model.NewGrade;
            NewGradedAt = model.NewGradedAt;
            NewGrader = model.NewGrader;
            PreviousGrade = model.PreviousGrade;
            PreviousGradedAt = model.PreviousGradedAt;
            PreviousGrader = model.PreviousGrader;
        }

        ///<inheritdoc/ >
        public override string ToPrettyString() {
            return "SubmissionVersion {" + 
                   ($"\n{nameof(AssignmentId)}: {AssignmentId}," +
                   $"\n{nameof(Assignment)}: {Assignment}," +
                   $"\n{nameof(Course)}: {Course}," +
                   $"\n{nameof(Attempt)}: {Attempt}," +
                   $"\n{nameof(Body)}: {Body}," +
                   $"\n{nameof(Grade)}: {Grade}," +
                   $"\n{nameof(GradeMatchesCurrentSubmission)}: {GradeMatchesCurrentSubmission}," +
                   $"\n{nameof(HtmlUrl)}: {HtmlUrl}," +
                   $"\n{nameof(PreviewUrl)}: {PreviewUrl}," +
                   $"\n{nameof(Score)}: {Score}," +
                   $"\n{nameof(SubmissionComments)}: {SubmissionComments?.ToPrettyString()}," +
                   $"\n{nameof(SubmissionType)}: {SubmissionType}," +
                   $"\n{nameof(SubmittedAt)}: {SubmittedAt}," +
                   $"\n{nameof(Url)}: {Url}," +
                   $"\n{nameof(UserId)}: {UserId}," +
                   $"\n{nameof(GraderId)}: {GraderId}," +
                   $"\n{nameof(GradedAt)}: {GradedAt}," +
                   $"\n{nameof(User)}: {User}," +
                   $"\n{nameof(Late)}: {Late}," +
                   $"\n{nameof(AssignmentVisible)}: {AssignmentVisible}," +
                   $"\n{nameof(Excused)}: {Excused}," +
                   $"\n{nameof(Missing)}: {Missing}," +
                   $"\n{nameof(LatePolicyStatus)}: {LatePolicyStatus}," +
                   $"\n{nameof(PointsDeducted)}: {PointsDeducted}," +
                   $"\n{nameof(SecondsLate)}: {SecondsLate}," +
                   $"\n{nameof(WorkflowState)}: {WorkflowState}," +
                   $"\n{nameof(ExtraAttempts)}: {ExtraAttempts}," +
                   $"\n{nameof(AnonymousId)}: {AnonymousId}," + 
                   $"\n{nameof(AssignmentName)}: {AssignmentName}," +
                   $"\n{nameof(CurrentGrade)}: {CurrentGrade}," +
                   $"\n{nameof(CurrentGradedAt)}: {CurrentGradedAt}," +
                   $"\n{nameof(CurrentGrader)}: {CurrentGrader}," +
                   $"\n{nameof(NewGrade)}: {NewGrade}," +
                   $"\n{nameof(NewGradedAt)}: {NewGradedAt}," +
                   $"\n{nameof(NewGrader)}: {NewGrader}," +
                   $"\n{nameof(PreviousGrade)}: {PreviousGrade}," +
                   $"\n{nameof(PreviousGradedAt)}: {PreviousGradedAt}," +
                   $"\n{nameof(PreviousGrader)}: {PreviousGrader}").Indent(4) +
                   "\n}";
        }
    }
}