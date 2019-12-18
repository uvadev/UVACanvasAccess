using System;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Modules;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Modules {
    
    [PublicAPI]
    public class ContentDetails : IPrettyPrint {

        private readonly Api _api;
    
        public uint? PointsPossible { get; }
        
        public DateTime? DueAt { get; }
        
        public DateTime? UnlockAt { get; }
        
        public DateTime? LockAt { get; }
        
        public bool LockedForUser { get; }
        
        [CanBeNull]
        public string LockExplanation { get; }

        internal ContentDetails(Api api, ContentDetailsModel model) {
            _api = api;
            PointsPossible = model.PointsPossible;
            DueAt = model.DueAt;
            UnlockAt = model.UnlockAt;
            LockAt = model.LockAt;
            LockedForUser = model.LockedForUser ?? false;
            LockExplanation = model.LockExplanation;
        }

        public string ToPrettyString() {
            return "ContentDetails {" + 
                   ($"\n{nameof(PointsPossible)}: {PointsPossible}," +
                   $"\n{nameof(DueAt)}: {DueAt}," +
                   $"\n{nameof(UnlockAt)}: {UnlockAt}," +
                   $"\n{nameof(LockAt)}: {LockAt}," +
                   $"\n{nameof(LockedForUser)}: {LockedForUser}," +
                   $"\n{nameof(LockExplanation)}: {LockExplanation}").Indent(4) +
                   "\n}";
        }
    }
}
