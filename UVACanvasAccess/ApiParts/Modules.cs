using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UVACanvasAccess.Model.Modules;
using UVACanvasAccess.Structures.Modules;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {
        
        /// <summary>
        /// Include modes for API methods returning <see cref="Module"/>.
        /// </summary>
        /// <remarks>
        /// Canvas allows these includes to be omitted even if requested if they exceed an unspecified size.
        /// Consider using explicit list items methods instead of relying on includes.
        /// </remarks>
        public enum ModuleIncludeType : byte {
            /// <summary>
            /// No additional includes requested.
            /// </summary>
            None,
            /// <summary>
            /// Try to include module items.
            /// </summary>
            TryIncludeItems,
            /// <summary>
            /// Try to include module items and content details.
            /// </summary>
            TryIncludeItemsAndContentDetails
        }

        /// <summary>
        /// Streams the list of modules for the given course.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="searchTerm">(Optional) A search term.</param>
        /// <param name="studentId">(Optional) A student id. Returns module completion information for this student if specified.</param>
        /// <param name="includes">(Optional) Additional data to request in the result.</param>
        /// <returns>The stream of modules.</returns>
        /// <remarks>
        /// Canvas does not guarantee that includes requested in <paramref name="includes"/> will actually be included.
        /// </remarks>
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

        /// <summary>
        /// Gets a module by its id and course id.
        /// </summary>
        /// <param name="moduleId">The module id.</param>
        /// <param name="courseId">The course id.</param>
        /// <param name="studentId">(Optional) A student id. Returns module completion information for this student if specified.</param>
        /// <param name="includes">(Optional) Additional data to request in the result.</param>
        /// <returns>The module.</returns>
        /// <remarks>
        /// Canvas does not guarantee that includes requested in <paramref name="includes"/> will actually be included.
        /// </remarks>
        public async Task<Module> GetModule(ulong moduleId, 
                                            ulong courseId,
                                            ulong? studentId = null,
                                            ModuleIncludeType includes = ModuleIncludeType.None) {
            IEnumerable<(string, string)> args = new[] {
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

            var response = await _client.GetAsync($"courses/{courseId}/modules/{moduleId}" + BuildDuplicateKeyQueryString(args.ToArray()));

            var model = JsonConvert.DeserializeObject<ModuleModel>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return new Module(this, model);
        }
    }
}
