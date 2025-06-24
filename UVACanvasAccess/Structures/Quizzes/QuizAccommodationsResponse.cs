using System.Collections.Generic;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Quizzes;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Quizzes {
    
    [PublicAPI]
    public class QuizAccommodationsResponse : IPrettyPrint {
        private readonly Api api;
        
        public string Message { get; set; }
        public IEnumerable<SuccessfulQuizAccommodationsResponseItem> Successful { get; }
        public IEnumerable<FailedQuizAccommodationsResponseItem> Failed { get; }
        
        internal QuizAccommodationsResponse(Api api, QuizAccommodationsResponseModel model) {
            this.api = api;
            Message = model.Message;
            Successful = model.Successful.SelectNotNull(m => new SuccessfulQuizAccommodationsResponseItem(m));
            Failed = model.Failed.SelectNotNull(m => new FailedQuizAccommodationsResponseItem(m));
        }
        
        public string ToPrettyString() {
            return "QuizAccommodationsResponse {" + 
                   ($"\n{nameof(Message)}: {Message}," +
                   $"\n{nameof(Successful)}: {Successful.ToPrettyString()}," +
                   $"\n{nameof(Failed)}: {Failed.ToPrettyString()}").Indent(4) + 
                   "\n}";
        }
    }
    
    [PublicAPI]
    public class SuccessfulQuizAccommodationsResponseItem : IPrettyPrint {
        public ulong UserId { get; }

        internal SuccessfulQuizAccommodationsResponseItem(SuccessfulQuizAccommodationsResponseItemModel model) {
            UserId = model.UserId;
        }
        
        public string ToPrettyString() {
            return "SuccessfulQuizAccommodationsResponseItem {" + 
                   ($"\n{nameof(UserId)}: {UserId}").Indent(4) + 
                   "\n}";
        }
    }
    
    [PublicAPI]
    public class FailedQuizAccommodationsResponseItem : IPrettyPrint {
        public ulong UserId { get; }
        public string Error { get; }

        internal FailedQuizAccommodationsResponseItem(FailedQuizAccommodationsResponseItemModel model) {
            UserId = model.UserId;
            Error = model.Error;
        }
        
        public string ToPrettyString() {
            return "FailedQuizAccommodationsResponseItem {" + 
                   ($"\n{nameof(UserId)}: {UserId}," +
                   $"\n{nameof(Error)}: {Error}").Indent(4) + 
                   "\n}";
        }
    }
}
