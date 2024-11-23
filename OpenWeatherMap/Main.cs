using Shared.IMeteo;
using Shared.MeteoData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeathermapMain
{
    public class OpenWeathermap(IHttpClientFactory httpClientFactory) : IMeteoProvider
    {
        private readonly HttpClient client = httpClientFactory.CreateClient();

        private string apikey = "awe";

        public async Task<string> ez()
        {
            return "DA OPENWEATHERMAP";
        }

        public async Task<List<GeoinfoModel>?> GeoinfoModel(string City)
        {


            var resp = await client.GetFromJsonAsync<List<GeoinfoModel>>($"https://api.openweathermap.org/geo/1.0/direct?q={City}&limit=3&appid={apikey}");

            return new();
        }
    }
}
