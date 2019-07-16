using System;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Courses;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Courses {
    
    [PublicAPI]
    public class Term : IPrettyPrint {
        private readonly Api _api;
        
        public ulong Id { get; }
        
        public string Name { get; }
        
        public DateTime? StartAt { get; }
        
        public DateTime? EndAt { get; }

        internal Term(Api api, TermModel model) {
            _api = api;
            Id = model.Id;
            Name = model.Name;
            StartAt = model.StartAt;
            EndAt = model.EndAt;
        }

        public string ToPrettyString() {
            return "Term {" + 
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(Name)}: {Name}," +
                   $"\n{nameof(StartAt)}: {StartAt}," +
                   $"\n{nameof(EndAt)}: {EndAt}").Indent(4) + 
                   "\n}";
        }
    }
}