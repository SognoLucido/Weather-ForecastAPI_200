using Shared.IMeteo;
using Shared.MeteoData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OpenMeteoMain
{
    public class OpenMeteo(IHttpClientFactory httpClientFactory) : IMeteoProvider
    {
       private readonly HttpClient client = httpClientFactory.CreateClient();

        public async Task<string> ez()
        {
            return "DA OPENMETEO";
        }

        public async Task<List<GeoinfoModel>> GeoinfoModel(string City)
        {
            return new();
        }
    }
}
