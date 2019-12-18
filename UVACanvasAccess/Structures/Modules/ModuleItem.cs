using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Exceptions;
using UVACanvasAccess.Model.Modules;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Modules {

    [PublicAPI]
    public enum ModuleItemType : byte {
        [ApiRepresentation("File")]
        File,
        [ApiRepresentation("Page")]
        Page,
        [ApiRepresentation("Discussion")]
        Discussion,
        [ApiRepresentation("Assignment")]
        Assignment,
        [ApiRepresentation("Quiz")]
        Quiz,
        [ApiRepresentation("SubHeader")]
        Subheader,
        [ApiRepresentation("ExternalUrl")]
        ExternalUrl,
        [ApiRepresentation("ExternalTool")]
        ExternalTool
    }
    
    [PublicAPI]
    public class ModuleItem : IPrettyPrint {
        private readonly Api _api;
        
        public ulong Id { get; set; }
        
        public ulong ModuleId { get; set; }
        
        public uint Position { get; set; }
        
        public string Title { get; set; }
        
        public uint? Indent { get; set; }
        
        public ModuleItemType Type { get; set; }
        
        public ulong? ContentId { get; set; }
        
        public string HtmlUrl { get; set; }
        
        [CanBeNull]
        public string Url { get; set; }
        
        [CanBeNull]
        public string PageUrl { get; set; }
        
        [CanBeNull]
        public string ExternalUrl { get; set; }
        
        public bool NewTab { get; set; }
        
        [OptIn]
        [CanBeNull]
        public CompletionRequirement CompletionRequirement { get; set; }
        
        public bool? Published { get; set; }

        internal ModuleItem(Api api, ModuleItemModel model) {
            _api = api;
            Id = model.Id;
            ModuleId = model.ModuleId;
            Position = model.Position;
            Title = model.Title;
            Indent = model.Indent;
            Type = model.Type.ToApiRepresentedEnum<ModuleItemType>()
                             .Expect(() => new BadApiStateException($"ModuleItem.Type was an unexpected value: {model.Type}"));
            ContentId = model.ContentId;
            HtmlUrl = model.HtmlUrl;
            Url = model.Url;
            PageUrl = model.PageUrl;
            ExternalUrl = model.ExternalUrl;
            NewTab = model.NewTab;
            CompletionRequirement = model.CompletionRequirement.ConvertIfNotNull(m => new CompletionRequirement(api, m));
            Published = model.Published;
        }

        public string ToPrettyString() {
            return "ModuleItem {" + 
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(ModuleId)}: {ModuleId}," +
                   $"\n{nameof(Position)}: {Position}," +
                   $"\n{nameof(Title)}: {Title}," +
                   $"\n{nameof(Indent)}: {Indent}," +
                   $"\n{nameof(Type)}: {Type.GetApiRepresentation()}," +
                   $"\n{nameof(ContentId)}: {ContentId}," +
                   $"\n{nameof(HtmlUrl)}: {HtmlUrl}," +
                   $"\n{nameof(Url)}: {Url}," +
                   $"\n{nameof(PageUrl)}: {PageUrl}," +
                   $"\n{nameof(ExternalUrl)}: {ExternalUrl}," +
                   $"\n{nameof(NewTab)}: {NewTab}," +
                   $"\n{nameof(CompletionRequirement)}: {CompletionRequirement?.ToPrettyString()}," +
                   $"\n{nameof(Published)}: {Published}").Indent(4) + 
                   "\n}";
        }
    }
}
