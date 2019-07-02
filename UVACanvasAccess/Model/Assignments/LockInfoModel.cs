using System;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Assignments {
    
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class LockInfoModel {
        
        [JsonProperty("asset_string")]
        public string AssetString { get; set; }
        
        [JsonProperty("unlock_at")]
        public DateTime? UnlockAt { get; set; }
        
        [JsonProperty("lock_at")]
        public DateTime? LockAt { get; set; }
        
        [CanBeNull]
        [JsonProperty("context_module")]
        public string ContextModule { get; set; }
        
        [JsonProperty("manually_locked")]
        public bool? ManuallyLocked { get; set; }
    }
}