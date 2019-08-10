using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Groups;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Groups {
    
    [PublicAPI]
    public class GroupMembership : IPrettyPrint {
        private readonly Api _api;

        public ulong Id { get; }
        
        public ulong GroupId { get; }
        
        public ulong UserId { get; }

        public string WorkflowState { get; }

        public bool Moderator { get; }
        
        public bool? JustCreated { get; }
        
        public ulong? SisImportId { get; }

        internal GroupMembership(Api api, GroupMembershipModel model) {
            _api = api;
            Id = model.Id;
            GroupId = model.GroupId;
            UserId = model.UserId;
            WorkflowState = model.WorkflowState;
            Moderator = model.Moderator;
            JustCreated = model.JustCreated;
            SisImportId = model.SisImportId;
        }

        public string ToPrettyString() {
            return "GroupMembership {" + 
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(GroupId)}: {GroupId}," +
                   $"\n{nameof(UserId)}: {UserId}," +
                   $"\n{nameof(WorkflowState)}: {WorkflowState}," +
                   $"\n{nameof(Moderator)}: {Moderator}," +
                   $"\n{nameof(JustCreated)}: {JustCreated}," +
                   $"\n{nameof(SisImportId)}: {SisImportId}").Indent(4) + 
                   "\n}";
        }
    }
}
