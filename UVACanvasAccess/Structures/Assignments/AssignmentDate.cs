using System;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Assignments;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Assignments {

    [PublicAPI]
    public class AssignmentDate : IPrettyPrint {
        private readonly Api _api;
        
        public ulong? Id { get; }

        public bool? Base { get; }
        
        public string Title { get; }
        
        public DateTime DueAt { get; }
        
        public DateTime? UnlockAt { get; }

        public DateTime? LockAt { get; }

        internal AssignmentDate(Api api, AssignmentDateModel model) {
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