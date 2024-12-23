using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Shared.MeteoData.Models;
using Shared.MeteoData.Models.Dto;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WireMock.RequestBuilders;

namespace WeatherApi.Integration.test
{
    public class WTapiEndpointsTest(ProgramTestApplicationFactory factory) : IClassFixture<ProgramTestApplicationFactory>
    {
        private readonly ForecastMockAPI mockAPI = factory.Services.GetRequiredService<ForecastMockAPI>();
        private readonly JsonSerializerOptions opt = new() { PropertyNameCaseInsensitive = true };



        [Theory]
        [InlineData(MeteoService.OpenMeteo)]
        [InlineData(MeteoService.OpenWeathermap)]
        public async void GETGeoInfo(MeteoService meteoSe)
        {

            const string Cityname = "Genoa";

            mockAPI.CreateMockGeolocation(Cityname, meteoSe);
            var client2 = factory.CreateClient();

            ///////////////////////

            var GetGeodata = await client2.GetAsync($"api/geocoding/{Cityname}?meteoservice={meteoSe}");

            var result = await GetGeodata.Content.ReadAsStringAsync();

            var Desdata = JsonSerializer.Deserialize<GeoinfoplusProvider?>(result, opt);


            ///////////////////////

            Assert.Equal(HttpStatusCode.OK, GetGeodata.StatusCode);
            Assert.Equal(meteoSe.ToString(), Desdata.MeteoProvider);
            Assert.Equal(3, Desdata.Geoinfo.Count);
            Assert.Equal(Cityname.ToLower(), Desdata.Geoinfo[0].name.ToLower());
        }





        [Theory]
        [InlineData(MeteoService.OpenMeteo)]
        [InlineData(MeteoService.OpenWeathermap)]
        public async void GETForecast(MeteoService meteoSe)
        {
            //genoa coord
            const double Lat = 44.40; 
            const double Lon = 8.94;

            var client2 = factory.CreateClient();
            mockAPI.CreateMockForecast(Lat, Lon,meteoSe);



            ////////////////

            var Forecast = await client2.GetAsync($"api/forecast?lat={Lat.ToString(CultureInfo.InvariantCulture)}&lon={Lon.ToString(CultureInfo.InvariantCulture)}&meteoservice={meteoSe}");

            var result = await Forecast.Content.ReadAsStringAsync();

            var Desdata = JsonSerializer.Deserialize<ForecastDto>(result, opt);

            Console.WriteLine();
            ////////////////

            Assert.Equal(HttpStatusCode.OK, Forecast.StatusCode);
            Assert.Equal(meteoSe.ToString(), Desdata.MeteoProvider);

            if(meteoSe is MeteoService.OpenWeathermap)
            {
                Assert.Equal("Genoa", Desdata.City);
            }


        }
    }
}
