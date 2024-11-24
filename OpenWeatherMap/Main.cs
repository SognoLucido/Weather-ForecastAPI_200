using Shared.IMeteo;
using Shared.MeteoData.Models;
using Shared.MeteoData.Models.Dto;
using System.Text.Json;


namespace OpenWeathermapMain
{
    public class OpenWeathermap(IHttpClientFactory httpClientFactory) : IMeteoProvider 
    {
        private readonly HttpClient client = httpClientFactory.CreateClient();

      

        public async Task<GeoinfoplusProvider?> GeoinfoModel(string City,string key)
        {
            

            var wtf = await client.GetAsync($"https://api.openweathermap.org/geo/1.0/direct?q={City}&limit=3&appid={key}");

            var responseData = await wtf.Content.ReadAsStringAsync();

            var test =  JsonSerializer.Deserialize<List<GeoinfoModel>?>(responseData, GeoinfolistSGmodel.Default.ListGeoinfoModel);


            return  new GeoinfoplusProvider
            {
                MeteoProvider = MeteoService.OpenWeathermap.ToString(),
                Geoinfo = test
            };


            
        }
    }
}
