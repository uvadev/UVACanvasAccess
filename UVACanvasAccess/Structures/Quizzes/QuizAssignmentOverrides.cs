using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Quizzes;
using UVACanvasAccess.Structures.Assignments;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Quizzes {
    
    /// <summary>
    /// Represents the assignment override data for a quiz.
    /// </summary>
    [PublicAPI]
    public class QuizAssignmentOverrides : IPrettyPrint {
        private readonly Api api;
        
        public ulong? QuizId { get; }
        
        [CanBeNull]
        public IEnumerable<AssignmentDate> DueDates { get; }
        
        [CanBeNull]
        public IEnumerable<AssignmentDate> AllDates { get; }

        internal QuizAssignmentOverrides(Api api, QuizAssignmentOverridesModel model) {
            this.api = api;
            QuizId = model.QuizId;
            DueDates = model.DueDates?.Select(d => new AssignmentDate(api, d));
            AllDates = model.AllDates?.Select(d => new AssignmentDate(api, d));
        }

        public string ToPrettyString() {
            return "QuizAssignmentOverrides {" +
                   ($"\n{nameof(QuizId)}: {QuizId}," +
                    $"\n{nameof(DueDates)}: {DueDates?.ToPrettyString()}," +
                    $"\n{nameof(AllDates)}: {AllDates?.ToPrettyString()}").Indent(4) +
                   "\n}";
        }
    }
}
