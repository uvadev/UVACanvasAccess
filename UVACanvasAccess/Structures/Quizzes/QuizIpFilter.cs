using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Quizzes;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Quizzes {
    
    /// <summary>
    /// Represents an IP filter for a quiz.
    /// </summary>
    [PublicAPI]
    public class QuizIpFilter : IPrettyPrint {
        private readonly Api api;
        
        public string Name { get; }
        
        public ulong? AccountId { get; }
        
        public string Filter { get; }

        internal QuizIpFilter(Api api, QuizIpFilterModel model) {
            this.api = api;
            Name = model.Name;
            AccountId = model.AccountId;
            Filter = model.Filter;
        }

        public string ToPrettyString() {
            return "QuizIpFilter {" +
                   ($"\n{nameof(Name)}: {Name}," +
                    $"\n{nameof(AccountId)}: {AccountId}," +
                    $"\n{nameof(Filter)}: {Filter}").Indent(4) +
                   "\n}";
        }
    }
}
