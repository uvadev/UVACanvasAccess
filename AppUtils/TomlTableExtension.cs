using Tomlyn.Model;

namespace AppUtils {
    public static class TomlTableExtension {
        public static T Get<T>(this TomlTable table, string key) => (T) table[key];
        public static int GetInt(this TomlTable table, string key)=> (int) (long) table[key];
    }
}
