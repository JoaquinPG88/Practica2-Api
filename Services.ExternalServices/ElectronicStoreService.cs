using System.Net.Http;
using Newtonsoft.Json;
using Serilog;
using Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.ExternalServices
{
    public class ElectronicStoreService
    {
        public async Task<List<Electronic>> GetAllElectronicItems()
        {
            const string url = "https://api.restful-api.dev/objects";
            Log.Information("Conectando a {Url}", url);
            using var client = new HttpClient();
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Electronic>>(json);
        }
    }
}
