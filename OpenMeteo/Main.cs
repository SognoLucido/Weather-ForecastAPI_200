using Shared.IMeteo;
using Shared.MeteoData.Models;
using Shared.MeteoData.Models.Dto;
using System.Text.Json;


namespace OpenMeteoMain
{
    public class OpenMeteo(IHttpClientFactory httpClientFactory) : IMeteoProvider
    {
       private readonly HttpClient client = httpClientFactory.CreateClient();

      
        public async Task<GeoinfoplusProvider?> GeoinfoModel(string City,string key)
        {



            var Request = await client.GetAsync($"https://geocoding-api.open-meteo.com/v1/search?name={City}&count=3&format=json");

            var RequestTostring = await Request.Content.ReadAsStringAsync();

            var datamodel = JsonSerializer.Deserialize<List<GeoinfoOpenmeteoVariant>?>(RequestTostring, GeoinfoOpenmeteoVariantSGmodel.Default.ListGeoinfoOpenmeteoVariant);

            Console.WriteLine();

            return new();
        }
    }
}
