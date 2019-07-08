using System;
using System.Collections.Generic;
using System.Linq;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Gradebook;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Gradebook {
    
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
    // ReSharper disable MemberCanBePrivate.Global
    public class Day : IPrettyPrint {
        private readonly Api _api;
        
        public DateTime Date { get; }

        public IEnumerable<Grader> Graders { get; }

        public Day(Api api, DayModel model) {
            _api = api;
            Date = model.Date;
            Graders = model.Graders.Select(m => new Grader(api, m));
        }

        public string ToPrettyString() {
            return "Day {" + 
                   ($"\n{nameof(Date)}: {Date}," +
                   $"\n{nameof(Graders)}: {Graders.ToPrettyString()}").Indent(4) + 
                   "\n}";
        }
    }
}