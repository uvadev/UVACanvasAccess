using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UVACanvasAccess.Model.CustomGradebookColumns;
using UVACanvasAccess.Structures.CustomGradebookColumns;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {

        /// <summary>
        /// Streams the custom gradebook columns in the given course.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="includeHidden">(Optional; default = false) If true, include hidden columns.</param>
        /// <returns>The stream of gradebook columns.</returns>
        public async IAsyncEnumerable<CustomColumn> StreamCustomGradebookColumns(ulong courseId, 
                                                                                 bool? includeHidden = null) {
            var args = BuildQueryString(("include_hidden", includeHidden?.ToShortString()));
            var response = await _client.GetAsync($"courses/{courseId}/custom_gradebook_columns" + args);

            await foreach (var model in StreamDeserializePages<CustomColumnModel>(response)) {
                yield return new CustomColumn(this, model);
            }
        }

        /// <summary>
        /// Creates a new custom gradebook column in the given course.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="title">The title of the column.</param>
        /// <param name="position">(Optional) The position of the column relative to other columns.</param>
        /// <param name="hidden">(Optional; default = false) If true, this column will be hidden.</param>
        /// <param name="teacherNotes">(Optional; default = false) If true, this column will be considered a teacher notes column.</param>
        /// <param name="readOnly">(Optional; default = false) If true, this column will not be editable through the UI.</param>
        /// <returns>The newly created column.</returns>
        public async Task<CustomColumn> CreateCustomColumn(ulong courseId,
                                                           string title, 
                                                           int? position = null,
                                                           bool? hidden = null,
                                                           bool? teacherNotes = null,
                                                           bool? readOnly = null) {
            var args = BuildHttpArguments(new [] {
                ("title", title),
                ("position", position?.ToString()),
                ("hidden", hidden?.ToShortString()),
                ("teacher_notes", teacherNotes?.ToShortString()),
                ("read_only", readOnly?.ToShortString())
            }.KeySelect(k => $"column[{k}]"));

            var response = await _client.PostAsync($"courses/{courseId}/custom_gradebook_columns", args);
            var model = JsonConvert.DeserializeObject<CustomColumnModel>(await response.Content.ReadAsStringAsync());
            return new CustomColumn(this, model);
        }

        /// <summary>
        /// Updates an existing gradebook column. Any omitted optional parameter is unchanged.
        /// </summary>
        /// <param name="columnId">The column id.</param>
        /// <param name="courseId">The course id.</param>
        /// <param name="title">(Optional) The title of the column.</param>
        /// <param name="position">(Optional) The position of the column relative to other columns.</param>
        /// <param name="hidden">(Optional) If true, this column will be hidden.</param>
        /// <param name="teacherNotes">(Optional) If true, this column will be considered a teacher notes column.</param>
        /// <param name="readOnly">(Optional) If true, this column will not be editable through the UI.</param>
        /// <returns>The newly updated column.</returns>
        public async Task<CustomColumn> UpdateCustomColumn(ulong columnId,
                                                           ulong courseId, 
                                                           string title = null, 
                                                           int? position = null,
                                                           bool? hidden = null,
                                                           bool? teacherNotes = null,
                                                           bool? readOnly = null) {
            var args = BuildHttpArguments(new [] {
                ("title", title),
                ("position", position?.ToString()),
                ("hidden", hidden?.ToShortString()),
                ("teacher_notes", teacherNotes?.ToShortString()),
                ("read_only", readOnly?.ToShortString())
            }.KeySelect(k => $"column[{k}]"));

            var response = await _client.PutAsync($"courses/{courseId}/custom_gradebook_columns/{columnId}", args);
            var model = JsonConvert.DeserializeObject<CustomColumnModel>(await response.Content.ReadAsStringAsync());
            return new CustomColumn(this, model);
        }
    }
}
