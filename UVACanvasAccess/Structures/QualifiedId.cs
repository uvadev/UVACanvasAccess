using JetBrains.Annotations;
using UVACanvasAccess.Util;
using static UVACanvasAccess.Structures.ContextType;

namespace UVACanvasAccess.Structures {
    
    [PublicAPI]
    public enum ContextType : byte {
        [ApiRepresentation("course")]
        Course,
        [ApiRepresentation("group")]
        Group,
        [ApiRepresentation("user")]
        User
    }
    
    [PublicAPI]
    public readonly struct QualifiedId : IPrettyPrint {
        public ContextType Type { get; }
        public ulong Id { get; }
        
        internal string AsString => Type == User ? $"{Id}"
                                                 : $"{Type.GetApiRepresentation()}_{Id}";
        
        public static implicit operator QualifiedId(ulong id) => new QualifiedId(id);
        public static implicit operator QualifiedId(int id) => new QualifiedId((ulong) id);

        public QualifiedId(ulong id) {
            Id = id;
            Type = User;
        }
        
        public QualifiedId(ulong id, ContextType type) {
            Id = id;
            Type = type;
        }
        
        public string ToPrettyString() {
            return $"QualifiedId ({AsString}) {{" +
                   ($"\n{nameof(Type)}: {Type}," +
                    $"\n{nameof(Id)}: {Id}").Indent(4) + 
                   "\n}";
        }
    }
}
