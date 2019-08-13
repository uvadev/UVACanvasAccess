using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Results;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Results {
    
    [PublicAPI]
    public class Result : IPrettyPrint {
        private readonly Api _api;
        
        public string Id { get; }
        
        public string UserId { get; }
        
        public string ResultScore { get; }
        
        public string ResultMaximum { get; }
        
        public string Comment { get; }
        
        public string ScoreOf { get; }

        internal Result(Api api, ResultModel model) {
            _api = api;
            Id = model.Id;
            UserId = model.UserId;
            ResultScore = model.ResultScore;
            ResultMaximum = model.ResultMaximum;
            Comment = model.Comment;
            ScoreOf = model.ScoreOf;
        }

        public string ToPrettyString() {
            return "Result {" + 
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(UserId)}: {UserId}," +
                   $"\n{nameof(ResultScore)}: {ResultScore}," +
                   $"\n{nameof(ResultMaximum)}: {ResultMaximum}," +
                   $"\n{nameof(Comment)}: {Comment}," +
                   $"\n{nameof(ScoreOf)}: {ScoreOf}").Indent(4) + 
                   "\n}";
        }
    }
}
