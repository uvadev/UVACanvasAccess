using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Files;
using UVACanvasAccess.Structures.Assignments;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Files {
    
    /// <summary>
    /// Represents a file present on the Canvas server.
    /// </summary>
    [PublicAPI]
    public class CanvasFile : IPrettyPrint {
        private readonly Api api;
        
        /// <summary>
        /// The file id.
        /// </summary>
        /// <remarks>This is distinct from the <see cref="CanvasFile.Uuid"/></remarks>
        public ulong Id { get; }
        
        /// <summary>
        /// The file's UUID.
        /// </summary>
        /// <remarks>This is distinct from the <see cref="CanvasFile.Id"/></remarks>
        public string Uuid { get; }
        
        /// <summary>
        /// The id of the folder the file is in.
        /// </summary>
        public ulong FolderId { get; private set; }
        
        /// <summary>
        /// The file name, as shown to end users on the web interface.
        /// </summary>
        /// <remarks>This is distinct from <see cref="Filename"/>, the file's true name on the server.</remarks>
        public string DisplayName { get; }
        
        /// <summary>
        /// The file's name.
        /// </summary>
        /// <remarks>This is distinct from <see cref="DisplayName"/>,the name shown on the web interface.</remarks>
        public string Filename { get; }
        
        /// <summary>
        /// The MIME content type of the file.
        /// </summary>
        public string ContentType { get; }
        
        /// <summary>
        /// The URL to the file. The file can be downloaded from this URL.
        /// </summary>
        public string Url { get; }
        
        /// <summary>
        /// The file's size in bytes.
        /// </summary>
        public ulong Size { get; }
        
        /// <summary>
        /// When the file was created.
        /// </summary>
        public DateTime? CreatedAt { get; }
        
        /// <summary>
        /// When the file was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; }
        
        /// <summary>
        /// When the file is set to unlock.
        /// </summary>
        public DateTime? UnlockAt { get; }
        
        /// <summary>
        /// Whether or not the file is locked.
        /// </summary>
        public bool Locked { get; }
        
        /// <summary>
        /// Whether or not the file is hidden.
        /// </summary>
        public bool Hidden { get; }
        
        /// <summary>
        /// When the file is set to lock.
        /// </summary>
        public DateTime? LockAt { get; }
        
        /// <summary>
        /// Whether or not the file is hidden for the file's owner.
        /// </summary>
        public bool HiddenForUser { get; }
        
        /// <summary>
        /// A URL to a thumbnail of the file.
        /// </summary>
        public string ThumbnailUrl { get; }
        
        /// <summary>
        /// When the file was last modified.
        /// </summary>
        public DateTime? ModifiedAt { get; }
        
        /// <summary>
        /// A simplified <see cref="ContentType"/>.
        /// </summary>
        public string MimeClass { get; }
        
        /// <summary>
        /// An opaque ID used by third-party systems.
        /// </summary>
        public string MediaEntryId { get; }
        
        /// <summary>
        /// Whether or not the file is locked for the file's owner.
        /// </summary>
        public bool LockedForUser { get; }
        
        /// <summary>
        /// If the file is locked, a <see cref="LockInfo"/> object.
        /// </summary>
        [CanBeNull]
        public LockInfo LockInfo { get; }
        
        /// <summary>
        /// If the file is locked, the reason it is locked.
        /// </summary>
        public string LockExplanation { get; }
        
        /// <summary>
        /// If the file was submitted through a submission endpoint, a URL to the document preview.
        /// </summary>
        public string PreviewUrl { get; }

        internal CanvasFile(Api api, CanvasFileModel model) {
            this.api = api;
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
        
        

        public string ToPrettyString() {
            return "CanvasFile { " +
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(Uuid)}: {Uuid}," +
                   $"\n{nameof(FolderId)}: {FolderId}," +
                   $"\n{nameof(DisplayName)}: {DisplayName}," +
                   $"\n{nameof(Filename)}: {Filename}," +
                   $"\n{nameof(ContentType)}: {ContentType}," +
                   $"\n{nameof(Url)}: {Url}," +
                   $"\n{nameof(Size)}: {Size}," +
                   $"\n{nameof(CreatedAt)}: {CreatedAt}," +
                   $"\n{nameof(UpdatedAt)}: {UpdatedAt}," +
                   $"\n{nameof(UnlockAt)}: {UnlockAt}," +
                   $"\n{nameof(Locked)}: {Locked}," +
                   $"\n{nameof(Hidden)}: {Hidden}," +
                   $"\n{nameof(LockAt)}: {LockAt}," +
                   $"\n{nameof(HiddenForUser)}: {HiddenForUser}," +
                   $"\n{nameof(ThumbnailUrl)}: {ThumbnailUrl}," +
                   $"\n{nameof(ModifiedAt)}: {ModifiedAt}," +
                   $"\n{nameof(MimeClass)}: {MimeClass}," +
                   $"\n{nameof(MediaEntryId)}: {MediaEntryId}," +
                   $"\n{nameof(LockedForUser)}: {LockedForUser}," +
                   $"\n{nameof(LockInfo)}: {LockInfo}," +
                   $"\n{nameof(LockExplanation)}: {LockExplanation}," +
                   $"\n{nameof(PreviewUrl)}: {PreviewUrl}").Indent(4) + 
                   "\n}";
        }

        public async Task<bool> MoveTo(ulong folderId, Api.OnDuplicate onDuplicate) {
            var r = await api.MovePersonalFile(Id, onDuplicate, destinationFolderId: folderId);
            
            if (r.Id != Id) {
                return false;
            }

            FolderId = r.FolderId;
            return true;
        }
        
        public Task<bool> MoveTo(Folder folder, Api.OnDuplicate onDuplicate) => MoveTo(folder.Id, onDuplicate);

        public async Task<bool> Rename(string newName, Api.OnDuplicate onDuplicate) {
            var r = await api.MovePersonalFile(Id, onDuplicate, newName);
            
            if (r.Id != Id) {
                return false;
            }

            FolderId = r.FolderId;
            return true;
        }

        public async Task<CanvasFile> CopyTo(ulong folderId, Api.OnDuplicate onDuplicate) {
            var r = await api.CopyPersonalFile(Id, folderId, onDuplicate);

            return r.Id == Id ? r
                              : null;
        }

        public Task<CanvasFile> CopyTo(Folder folder, Api.OnDuplicate onDuplicate) => CopyTo(folder.Id, onDuplicate);

        public Task<byte[]> Download() {
            return api.DownloadPersonalFile(this);
        }
    }
}