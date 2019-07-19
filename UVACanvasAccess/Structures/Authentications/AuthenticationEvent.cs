using System;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Exceptions;
using UVACanvasAccess.Model.Authentications;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Authentications {
    
    /// <summary>
    /// An authentication (login/logout) event.
    /// </summary>
    [PublicAPI]
    public class AuthenticationEvent : IPrettyPrint {
        private readonly Api _api;
        
        public string Id { get; }
        
        public DateTime CreatedAt { get; }
        
        public EventType Event { get; }
        
        public ulong LoginId { get; }
        
        public ulong AccountId { get; }
        
        public ulong UserId { get; }
        
        public ulong? PageViewId { get; }

        internal AuthenticationEvent(Api api, AuthenticationEventModel model) {
            _api = api;
            Id = model.Id;
            CreatedAt = model.CreatedAt;
            Event = model.EventType.ToApiRepresentedEnum<EventType>() ?? throw new CommunicationException();
            LoginId = model.Links.Login;
            AccountId = model.Links.Account;
            UserId = model.Links.User;
            PageViewId = model.Links.PageView;
        }

        public string ToPrettyString() {
            return "AuthenticationEvent {" +
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(CreatedAt)}: {CreatedAt}," +
                   $"\n{nameof(Event)}: {Event}," +
                   $"\n{nameof(LoginId)}: {LoginId}," +
                   $"\n{nameof(AccountId)}: {AccountId}," +
                   $"\n{nameof(UserId)}: {UserId}," +
                   $"\n{nameof(PageViewId)}: {PageViewId}").Indent(4) + 
                   "\n}";
        }
    }
}