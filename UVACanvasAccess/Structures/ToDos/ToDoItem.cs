using System.Threading.Tasks;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Exceptions;
using UVACanvasAccess.Model.ToDos;
using UVACanvasAccess.Structures.Assignments;
using UVACanvasAccess.Structures.Quizzes;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.ToDos {

    [PublicAPI]
    public enum ToDoType : byte {
        [ApiRepresentation("grading")]
        Grading,
        [ApiRepresentation("submitting")]
        Submitting
    }

    [PublicAPI]
    public abstract class ToDoItem : IPrettyPrint {
        
        private readonly Api _api;

        public ToDoType Type { get; }
        
        public string IgnoreUrl { get; }
        
        public string PermanentIgnoreUrl { get; }
        
        public QualifiedId ContextId { get; }

        private protected ToDoItem(Api api, ToDoItemModel model) {
            _api = api;
            Type = model.Type.ToApiRepresentedEnum<ToDoType>()
                             .Expect(() => new BadApiStateException($"ToDoItem.Type was an unexpected value: {model.Type}"));
            ContextId = model.ContextType.ToLowerInvariant() switch {
                "course" => new QualifiedId(model.CourseId.Expect(), ContextType.Course),
                "group"  => new QualifiedId(model.GroupId.Expect(), ContextType.Group),
                _        => throw new BadApiStateException($"ToDoItemModel.ContextType was an unexpected value: {model.ContextType}")
            };
            IgnoreUrl = model.IgnoreUrl;
            PermanentIgnoreUrl = model.PermanentIgnoreUrl;
        }

        internal static ToDoItem NewToDoItem(Api api, ToDoItemModel model) {
            if (model.Assignment != null) {
                return new AssignmentToDoItem(api, model);
            } else {
                return new QuizToDoItem(api, model);
            }
        }

        /// <summary>
        /// Hide this item from future requests until it changes.
        /// </summary>
        /// <returns>A Task representing completion of this request.</returns>
        public Task Ignore() {
            return _api.IgnoreToDoItem(this, false);
        }

        /// <summary>
        /// Hide this item from all future requests.
        /// </summary>
        /// <returns>A Task representing completion of this request.</returns>
        public Task IgnorePermanently() {
            return _api.IgnoreToDoItem(this, true);
        }

        public abstract string ToPrettyString();
    }

    [PublicAPI]
    public sealed class AssignmentToDoItem : ToDoItem {

        public Assignment Assignment { get; }

        internal AssignmentToDoItem(Api api, ToDoItemModel model) : base(api, model) {
            Assignment = model.Assignment.ConvertIfNotNull(m => new Assignment(api, m));
        }

        public override string ToPrettyString() {
            return "AssignmentToDoItem {" + 
                   ($"\n{nameof(Type)}: {Type.GetApiRepresentation()}," +
                   $"\n{nameof(IgnoreUrl)}: {IgnoreUrl}," +
                   $"\n{nameof(PermanentIgnoreUrl)}: {PermanentIgnoreUrl}," +
                   $"\n{nameof(ContextId)}: {ContextId.ToPrettyString()}," +
                   $"\n{nameof(Assignment)}: {Assignment.ToPrettyString()}").Indent(4) + 
                   "\n}";
        }
    }
    
    [PublicAPI]
    public sealed class QuizToDoItem : ToDoItem {

        public Quiz Quiz { get; }

        internal QuizToDoItem(Api api, ToDoItemModel model) : base(api, model) {
            Quiz = model.Quiz.ConvertIfNotNull(m => new Quiz(api, m));
        }

        public override string ToPrettyString() {
            return "QuizToDoItem {" + 
                   ($"\n{nameof(Type)}: {Type.GetApiRepresentation()}," +
                    $"\n{nameof(IgnoreUrl)}: {IgnoreUrl}," +
                    $"\n{nameof(PermanentIgnoreUrl)}: {PermanentIgnoreUrl}," +
                    $"\n{nameof(ContextId)}: {ContextId.ToPrettyString()}," +
                    $"\n{nameof(Quiz)}: {Quiz.ToPrettyString()}").Indent(4) + 
                   "\n}";
        }
    }
}
