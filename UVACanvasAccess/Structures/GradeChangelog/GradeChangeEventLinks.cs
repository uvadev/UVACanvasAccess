using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.GradeChangelog;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.GradeChangelog {
    
    [PublicAPI]
    public class GradeChangeEventLinks : IPrettyPrint {
        private readonly Api _api;
        
        public ulong Assignment { get; }
        
        public ulong Course { get; }
        
        public ulong Student { get; }
        
        public ulong Grader { get; }
        
        [CanBeNull] 
        public string PageView { get; }

        internal GradeChangeEventLinks(Api api, GradeChangeEventLinksModel model) {
            _api = api;
            Assignment = model.Assignment;
            Course = model.Course;
            Student = model.Student;
            Grader = model.Grader;
            PageView = model.PageView;
        }

        public string ToPrettyString() {
            return "GradeChangeEventLinks {" +
                   ($"\n{nameof(Assignment)}: {Assignment}," +
                   $"\n{nameof(Course)}: {Course}," +
                   $"\n{nameof(Student)}: {Student}," +
                   $"\n{nameof(Grader)}: {Grader}," +
                   $"\n{nameof(PageView)}: {PageView}").Indent(4) + 
                   "\n}";
        }
    }
}