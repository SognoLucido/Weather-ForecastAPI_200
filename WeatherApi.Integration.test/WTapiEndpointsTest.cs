using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Shared.MeteoData.Models;
using Shared.MeteoData.Models.Dto;
using System.Net;
using System.Text.Json;
using WireMock.RequestBuilders;

namespace WeatherApi.Integration.test
{
    public class WTapiEndpointsTest(ProgramTestApplicationFactory factory) : IClassFixture<ProgramTestApplicationFactory>
    {
        private readonly ForecastMockAPI mockAPI = factory.Services.GetRequiredService<ForecastMockAPI>();
        private readonly JsonSerializerOptions opt  = new() { PropertyNameCaseInsensitive = true };



        [Theory]
        [InlineData(MeteoService.OpenMeteo)]
        //[InlineData(MeteoService.OpenWeathermap)] //todo
        public async void GETGeoInfo(MeteoService meteoSe)
        {

            const string Cityname = "Genoa";

            mockAPI.CreateMockGeolocation(Cityname);
            var client2 = factory.CreateClient();

            ///////////////////////
            
            var wtf = await client2.GetAsync($"api/geocoding/{Cityname}?meteoservice={meteoSe}");

            var result = await wtf.Content.ReadAsStringAsync();

            var Desdata =  JsonSerializer.Deserialize<GeoinfoplusProvider?>(result, opt);



            //var test = await client.GetAsync("hello");

            ///////////////////////

            Assert.Equal(HttpStatusCode.OK, wtf.StatusCode);
            Assert.Equal(meteoSe.ToString(),Desdata.MeteoProvider);
            Assert.Equal(3, Desdata.Geoinfo.Count);
            Assert.Equal(Cityname.ToLower(), Desdata.Geoinfo[0].name.ToLower());
        }
    }
}
