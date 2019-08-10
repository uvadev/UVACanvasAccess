using System.Diagnostics;
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
    public enum SubEventContextType : byte {
        [ApiRepresentation("section")]
        Section,
        [ApiRepresentation("category")]
        Category
    }
    
    [PublicAPI]
    public readonly struct EventContext : IPrettyPrint {
        public EventContextType Type { get; }
        public SubEventContextType? SubType { get; }
        public ulong Id { get; }

        public EventContext(EventContextType type, ulong id) {
            Type = type;
            Id = id;
            SubType = null;
        }
        
        public EventContext(EventContextType type, SubEventContextType subType, ulong id) {
            Type = type;
            Id = id;
            SubType = subType;
        }

        internal EventContext(string contextCode) {
            string[] parts = contextCode.Split('_');
            Debug.Assert(parts.Length == 2 || parts.Length == 3);

            Type = (EventContextType) parts[0].ToApiRepresentedEnum<EventContextType>();
            if (parts.Length == 2) {
                SubType = null;
                Id = ulong.Parse(parts[1]);
            } else {
                SubType = (SubEventContextType) parts[1].ToApiRepresentedEnum<SubEventContextType>();
                Id = ulong.Parse(parts[2]);
            }
        }

        internal string ContextCode => SubType == null ? $"{Type.GetApiRepresentation()}_{Id}" 
                                                       : $"{Type.GetApiRepresentation()}_{SubType.GetApiRepresentation()}_{Id}";

        public string ToPrettyString() {
            return $"EventContext ({ContextCode}) {{" +
                   ($"\n{nameof(Type)}: {Type}," +
                   $"\n{nameof(SubType)}: {SubType}," +
                   $"\n{nameof(Id)}: {Id}").Indent(4) + 
                   "\n}";
        }
    }
}
