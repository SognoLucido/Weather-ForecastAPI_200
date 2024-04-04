using OpenWeatherMapLogic.JsonModelApi;



namespace OpenWeatherMapLogic
{
    public interface IServiceLink
    {   
     
        Task<List<ApiModels.City>?> GetCityInformation(string city);

        Task<CustomWeathermodel?> GetCityWeather(double? Latitude, double? Longitude,string city);

        Task<bool> QuickvalidCheck();
    }
}
