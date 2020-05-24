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

            var response = await _client.PostAsync($"accounts/{accountId.IdOrSelf()}/sis_imports" + args, bytesContent);

            var model = JsonConvert.DeserializeObject<SisImportModel>(await response.Content.ReadAsStringAsync());
            return new SisImport(this, model);
        }

        public async Task<SisImport> GetSisImport(ulong id, ulong? accountId = null) {
            var response = await _client.GetAsync($"accounts/{accountId.IdOrSelf()}/sis_imports/{id}");
            
            var model = JsonConvert.DeserializeObject<SisImportModel>(await response.Content.ReadAsStringAsync());
            return new SisImport(this, model);
        }
    }
}
