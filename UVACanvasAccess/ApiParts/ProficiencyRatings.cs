using System.Threading.Tasks;
using Newtonsoft.Json;
using UVACanvasAccess.Model.ProficiencyRatings;
using UVACanvasAccess.Structures.ProficiencyRatings;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {

        public async Task<Proficiency> GetProficiencyRatings(ulong? accountId = null) {
            var result = await client.GetAsync($"accounts/{accountId.IdOrSelf()}/outcome_proficiency");

            var model = JsonConvert.DeserializeObject<ProficiencyModel>(await result.Content.ReadAsStringAsync());
            return new Proficiency(this, model);
        }
    }
}
