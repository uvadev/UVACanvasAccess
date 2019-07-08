using System.Collections.Generic;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Gradebook;
using UVACanvasAccess.Structures.Assignments;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Gradebook {
    
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
    // ReSharper disable MemberCanBePrivate.Global
    public class Grader : IPrettyPrint {
        private readonly Api _api;
        
        public ulong Id { get; }
        
        public string Name { get; }
        
        public IEnumerable<Assignment> Assignments { get; }

        public Grader(Api api, GraderModel model) {
            _api = api;
            Id = model.Id;
            Name = model.Name;
            Assignments = model.Assignments.SelectNotNull(m => new Assignment(api, m));
        }

        public string ToPrettyString() {
            return "Grader {" + 
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(Name)}: {Name}," +
                   $"\n{nameof(Assignments)}: {Assignments.ToPrettyString()}").Indent(4) + 
                   "\n}";
        }
    }
}