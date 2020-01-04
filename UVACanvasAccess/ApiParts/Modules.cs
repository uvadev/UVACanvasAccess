using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UVACanvasAccess.Model.Modules;
using UVACanvasAccess.Structures.Modules;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {
        public enum ModuleIncludeType {
            None,
            TryIncludeItems,
            TryIncludeItemsAndContentDetails
        }

        public async IAsyncEnumerable<Module> StreamModules(ulong courseId,
                                                            [CanBeNull] string searchTerm = null,
                                                            ulong? studentId = null,
                                                            ModuleIncludeType includes = ModuleIncludeType.None) {
            IEnumerable<(string, string)> args = new[] {
                ("search_term", searchTerm),
                ("student_id", studentId?.ToString())
            };

            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (includes) {
                case ModuleIncludeType.TryIncludeItemsAndContentDetails:
                    args = args.Append(("include[]", "content_details"));
                    goto case ModuleIncludeType.TryIncludeItems; // `content_details` requires `items` to exist too
                case ModuleIncludeType.TryIncludeItems:
                    args = args.Append(("include[]", "items"));
                    break;
            }

            var response = await _client.GetAsync($"courses/{courseId}/modules" + BuildDuplicateKeyQueryString(args.ToArray()));

            await foreach (var model in StreamDeserializePages<ModuleModel>(response)) {
                yield return new Module(this, model);
            }
        }
    }
}
