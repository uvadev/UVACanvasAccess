using System;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Gradebook;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Gradebook {
    
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
    // ReSharper disable MemberCanBePrivate.Global
    public class SubmissionVersion : IPrettyPrint {
        private readonly Api _api;
        
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

        public SubmissionVersion(Api api, SubmissionVersionModel model) {
            _api = api;
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

        public string ToPrettyString() {
            return "SubmissionVersion {" + 
                   ($"\n{nameof(AssignmentName)}: {AssignmentName}," +
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