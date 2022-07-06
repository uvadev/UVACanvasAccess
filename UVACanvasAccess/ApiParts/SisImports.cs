using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UVACanvasAccess.Model.SisImports;
using UVACanvasAccess.Structures.SisImports;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {

        /// <summary>
        /// Performs a SIS import.
        /// </summary>
        /// <param name="file">The file containing the import data.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="accountId">The account id. Defaults to the current account.</param>
        /// <param name="batchMode">
        /// Whether the import should be run in batch mode. Normally, when the import file is missing an entry currently
        /// present in Canvas, the entry is left alone. In batch mode, it will be deleted instead. Defaults to false.
        /// </param>
        /// <param name="overrideStickiness">Whether the import should override SIS stickiness.</param>
        /// <returns>The <see cref="SisImport"/>.</returns>
        public async Task<SisImport> ImportSisData([NotNull] byte[] file, 
                                                   [NotNull] string filePath, 
                                                   ulong? accountId = null,
                                                   bool batchMode = false,
                                                   bool? overrideStickiness = null) {
            var contentType = MimeMapping.GetMimeMapping(filePath);

            if (filePath.EndsWith(".csv")) {
                contentType = "text/csv"; // C# doesn't understand what a csv is
            }

            var args = BuildQueryString(new[] {
                ("batch_mode", batchMode.ToShortString()),
                ("override_sis_stickiness", overrideStickiness?.ToShortString())
            });
            
            var bytesContent = new ByteArrayContent(file);
            bytesContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            var response = await client.PostAsync($"accounts/{accountId.IdOrSelf()}/sis_imports" + args, bytesContent);

            var model = JsonConvert.DeserializeObject<SisImportModel>(await response.Content.ReadAsStringAsync());
            return new SisImport(this, model);
        }

        /// <summary>
        /// Gets a previously performed SIS import.
        /// </summary>
        /// <param name="id">The import ID.</param>
        /// <param name="accountId">The account id. Defaults to the current account.</param>
        /// <returns>The <see cref="SisImport"/>.</returns>
        public async Task<SisImport> GetSisImport(ulong id, ulong? accountId = null) {
            var response = await client.GetAsync($"accounts/{accountId.IdOrSelf()}/sis_imports/{id}");
            
            var model = JsonConvert.DeserializeObject<SisImportModel>(await response.Content.ReadAsStringAsync());
            return new SisImport(this, model);
        }
    }
}
