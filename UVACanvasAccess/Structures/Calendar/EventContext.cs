using JetBrains.Annotations;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Calendar {

    [PublicAPI]
    public enum EventContextType : byte {
        [ApiRepresentation("course")]
        Course,
        [ApiRepresentation("group")]
        Group,
        [ApiRepresentation("user")]
        User
    }
    
    [PublicAPI]
    public readonly struct EventContext {
        public EventContextType Type { get; }
        public ulong Id { get; }

        public EventContext(EventContextType type, ulong id) {
            Type = type;
            Id = id;
        }

        internal string ContextCode => $"{Type.GetApiRepresentation()}_{Id}";
    }
}
