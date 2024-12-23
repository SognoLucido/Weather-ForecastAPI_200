using Shared.IMeteo;
using Shared.MeteoData;
using Shared.MeteoData.Models;
using Shared.MeteoData.Models.Dto;
using System.Text.Json;
using WeatherApi;


namespace OpenWeathermapMain
{
    public class OpenWeathermap(IHttpClientFactory httpClientFactory, MeteoApisBaseurls testurls) : IMeteoProvider
    {
        private readonly HttpClient client = httpClientFactory.CreateClient();
        private readonly MeteoApisBaseurls testurls1 = testurls;


        public async Task<GeoinfoplusProvider?> GeoinfoModel(string City, string key)
        {


            var Request = await client.GetAsync($"{testurls1.GETGeocodingOpenWeatherMap}/direct?q={City}&limit=3&appid={key}");

            var responseData = await Request.Content.ReadAsStringAsync();

            var DesData = JsonSerializer.Deserialize(responseData, GeoinfolistSGmodel.Default.ListGeoinfoModel);


            return new GeoinfoplusProvider
            {
                MeteoProvider = MeteoService.OpenWeathermap.ToString(),
                Geoinfo = DesData
            };



        }




        public async Task<ForecastDto> Forecast(double lat, double lon,int? limit, string? key)
        {
            var Request = await client.GetAsync($"{testurls1.GetForecastOpenWeatherMap}/forecast?lat={lat}&lon={lon}&appid={key}&units=metric");

            var responseData = await Request.Content.ReadAsStringAsync();

            var Serializ = JsonSerializer.Deserialize(responseData, ForecastSGmodel.Default.ForecastGETowmmodel);


            var result = MaptoForecastdto(Serializ,limit);


            result.MeteoProvider = MeteoService.OpenWeathermap.ToString();
            result.City = Serializ.city.name;
            result.Country_code = Serializ.city.country;
            result.lat = Serializ.city.coord.lat;
            result.lon = Serializ.city.coord.lon;


            return result;



        }



        private ForecastDto MaptoForecastdto(ForecastGETowmmodel model,int? limit) 
        {

            List<Data> datas = [];

            for (int i = 0; i < model.list.Count;)
            {

                var splitter = model.list[i].dt_txt.Split(' ');
                DateOnly testdateonly = DateOnly.Parse(splitter[0]);

                Data data = new()
                {
                    Date = testdateonly,
                };

                var clone = testdateonly;
                clone = clone.AddDays(1);



                for (int e = i; ; e++)
                {
                    if (e >= model.list.Count)
                    {
                        i = e;
                        break;
                    }

                    var split = model.list[e].dt_txt.Split(' ');

                    testdateonly = DateOnly.Parse(split[0]);

                    if (testdateonly >= clone)
                    {
                        i = e;

                        break;
                    }


                    data.Day.Add(
                        new Day
                        (
                            split[1],
                            model.list[e].main.temp,
                            model.list[e].weather[0].description
                        )
                        );

                    testdateonly = DateOnly.Parse(split[0]);

                }

                datas.Add(data);

                if(limit is not null)
                if (datas.Count >= limit) break; 
            }

            return new() 
            { 
                datas = datas,
            };
        }




    }
}
