using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Quizzes;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Quizzes {
    
    [PublicAPI]
    public class QuizPermissions : IPrettyPrint {
        private readonly Api _api;
        
        public bool Read { get; }
        
        public bool Submit { get; }
        
        public bool Create { get; }
        
        public bool Manage { get; }

        public bool ReadStatistics { get; }
        
        public bool ReviewGrades { get; }
        
        public bool Update { get; }

        internal QuizPermissions(Api api, QuizPermissionsModel model) {
            _api = api;
            Read = model.Read;
            Submit = model.Submit;
            Create = model.Create;
            Manage = model.Manage;
            ReadStatistics = model.ReadStatistics;
            ReviewGrades = model.ReviewGrades;
            Update = model.Update;
        }

        public string ToPrettyString() {
            return "QuizPermissions { " + 
                   ($"\n{nameof(Read)}: {Read}," +
                   $"\n{nameof(Submit)}: {Submit}," +
                   $"\n{nameof(Create)}: {Create}," +
                   $"\n{nameof(Manage)}: {Manage}," +
                   $"\n{nameof(ReadStatistics)}: {ReadStatistics}," +
                   $"\n{nameof(ReviewGrades)}: {ReviewGrades}," +
                   $"\n{nameof(Update)}: {Update}").Indent(4) + 
                   "\n}";
        }
    }
}
