using Shared.MeteoData.Models;
using Shared.MeteoData.Models.Dto;

namespace Shared.IMeteo
{
   
    public interface IMeteoProvider
    {
       
        Task<GeoinfoplusProvider?> GeoinfoModel(string City,string? key);

        Task<ForecastDto?> Forecast(double lat, double lon,int? limit, string? key);


    }
}
