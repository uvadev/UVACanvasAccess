using System;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.GradingPeriods;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.GradingPeriods {
    
    [PublicAPI]
    public class GradingPeriod : IPrettyPrint {
        private readonly Api _api;
        
        public ulong Id { get; }
        
        public string Title { get; }
        
        public DateTime? StartDate { get; }

        public DateTime? EndDate { get; }
        
        public DateTime? CloseDate { get; }
        
        public double? Weight { get; }
        
        public bool? IsClosed { get; }
        
        internal GradingPeriod(Api api, GradingPeriodModel model) {
            _api = api;
            Id = model.Id;
            Title = model.Title;
            StartDate = model.StartDate;
            EndDate = model.EndDate;
            CloseDate = model.CloseDate;
            Weight = model.Weight;
            IsClosed = model.IsClosed;
        }

        public string ToPrettyString() {
            return "GradingPeriod {" +
                   ($"\n{nameof(Id)}: {Id}," +
                    $"\n{nameof(Title)}: {Title}," +
                    $"\n{nameof(StartDate)}: {StartDate}," +
                    $"\n{nameof(EndDate)}: {EndDate}," +
                    $"\n{nameof(CloseDate)}: {CloseDate}," +
                    $"\n{nameof(Weight)}: {Weight}," +
                    $"\n{nameof(IsClosed)}: {IsClosed}").Indent(4) + 
                   "\n}";
        }
    }
}
