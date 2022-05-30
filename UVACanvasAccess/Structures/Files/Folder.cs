using System;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Files;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Files {
    
    /// <summary>
    /// Represents a folder on the Canvas server. Folders contain <see cref="CanvasFile"/>s.
    /// </summary>
    [PublicAPI]
    public class Folder : IPrettyPrint {
        private readonly Api api;
        
        /// <summary>
        /// The context of the folder; e.g. course, group, user, etc.
        /// </summary>
        public string ContextType { get; }

        /// <summary>
        /// The id of the folder context. <see cref="Folder.ContextType"/> determines the type of this id.
        /// </summary>
        public ulong ContextId { get; }

        /// <summary>
        /// The number of files in the folder.
        /// </summary>
        public uint FilesCount { get; }
        
        /// <summary>
        /// The visual position of this folder relative to other folders in the same parent folder.
        /// </summary>
        public int? Position { get; }
        
        /// <summary>
        /// When the folder was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; }

        /// <summary>
        /// The URL to this folder.
        /// </summary>
        public string FoldersUrl { get; }
        
        /// <summary>
        /// The URL to the files within this folder.
        /// </summary>
        public string FilesUrl { get; }
        
        /// <summary>
        /// The full path of this folder.
        /// </summary>
        public string FullName { get; }

        /// <summary>
        /// When the folder is set to lock.
        /// </summary>
        public DateTime? LockAt { get; }
        
        /// <summary>
        /// The folder id.
        /// </summary>
        public ulong Id { get; }
        
        /// <summary>
        /// The number of folders in the folder.
        /// </summary>
        public uint FoldersCount { get; }
        
        /// <summary>
        /// The name of the folder.
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// The id of the folder's parent folder, if it exists.
        /// </summary>
        public ulong? ParentFolderId { get; }

        /// <summary>
        /// When the folder was created.
        /// </summary>
        public DateTime CreatedAt { get; }

        /// <summary>
        /// When the folder is set to unlock.
        /// </summary>
        public DateTime? UnlockAt { get; }

        /// <summary>
        /// Whether or not the folder is hidden.
        /// </summary>
        public bool? Hidden { get; }
        
        /// <summary>
        /// Whether or not the folder is hidden for its owner.
        /// </summary>
        public bool? HiddenForUser { get; }

        /// <summary>
        /// Whether or not the folder is locked.
        /// </summary>
        public bool? Locked { get; }
        
        /// <summary>
        /// Whether or not the folder is locked for its owner.
        /// </summary>
        public bool? LockedForUser { get; }
        
        /// <summary>
        /// Whether or not the folder is designated for assignment submissions.
        /// </summary>
        public bool? ForSubmissions { get; }

        internal Folder(Api api, FolderModel model) {
            this.api = api;
            ContextType = model.ContextType;
            ContextId = model.ContextId;
            FilesCount = model.FilesCount;
            Position = model.Position;
            UpdatedAt = model.UpdatedAt;
            FoldersUrl = model.FoldersUrl;
            FilesUrl = model.FilesUrl;
            FullName = model.FullName;
            LockAt = model.LockAt;
            Id = model.Id;
            FoldersCount = model.FoldersCount;
            Name = model.Name;
            ParentFolderId = model.ParentFolderId;
            CreatedAt = model.CreatedAt;
            UnlockAt = model.UnlockAt;
            Hidden = model.Hidden;
            HiddenForUser = model.HiddenForUser;
            Locked = model.Locked;
            LockedForUser = model.LockedForUser;
            ForSubmissions = model.ForSubmissions;
        }

        /// <inheritdoc /> 
        public string ToPrettyString() {
            return "Folder {" + 
                   ($"\n{nameof(ContextType)}: {ContextType}," +
                   $"\n{nameof(ContextId)}: {ContextId}," +
                   $"\n{nameof(FilesCount)}: {FilesCount}," +
                   $"\n{nameof(Position)}: {Position}," +
                   $"\n{nameof(UpdatedAt)}: {UpdatedAt}," +
                   $"\n{nameof(FoldersUrl)}: {FoldersUrl}," +
                   $"\n{nameof(FilesUrl)}: {FilesUrl}," +
                   $"\n{nameof(FullName)}: {FullName}," +
                   $"\n{nameof(LockAt)}: {LockAt}," +
                   $"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(FoldersCount)}: {FoldersCount}," +
                   $"\n{nameof(Name)}: {Name}," +
                   $"\n{nameof(ParentFolderId)}: {ParentFolderId}," +
                   $"\n{nameof(CreatedAt)}: {CreatedAt}," +
                   $"\n{nameof(UnlockAt)}: {UnlockAt}," +
                   $"\n{nameof(Hidden)}: {Hidden}," +
                   $"\n{nameof(HiddenForUser)}: {HiddenForUser}," +
                   $"\n{nameof(Locked)}: {Locked}," +
                   $"\n{nameof(LockedForUser)}: {LockedForUser}," +
                   $"\n{nameof(ForSubmissions)}: {ForSubmissions}").Indent(4) + 
                   "\n}";
        }
    }
}
