

using OpenMeteoMain.Model;
using Shared.IMeteo;
using Shared.MeteoData;
using Shared.MeteoData.Models;
using Shared.MeteoData.Models.Dto;
using System.Text.Json;


namespace OpenMeteoMain;

public class OpenMeteo(IHttpClientFactory httpClientFactory, MeteoApisBaseurls testurls) : IMeteoProvider
{
    private readonly HttpClient client = httpClientFactory.CreateClient();
    private readonly MeteoApisBaseurls testurls1 = testurls;

    public async Task<GeoinfoplusProvider?> GeoinfoModel(string City, string key)
    {



        var Request = await client.GetAsync($"{testurls1.GETGeocodingOpenApi}/search?name={City}&count=3&format=json");

        //var Request = await client.GetAsync($"{testurls1.GETGeocodingOpenApi}/search?");
        var RequestTostring = await Request.Content.ReadAsStringAsync();

        if (!Request.IsSuccessStatusCode) return null;

        var datamodel = JsonSerializer.Deserialize<GeoinfoOpenmeteoVariant?>(RequestTostring, GeoinfoOpenmeteoVariantSGmodel.Default.GeoinfoOpenmeteoVariant);

        if (datamodel.results is null) return null;

        var resposte = new GeoinfoplusProvider
        {
            MeteoProvider = MeteoService.OpenMeteo.ToString(),
            Geoinfo = new(datamodel.results.Count)
        };

        Console.WriteLine();

        foreach (var data in datamodel.results)
        {
            resposte.Geoinfo.Add
                (
                new()
                {
                    country = data.country_code,
                    lat = data.latitude,
                    lon = data.longitude,
                    name = data.name,
                    state = data.admin1

                }

                );
        }

        return resposte;
    }

    public async Task<ForecastDto?> Forecast(double lat, double lon,int? limit, string? key)
    {

        var Request = await client.GetAsync($"{testurls1.GETForecastOpenApi}/forecast?latitude={lat}&longitude={lon}&hourly=temperature_2m,weather_code");

        if (!Request.IsSuccessStatusCode) return null;

        var RequestTostring = await Request.Content.ReadAsStringAsync();

        var datamodel = JsonSerializer.Deserialize<ForecastGETommodel?>(RequestTostring, ForecastomVariantSGmodel.Default.ForecastGETommodel);


        var result = MaptoForecastdto(datamodel, limit);


        result.MeteoProvider = MeteoService.OpenMeteo.ToString();
        result.lat = datamodel.latitude;
        result.lon = datamodel.longitude;
       
        

        return result;

    }


    private ForecastDto MaptoForecastdto(ForecastGETommodel model,int? limit)
    {

        List<Data> datas = [];
        List<int> Takeindex = [];
        var modelsize = model.hourly.time.Count;


        var utcnow = DateTime.UtcNow;
        int startindex = 0;

        for(int y = 0; y < modelsize; y++)
        {
            if (DateTime.TryParse(model.hourly.time[y] , out var result))
            {
                if (result >= utcnow)
                {
                    startindex = y;
                    break;
                }
            }
        }

        startindex++;

        
        for(;startindex < modelsize; startindex++)
        {
            if (TimeOnly.Parse(model.hourly.time[startindex]).Hour % 3 == 0)
            {
                Takeindex.Add(startindex);
            }
        }



        for (int i = 0; i < Takeindex.Count;)
        {


            DateOnly testdateonly = DateOnly.Parse(model.hourly.time[Takeindex[i]]);

            Data data = new()
            {
                Date = testdateonly,
            };

            var clone = testdateonly;
            clone = clone.AddDays(1);



            for (int e = i; ; e++)
            {

                if (e >= Takeindex.Count)
                {
                    i = e;
                    break;
                }

                var split = model.hourly.time[Takeindex[e]].Split('T');

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
                    model.hourly.temperature_2m[Takeindex[e]],
                    ((WeatherCode)model.hourly.weather_code[Takeindex[e]]).ToString()
                    )
                    );


            }

            datas.Add(data);

            if (limit is not null)
            if (datas.Count >= limit) break;
        }

        return new()
        {
            datas = datas,
        };



    }


   
}



