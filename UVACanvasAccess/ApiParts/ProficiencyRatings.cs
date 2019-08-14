using System.Threading.Tasks;
using Newtonsoft.Json;
using UVACanvasAccess.Model.ProficiencyRatings;
using UVACanvasAccess.Structures.ProficiencyRatings;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {

        public async Task<Proficiency> GetProficiencyRatings(ulong? accountId = null) {
            var result = await _client.GetAsync($"accounts/{accountId?.ToString() ?? "self"}/outcome_proficiency");

            var model = JsonConvert.DeserializeObject<ProficiencyModel>(await result.Content.ReadAsStringAsync());
            return new Proficiency(this, model);
        }
    }
}
