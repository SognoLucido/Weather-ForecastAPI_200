

using Shared.IMeteo;
using Shared.MeteoData.Models;
using Shared.MeteoData.Models.Dto;
using System.Text.Json;


namespace OpenMeteoMain;

public class OpenMeteo(IHttpClientFactory httpClientFactory) : IMeteoProvider
{
   private readonly HttpClient client = httpClientFactory.CreateClient();


    public async Task<GeoinfoplusProvider?> GeoinfoModel(string City,string key)
    {



        var Request = await client.GetAsync($"https://geocoding-api.open-meteo.com/v1/search?name={City}&count=3&format=json");

        var RequestTostring = await Request.Content.ReadAsStringAsync();

        var datamodel = JsonSerializer.Deserialize<GeoinfoOpenmeteoVariant?>(RequestTostring, GeoinfoOpenmeteoVariantSGmodel.Default.GeoinfoOpenmeteoVariant);

        if (datamodel is null) return null;

        var resposte = new GeoinfoplusProvider
        {
            MeteoProvider = MeteoService.OpenMeteo.ToString(),
            Geoinfo = new(datamodel.results.Count)
        };

        Console.WriteLine();

        foreach(var data in datamodel.results)
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

   public async Task<ForecastDto> Forecast(double lat, double lon, string? key)
    {
        throw new NotImplementedException();

        WeatherCodes.codes.Clear();
        return new();
         
    }
}



