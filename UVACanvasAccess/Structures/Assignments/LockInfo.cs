using System;
using JetBrains.Annotations;
using UVACanvasAccess.Model.Assignments;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Assignments {
    
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
    // ReSharper disable MemberCanBePrivate.Global
    public class LockInfo : IPrettyPrint {
        private readonly Api _api;
        
        public string AssetString { get; }
        
        public DateTime? UnlockAt { get; }
        
        public DateTime? LockAt { get; }
        
        [CanBeNull]
        public string ContextModule { get; }
        
        public bool? ManuallyLocked { get; }

        public LockInfo(Api api, LockInfoModel model) {
            _api = api;
            AssetString = model.AssetString;
            UnlockAt = model.UnlockAt;
            LockAt = model.LockAt;
            ContextModule = model.ContextModule;
            ManuallyLocked = model.ManuallyLocked;
        }

        public string ToPrettyString() {
            return "LockInfo {" + 
                   ($"\n{nameof(AssetString)}: {AssetString}," +
                   $"\n{nameof(UnlockAt)}: {UnlockAt}," +
                   $"\n{nameof(LockAt)}: {LockAt}," +
                   $"\n{nameof(ContextModule)}: {ContextModule}," +
                   $"\n{nameof(ManuallyLocked)}: {ManuallyLocked}").Indent(4) + 
                   "\n}";
        }
    }
}