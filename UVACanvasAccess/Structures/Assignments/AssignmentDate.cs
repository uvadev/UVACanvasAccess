using System;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Assignments;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Assignments {

    /// <summary>
    /// Represents the set of dates applicable to an <see cref="Assignment">assignment</see>.
    /// </summary>
    [PublicAPI]
    public class AssignmentDate : IPrettyPrint {
        private readonly Api _api;
        
        /// <summary>
        /// If these dates comes from an assignment override, the override id.
        /// </summary>
        public ulong? Id { get; }

        /// <summary>
        /// Whether or not these dates do not come from an assignment override.
        /// </summary>
        public bool? Base { get; }
        
        /// <summary>
        /// The title.
        /// </summary>
        public string Title { get; }
        
        /// <summary>
        /// When the assignment is due.
        /// </summary>
        public DateTime DueAt { get; }
        
        /// <summary>
        /// When the assignment unlocks itself.
        /// </summary>
        public DateTime? UnlockAt { get; }

        /// <summary>
        /// When the assignment locks itself.
        /// </summary>
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