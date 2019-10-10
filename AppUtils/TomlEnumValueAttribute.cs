using System;
using JetBrains.Annotations;

namespace AppUtils {
    
    [AttributeUsage(AttributeTargets.Field)]
    [PublicAPI]
    public sealed class TomlEnumValueAttribute : Attribute {
        public string Name { get; }

        public TomlEnumValueAttribute(string name) {
            Name = name;
        }
    }
}
