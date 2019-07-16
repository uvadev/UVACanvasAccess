using System;
using StatePrinting;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Users;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Users {
    
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
    // ReSharper disable MemberCanBePrivate.Global
    public class PageView : IPrettyPrint {
        private readonly Api _api;

        public string Id { get; private set; }
        
        public string AppName { get; private set; }
        
        public string Url { get; private set; }
        
        public string ContextType { get; private set; }
        
        public string AssetType { get; private set; }
        
        public string Controller { get; private set; }
        
        public string Action { get; private set; }
        
        public bool? Contributed { get; private set; }
        
        public ulong? InteractionSeconds { get; private set; }
        
        public DateTime CreatedAt { get; private set; }
        
        public bool? UserRequest { get; private set; }
        
        public double? RenderTime { get; private set; }
        
        public string UserAgent { get; private set; }
        
        public bool? Participated { get; private set; }
        
        public string HttpMethod { get; private set; }
        
        public string RemoteIp { get; private set; }
        
        public PageViewLinks Links { get; private set; }

        public PageView(Api api, PageViewModel model) {
            _api = api;
            Id = model.Id;
            AppName = model.AppName;
            Url = model.Url;
            ContextType = model.ContextType;
            AssetType = model.AssetType;
            Controller = model.Controller;
            Action = model.Action;
            Contributed = model.Contributed;
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
                   $"\n{nameof(Contributed)}: {Contributed}," +
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