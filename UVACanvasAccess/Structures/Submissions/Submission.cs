using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Submissions;
using UVACanvasAccess.Structures.Assignments;
using UVACanvasAccess.Structures.Courses;
using UVACanvasAccess.Structures.Users;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Submissions {
    
    /// <summary>
    /// Represents an assignment submission.
    /// </summary>
    /// <seealso cref="Assignment"/>
    [PublicAPI]
    public class Submission : IPrettyPrint {
        private protected readonly Api Api;
        
        /// <summary>
        /// The assignment id this submission is for.
        /// </summary>
        public ulong AssignmentId { get; }
        
        /// <summary>
        /// The assignment object this submission is for.
        /// </summary>
        [CanBeNull]
        public Assignment Assignment { get; }
        
        /// <summary>
        /// The course the assignment belongs to.
        /// </summary>
        [CanBeNull]
        public Course Course { get; }

        /// <summary>
        /// Which attempt this submission is (e.g. 1 for the first attempt).
        /// </summary>
        /// <seealso cref="ExtraAttempts"/>
        public uint? Attempt { get; }
        
        /// <summary>
        /// The textual submission contents, if they exist.
        /// </summary>
        [CanBeNull]
        public string Body { get; }

        /// <summary>
        /// The submission grade.
        /// </summary>
        public string Grade { get; }

        /// <summary>
        /// Whether this submission is the most recent <see cref="Attempt"> attempt</see>.
        /// </summary>
        public bool? GradeMatchesCurrentSubmission { get; }

        /// <summary>
        /// The url to the web UI.
        /// </summary>
        public string HtmlUrl { get; }

        /// <summary>
        /// The url to the web UI preview.
        /// </summary>
        public string PreviewUrl { get; }

        /// <summary>
        /// The submission score.
        /// </summary>
        public decimal? Score { get; }
        
        /// <summary>
        /// The list of submission comments, if any.
        /// </summary>
        [CanBeNull]
        public IEnumerable<SubmissionComment> SubmissionComments { get; }

        /// <summary>
        /// The submission type.
        /// </summary>
        public string SubmissionType { get; }
        
        /// <summary>
        /// When the submission was created.
        /// </summary>
        public DateTime? SubmittedAt { get; }
        
        /// <summary>
        /// If the submission was a URL submission, this is the submitted URL.
        /// </summary>
        [CanBeNull]
        public string Url { get; }

        /// <summary>
        /// The user id of the submitter.
        /// </summary>
        public ulong UserId { get; }

        /// <summary>
        /// The grader id. Not guaranteed to map to a canvas user id.
        /// </summary>
        [Enigmatic]
        public long? GraderId { get; }
        
        /// <summary>
        /// When the submission was graded.
        /// </summary>
        public DateTime? GradedAt { get; }

        /// <summary>
        /// The user object of the submitter.
        /// </summary>
        public User User { get; }

        /// <summary>
        /// Whether the submission was late.
        /// </summary>
        public bool? Late { get; }

        /// <summary>
        /// Whether the assignment is visible to the submitter.
        /// </summary>
        public bool? AssignmentVisible { get; }

        /// <summary>
        /// Whether the assignment was excused for the submitter.
        /// </summary>
        public bool? Excused { get; }
        
        /// <summary>
        /// Whether the submission is missing.
        /// </summary>
        public bool? Missing { get; }

        /// <summary>
        /// The late status.
        /// </summary>
        /// <seealso cref="SubmissionLateStatus"/>
        public SubmissionLateStatus LatePolicyStatus { get; }

        /// <summary>
        /// How many points were deducted as a result of the <see cref="LatePolicyStatus"/>.
        /// </summary>
        public double? PointsDeducted { get; }
        
        /// <summary>
        /// How many seconds the submission was late by.
        /// </summary>
        public double? SecondsLate { get; }

        /// <summary>
        /// The overall submission state.
        /// </summary>
        public string WorkflowState { get; }
        
        /// <summary>
        /// How many extra attempts are allowed.
        /// </summary>
        /// <seealso cref="Attempt"/>
        public uint? ExtraAttempts { get; }
        
        /// <summary>
        /// A short string identifying this submission without referencing the submitter.
        /// </summary>
        [CanBeNull]
        public string AnonymousId { get; }

        internal Submission(Api api, SubmissionModel model) {
            Api = api;
            AssignmentId = model.AssignmentId;
            Assignment = model.Assignment.ConvertIfNotNull(m => new Assignment(api, m));
            Course = model.Course.ConvertIfNotNull(c => new Course(api, c));
            Attempt = model.Attempt;
            Body = model.Body;
            Grade = model.Grade;
            GradeMatchesCurrentSubmission = model.GradeMatchesCurrentSubmission;
            HtmlUrl = model.HtmlUrl;
            PreviewUrl = model.PreviewUrl;
            Score = model.Score;
            SubmissionComments = model.SubmissionComments.ConvertIfNotNull(ie => ie.Select(m => new SubmissionComment(api, m)));
            SubmissionType = model.SubmissionType;
            SubmittedAt = model.SubmittedAt;
            Url = model.Url;
            UserId = model.UserId;
            GraderId = model.GraderId;
            GradedAt = model.GradedAt;
            User = model.User.ConvertIfNotNull(m => new User(api, m));
            Late = model.Late;
            AssignmentVisible = model.AssignmentVisible;
            Excused = model.Excused;
            Missing = model.Missing;
            LatePolicyStatus = model.LatePolicyStatus.ToApiRepresentedEnum<SubmissionLateStatus>() ?? SubmissionLateStatus.None;
            PointsDeducted = model.PointsDeducted;
            SecondsLate = model.SecondsLate;
            WorkflowState = model.WorkflowState;
            ExtraAttempts = model.ExtraAttempts;
            AnonymousId = model.AnonymousId;
        }

        ///<inheritdoc />
        public virtual string ToPrettyString() {
            return "Submission {" + 
                   ($"\n{nameof(AssignmentId)}: {AssignmentId}," +
                   $"\n{nameof(Assignment)}: {Assignment?.ToPrettyString()}," +
                   $"\n{nameof(Course)}: {Course?.ToPrettyString()}," +
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
                   $"\n{nameof(User)}: {User?.ToPrettyString()}," +
                   $"\n{nameof(Late)}: {Late}," +
                   $"\n{nameof(AssignmentVisible)}: {AssignmentVisible}," +
                   $"\n{nameof(Excused)}: {Excused}," +
                   $"\n{nameof(Missing)}: {Missing}," +
                   $"\n{nameof(LatePolicyStatus)}: {LatePolicyStatus}," +
                   $"\n{nameof(PointsDeducted)}: {PointsDeducted}," +
                   $"\n{nameof(SecondsLate)}: {SecondsLate}," +
                   $"\n{nameof(WorkflowState)}: {WorkflowState}," +
                   $"\n{nameof(ExtraAttempts)}: {ExtraAttempts}," +
                   $"\n{nameof(AnonymousId)}: {AnonymousId}").Indent(4) +
                   "\n}";
        }
    }

    /// <summary>
    /// Ways in which a submission can be late.
    /// </summary>
    [PublicAPI]
    public enum SubmissionLateStatus {
        /// <summary>
        /// The submission exists, but is late.
        /// </summary>
        [ApiRepresentation("late")]
        Late,
        /// <summary>
        /// The submission does not exist.
        /// </summary>
        [ApiRepresentation("missing")]
        Missing,
        /// <summary>
        /// The submission falls under an extended deadline.
        /// </summary>
        [ApiRepresentation("extended")]
        Extended,
        /// <summary>
        /// The submission is not late.
        /// </summary>
        [ApiRepresentation("none")]
        None
    }
}
