using System;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Gradebook;
using UVACanvasAccess.Structures.Submissions;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Gradebook {
    
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
    // ReSharper disable MemberCanBePrivate.Global
    public class SubmissionVersion : Submission, IPrettyPrint {
        
        public string AssignmentName { get; }

        public string CurrentGrade { get; }

        public DateTime? CurrentGradedAt { get; }

        public string CurrentGrader { get; }

        public string NewGrade { get; }

        public DateTime? NewGradedAt { get; }

        public string NewGrader { get; }

        public string PreviousGrade { get; }

        public DateTime? PreviousGradedAt { get; }

        public string PreviousGrader { get; }

        public SubmissionVersion(Api api, SubmissionVersionModel model) : base(api, model) {
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

        public new string ToPrettyString() {
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