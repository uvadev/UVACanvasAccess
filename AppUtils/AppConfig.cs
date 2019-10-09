using System.IO;
using JetBrains.Annotations;
using OneOf;
using Tomlyn;
using Tomlyn.Model;
using Tomlyn.Syntax;

namespace AppUtils {
    
    [PublicAPI]
    public class AppConfig {
        private TomlTable _config;

        public static OneOf<AppConfig, DiagnosticsBag> FromConfigPath(string configPath) {
            var syntaxTree = Toml.Parse(File.ReadAllText(configPath));

            if (syntaxTree.HasErrors) {
                return syntaxTree.Diagnostics;
            }

            return new AppConfig(syntaxTree);
        }
        
        private AppConfig(DocumentSyntax validatedSyntaxTree) {
            _config = validatedSyntaxTree.ToModel();
        }

        public TomlTable GetTable(string name) => (TomlTable) _config[name];
    }
}
