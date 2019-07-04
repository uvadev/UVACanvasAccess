using System;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Assignments;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Assignments {

    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
    // ReSharper disable MemberCanBePrivate.Global
    public class AssignmentDate : IPrettyPrint {
        private readonly Api _api;
        
        public ulong? Id { get; }

        public bool? Base { get; }
        
        public string Title { get; }
        
        public DateTime DueAt { get; }
        
        public DateTime? UnlockAt { get; }

        public DateTime? LockAt { get; }

        public AssignmentDate(Api api, AssignmentDateModel model) {
            _api = api;
            Id = model.Id;
            Base = model.Base;
            Title = model.Title;
            DueAt = model.DueAt;
            UnlockAt = model.UnlockAt;
            LockAt = model.LockAt;
        }

        public string ToPrettyString() {
            return "AssignmentDate {" +
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(Base)}: {Base}," +
                   $"\n{nameof(Title)}: {Title}," +
                   $"\n{nameof(DueAt)}: {DueAt}," +
                   $"\n{nameof(UnlockAt)}: {UnlockAt}," +
                   $"\n{nameof(LockAt)}: {LockAt}").Indent(4) +
                   "\n}";
        }
    }
}