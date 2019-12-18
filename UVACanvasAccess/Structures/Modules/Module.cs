using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Exceptions;
using UVACanvasAccess.Model.Modules;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Modules {

    [PublicAPI]
    public enum ModuleState : byte {
        [ApiRepresentation("locked")]
        Locked,
        [ApiRepresentation("unlocked")]
        Unlocked,
        [ApiRepresentation("started")]
        Started,
        [ApiRepresentation("completed")]
        Completed
    }
    
    [PublicAPI]
    public class Module : IPrettyPrint {
        private readonly Api _api;
        
        public ulong Id { get; } 
        
        public string WorkflowState { get; }
        
        public uint Position { get; }
        
        public string Name { get; }
        
        public DateTime? UnlockAt { get; }
        
        public bool? RequireSequentialProgress { get; }
        
        [CanBeNull]
        public IEnumerable<ulong> PrerequisiteModuleIds { get; }
        
        public uint ItemsCount { get; }
        
        public string ItemsUrl { get; }
        
        [OptIn]
        [CanBeNull]
        [Enigmatic] // can be null if "the module is deemed too large", even if opted-in
        public IEnumerable<ModuleItem> Items { get; }
        
        [CanBeNull]
        public ModuleState? State { get; }
        
        [OptIn]
        public DateTime? CompletedAt { get; }
        
        public bool? PublishFinalGrade { get; }
        
        public bool? Published { get; }

        internal Module(Api api, ModuleModel model) {
            _api = api;
            Id = model.Id;
            WorkflowState = model.WorkflowState;
            Position = model.Position;
            Name = model.Name;
            UnlockAt = model.UnlockAt;
            RequireSequentialProgress = model.RequireSequentialProgress;
            PrerequisiteModuleIds = model.PrerequisiteModuleIds;
            ItemsCount = model.ItemsCount;
            ItemsUrl = model.ItemsUrl;
            Items = model.Items?.SelectNotNull(m => new ModuleItem(api, m));
            State = model.State?.ToApiRepresentedEnum<ModuleState>()
                                .Expect(() => new BadApiStateException($"Module.State was an unexpected value: {model.State}"));
            CompletedAt = model.CompletedAt;
            PublishFinalGrade = model.PublishFinalGrade;
            Published = model.Published;
        }

        public string ToPrettyString() {
            return "Module {" + 
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(WorkflowState)}: {WorkflowState}," +
                   $"\n{nameof(Position)}: {Position}," +
                   $"\n{nameof(Name)}: {Name}," +
                   $"\n{nameof(UnlockAt)}: {UnlockAt}," +
                   $"\n{nameof(RequireSequentialProgress)}: {RequireSequentialProgress}," +
                   $"\n{nameof(PrerequisiteModuleIds)}: {PrerequisiteModuleIds?.ToPrettyString()}," +
                   $"\n{nameof(ItemsCount)}: {ItemsCount}," +
                   $"\n{nameof(ItemsUrl)}: {ItemsUrl}," +
                   $"\n{nameof(Items)}: {Items?.ToPrettyString()}," +
                   $"\n{nameof(State)}: {State?.GetApiRepresentation()}," +
                   $"\n{nameof(CompletedAt)}: {CompletedAt}," +
                   $"\n{nameof(PublishFinalGrade)}: {PublishFinalGrade}," +
                   $"\n{nameof(Published)}: {Published}").Indent(4) + 
                   "\n}";
        }
    }
}
