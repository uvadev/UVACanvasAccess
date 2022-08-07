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
        private readonly Api api;
        
        /// <summary>
        /// The id.
        /// </summary>
        public string Id { get; }
        
        /// <summary>
        /// When the event occurred.
        /// </summary>
        public DateTime CreatedAt { get; }
        
        /// <summary>
        /// The event type.
        /// </summary>
        public EventType Event { get; }
        
        /// <summary>
        /// The login id.
        /// </summary>
        public ulong LoginId { get; }
        
        /// <summary>
        /// The account id.
        /// </summary>
        public ulong AccountId { get; }
        
        /// <summary>
        /// The id of the user who performed the event.
        /// </summary>
        public ulong UserId { get; }
        
        /// <summary>
        /// If any, the page view id.
        /// </summary>
        public ulong? PageViewId { get; }

        internal AuthenticationEvent(Api api, AuthenticationEventModel model) {
            this.api = api;
            Id = model.Id;
            CreatedAt = model.CreatedAt;
            Event = model.EventType.ToApiRepresentedEnum<EventType>() ?? throw new CommunicationException();
            LoginId = model.Links.Login;
            AccountId = model.Links.Account;
            UserId = model.Links.User;
            PageViewId = model.Links.PageView;
        }
        
        /// <inheritdoc />
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