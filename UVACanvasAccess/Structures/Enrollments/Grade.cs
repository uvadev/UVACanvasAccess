using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Enrollments;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Enrollments {
    
    /// <summary>
    /// Represents a student's grade in a <see cref="Enrollment">course enrollment</see>.
    /// </summary>
    [PublicAPI]
    public class Grade : IPrettyPrint {
        private readonly Api api;
        
        /// <summary>
        /// The corresponding URL on the web interface.
        /// </summary>
        [CanBeNull]
        public string HtmlUrl { get; }
        
        /// <summary>
        /// The current grade.
        /// </summary>
        [CanBeNull]
        public string CurrentGrade { get; }

        /// <summary>
        /// The final grade.
        /// </summary>
        [CanBeNull]
        public string FinalGrade { get; }

        /// <summary>
        /// The current score.
        /// </summary>
        [CanBeNull]
        public string CurrentScore { get; }

        /// <summary>
        /// The final score.
        /// </summary>
        [CanBeNull]
        public string FinalScore { get; }

        /// <summary>
        /// The unposted current grade.
        /// </summary>
        [CanBeNull]
        public string UnpostedCurrentGrade { get; }
        
        /// <summary>
        /// The unposted final grade.
        /// </summary>
        [CanBeNull]
        public string UnpostedFinalGrade { get; }

        /// <summary>
        /// The unposted final score.
        /// </summary>
        [CanBeNull]
        public string UnpostedCurrentScore { get; }

        /// <summary>
        /// The unposted final score.
        /// </summary>
        [CanBeNull]
        public string UnpostedFinalScore { get; }

        internal Grade(Api api, GradeModel model) {
            this.api = api;
            HtmlUrl = model.HtmlUrl;
            CurrentGrade = model.CurrentGrade;
            FinalGrade = model.FinalGrade;
            CurrentScore = model.CurrentScore;
            FinalScore = model.FinalScore;
            UnpostedCurrentGrade = model.UnpostedCurrentGrade;
            UnpostedFinalGrade = model.UnpostedFinalGrade;
            UnpostedCurrentScore = model.UnpostedCurrentScore;
            UnpostedFinalScore = model.UnpostedFinalScore;
        }
        
        /// <inheritdoc />
        public string ToPrettyString() {
            return "Grade {" + 
                   ($"\n{nameof(HtmlUrl)}: {HtmlUrl}," +
                   $"\n{nameof(CurrentGrade)}: {CurrentGrade}," +
                   $"\n{nameof(FinalGrade)}: {FinalGrade}," +
                   $"\n{nameof(CurrentScore)}: {CurrentScore}," +
                   $"\n{nameof(FinalScore)}: {FinalScore}," +
                   $"\n{nameof(UnpostedCurrentGrade)}: {UnpostedCurrentGrade}," +
                   $"\n{nameof(UnpostedFinalGrade)}: {UnpostedFinalGrade}," +
                   $"\n{nameof(UnpostedCurrentScore)}: {UnpostedCurrentScore}," +
                   $"\n{nameof(UnpostedFinalScore)}: {UnpostedFinalScore}").Indent(4) +
                   "\n}";
        }
    }
}