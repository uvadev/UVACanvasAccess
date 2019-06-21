using StatePrinting;
using UVACanvasAccess.Model.Files;

namespace UVACanvasAccess.Structures.Files {
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
    // ReSharper disable MemberCanBePrivate.Global
    public class CanvasFile {
        private readonly Api _api;
        
        public ulong Id { get; }
        
        public string Uuid { get; }
        
        public ulong FolderId { get; }
        
        public string DisplayName { get; }
        
        public string Filename { get; }
        
        public string ContentType { get; }
        
        public string Url { get; }
        
        public ulong Size { get; }
        
        public string CreatedAt { get; }
        
        public string UpdatedAt { get; }
        
        public string UnlockAt { get; }
        
        public bool Locked { get; }
        
        public bool Hidden { get; }
        
        public string LockAt { get; }
        
        public bool HiddenForUser { get; }
        
        public string ThumbnailUrl { get; }
        
        public string ModifiedAt { get; }
        
        public string MimeClass { get; }
        
        public string MediaEntryId { get; }
        
        public bool LockedForUser { get; }
        
        public object LockInfo { get; }
        
        public string LockExplanation { get; }
        
        public string PreviewUrl { get; }

        public CanvasFile(Api api, CanvasFileModel model) {
            _api = api;
            Id = model.Id;
            Uuid = model.Uuid;
            FolderId = model.FolderId;
            DisplayName = model.DisplayName;
            Filename = model.Filename;
            ContentType = model.ContentType;
            Url = model.Url;
            Size = model.Size;
            CreatedAt = model.CreatedAt;
            UpdatedAt = model.UpdatedAt;
            UnlockAt = model.UnlockAt;
            Locked = model.Locked;
            Hidden = model.Hidden;
            LockAt = model.LockAt;
            HiddenForUser = model.HiddenForUser;
            ThumbnailUrl = model.ThumbnailUrl;
            ModifiedAt = model.ModifiedAt;
            MimeClass = model.MimeClass;
            MediaEntryId = model.MediaEntryId;
            LockedForUser = model.LockedForUser;
            LockInfo = model.LockInfo;
            LockExplanation = model.LockExplanation;
            PreviewUrl = model.PreviewUrl;
        }
        
        private static readonly Stateprinter Printer = new Stateprinter();
        public override string ToString() {
            return Printer.PrintObject(this);
        }
    }
}