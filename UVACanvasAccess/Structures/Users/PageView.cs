using System;
using JetBrains.Annotations;
using StatePrinting;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Users;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Users {
    
    /// <summary>
    /// Represents a record of a user page view request.
    /// </summary>
    [PublicAPI]
    public class PageView : IPrettyPrint {
        private readonly Api _api;

        /// <summary>
        /// The view's UUID.
        /// </summary>
        public string Id { get; }
        
        /// <summary>
        /// If this was an API request, the name of the app that generated the access token.
        /// </summary>
        [CanBeNull]
        public string AppName { get; }
        
        /// <summary>
        /// The requested URL.
        /// </summary>
        public string Url { get; }
        
        /// <summary>
        /// The context for this request. I.e., what part of the API this request is being made on.
        /// </summary>
        public string ContextType { get; }
        
        /// <summary>
        /// The asset within the <see cref="ContextType"/> this request is being made on, if relevant.
        /// </summary>
        [CanBeNull]
        public string AssetType { get; }
        
        /// <summary>
        /// The Rails controller that handled this request.
        /// </summary>
        public string Controller { get; }
        
        /// <summary>
        /// The Rails action that handled this request.
        /// </summary>
        public string Action { get; }

        /// <summary>
        /// An approximation of how long the user spent on the page, in seconds.
        /// </summary>
        public decimal? InteractionSeconds { get; }
        
        /// <summary>
        /// When this request was made.
        /// </summary>
        public DateTime CreatedAt { get; }
        
        /// <summary>
        /// Whether or not this request was initiated by a real user.
        /// </summary>
        public bool? UserRequest { get; }
        
        /// <summary>
        /// An approximation of how long the page took to render, in seconds.
        /// </summary>
        public double? RenderTime { get; }
        
        /// <summary>
        /// The user agent of the browser or program that made this request.
        /// </summary>
        public string UserAgent { get; }
        
        /// <summary>
        /// Whether or not this interaction counted as participation. 
        /// </summary>
        public bool? Participated { get; }
        
        /// <summary>
        /// The HTTP method of this request.
        /// </summary>
        public string HttpMethod { get; }
        
        /// <summary>
        /// The origin IP address of this request.
        /// </summary>
        public string RemoteIp { get; }
        
        /// <summary>
        /// Relationships to this view.
        /// </summary>
        public PageViewLinks Links { get; }

        internal PageView(Api api, PageViewModel model) {
            _api = api;
            Id = model.Id;
            AppName = model.AppName;
            Url = model.Url;
            ContextType = model.ContextType;
            AssetType = model.AssetType;
            Controller = model.Controller;
            Action = model.Action;
            InteractionSeconds = model.InteractionSeconds;
            CreatedAt = model.CreatedAt;
            UserRequest = model.UserRequest;
            RenderTime = model.RenderTime;
            UserAgent = model.UserAgent;
            Participated = model.Participated;
            HttpMethod = model.HttpMethod;
            RemoteIp = model.RemoteIp;
            Links = new PageViewLinks(api, model.Links);
        }
        
        private static readonly Stateprinter Printer = new Stateprinter();
        public override string ToString() {
            return Printer.PrintObject(this);
        }

        public string ToPrettyString() {
            return "PageView {" +
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(AppName)}: {AppName}," +
                   $"\n{nameof(Url)}: {Url}," +
                   $"\n{nameof(ContextType)}: {ContextType}," +
                   $"\n{nameof(AssetType)}: {AssetType}," +
                   $"\n{nameof(Controller)}: {Controller}," +
                   $"\n{nameof(Action)}: {Action}," +
                   $"\n{nameof(InteractionSeconds)}: {InteractionSeconds}," +
                   $"\n{nameof(CreatedAt)}: {CreatedAt}," +
                   $"\n{nameof(UserRequest)}: {UserRequest}," +
                   $"\n{nameof(RenderTime)}: {RenderTime}," +
                   $"\n{nameof(UserAgent)}: {UserAgent}," +
                   $"\n{nameof(Participated)}: {Participated}," +
                   $"\n{nameof(HttpMethod)}: {HttpMethod}," +
                   $"\n{nameof(RemoteIp)}: {RemoteIp}," +
                   $"\n{nameof(Links)}: {Links.ToPrettyString()}").Indent(4) +
                   "\n}";
        }
    }
}