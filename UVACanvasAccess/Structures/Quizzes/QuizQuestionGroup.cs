using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Quizzes;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Quizzes {
    
    /// <summary>
    /// Represents a quiz question group.
    /// </summary>
    [PublicAPI]
    public class QuizQuestionGroup : IPrettyPrint {
        private readonly Api api;
        
        public ulong Id { get; }
        
        public ulong QuizId { get; }
        
        public string Name { get; }
        
        public uint? Position { get; }
        
        public uint? PickCount { get; }
        
        public decimal? QuestionPoints { get; }
        
        public ulong? AssessmentQuestionBankId { get; }
        
        public string QuestionBankName { get; }

        internal QuizQuestionGroup(Api api, QuizQuestionGroupModel model) {
            this.api = api;
            Id = model.Id;
            QuizId = model.QuizId;
            Name = model.Name;
            Position = model.Position;
            PickCount = model.PickCount;
            QuestionPoints = model.QuestionPoints;
            AssessmentQuestionBankId = model.AssessmentQuestionBankId;
            QuestionBankName = model.QuestionBankName;
        }

        public string ToPrettyString() {
            return "QuizQuestionGroup {" +
                   ($"\n{nameof(Id)}: {Id}," +
                    $"\n{nameof(QuizId)}: {QuizId}," +
                    $"\n{nameof(Name)}: {Name}," +
                    $"\n{nameof(Position)}: {Position}," +
                    $"\n{nameof(PickCount)}: {PickCount}," +
                    $"\n{nameof(QuestionPoints)}: {QuestionPoints}," +
                    $"\n{nameof(AssessmentQuestionBankId)}: {AssessmentQuestionBankId}," +
                    $"\n{nameof(QuestionBankName)}: {QuestionBankName}").Indent(4) +
                   "\n}";
        }
    }
}
