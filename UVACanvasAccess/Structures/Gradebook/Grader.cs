using System.Collections.Generic;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Gradebook;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Gradebook {
    
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
    // ReSharper disable MemberCanBePrivate.Global
    public class Grader : IPrettyPrint {
        private readonly Api _api;
        
        public ulong Id { get; }
        
        public string Name { get; }
        
        public IEnumerable<ulong> Assignments { get; }

        public Grader(Api api, GraderModel model) {
            _api = api;
            Id = model.Id;
            Name = model.Name;
            Assignments = model.Assignments;
        }

        public string ToPrettyString() {
            return "Grader {" + 
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(Name)}: {Name}," +
                   $"\n{nameof(Assignments)}: {Assignments}").Indent(4) + 
                   "\n}";
        }
    }
}