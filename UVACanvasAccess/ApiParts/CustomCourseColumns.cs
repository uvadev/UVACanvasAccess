using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            var response = await client.GetAsync($"courses/{courseId}/custom_gradebook_columns" + args);

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

            var response = await client.PostAsync($"courses/{courseId}/custom_gradebook_columns", args);
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

            var response = await client.PutAsync($"courses/{courseId}/custom_gradebook_columns/{columnId}", args);
            var model = JsonConvert.DeserializeObject<CustomColumnModel>(await response.Content.ReadAsStringAsync());
            return new CustomColumn(this, model);
        }

        /// <summary>
        /// Deletes a gradebook column.
        /// </summary>
        /// <param name="columnId">The column id.</param>
        /// <param name="courseId">The course id.</param>
        /// <returns>The deleted column.</returns>
        public async Task<CustomColumn> DeleteCustomColumn(ulong columnId, ulong courseId) {
            var response = await client.DeleteAsync($"courses/{courseId}/custom_gradebook_columns/{columnId}");
            var model = JsonConvert.DeserializeObject<CustomColumnModel>(await response.Content.ReadAsStringAsync());
            return new CustomColumn(this, model);
        }

        /// <summary>
        /// Streams column entries.
        /// </summary>
        /// <param name="columnId">The column id.</param>
        /// <param name="courseId">The course id.</param>
        /// <returns>The stream of entries.</returns>
        public async IAsyncEnumerable<ColumnDatum> StreamColumnEntries(ulong columnId, ulong courseId) {
            var args = BuildQueryString(("include_hidden", true.ToShortString()));
            var response = await client.GetAsync($"courses/{courseId}/custom_gradebook_columns/{columnId}/data" + args);
            
            await foreach (var model in StreamDeserializePages<ColumnDatumModel>(response)) {
                yield return new ColumnDatum(this, model);
            }
        }
        
        /// <summary>
        /// Updates the content of a custom column entry.
        /// </summary>
        /// <param name="columnId">The column id.</param>
        /// <param name="courseId">The course id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="content">The content.</param>
        /// <returns>The updated entry.</returns>
        public async Task<ColumnDatum> UpdateColumnCustomEntry(ulong columnId, ulong courseId, ulong userId, string content) {
            var args = BuildHttpArguments(new[] {("column_data[content]", content)});
            var response = 
                await client.PutAsync($"courses/{courseId}/custom_gradebook_columns/{columnId}/data/{userId}", args);

            var model = JsonConvert.DeserializeObject<ColumnDatumModel>(await response.Content.ReadAsStringAsync());
            return new ColumnDatum(this, model);
        }

        /// <summary>
        /// Bulk update the contents of several custom column entries.
        /// </summary>
        /// <param name="courseId">The column id.</param>
        /// <param name="updates">The updates.</param>
        /// <returns></returns>
        public async Task UpdateCustomColumnEntries(ulong courseId, IEnumerable<ColumnEntryUpdate> updates) {
            var s = new JsonSerializerSettings {
                TypeNameHandling = TypeNameHandling.None
            };
            var o = new {
                column_data = updates
            };
            var body = BuildHttpJsonBody(JObject.FromObject(o, JsonSerializer.CreateDefault(s)));
            await client.PutAsync($"courses/{courseId}/custom_gradebook_column_data", body);
        }

        /// <summary>
        /// Represents one update to a custom column entry.
        /// </summary>
        /// <see cref="Api.UpdateCustomColumnEntries"/>
        public struct ColumnEntryUpdate {
            /// <summary>
            /// The column id.
            /// </summary>
            [JsonProperty("column_id")]
            public ulong ColumnId { get; set;  }
            
            /// <summary>
            /// The user id.
            /// </summary>
            [JsonProperty("user_id")]
            public ulong UserId { get; set; }
            
            /// <summary>
            /// The new content of the entry.
            /// </summary>
            [JsonProperty("content")]
            public string Content { get; set; }
        }
    }
}
