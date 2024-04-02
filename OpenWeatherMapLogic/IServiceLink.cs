using OpenWeatherMapLogic.JsonModelApi;



namespace OpenWeatherMapLogic
{
    public interface IServiceLink
    {
      
      //  string CityName { set; }

       

      //  bool ValidApi { get; }

        Task<List<ApiModels.City>?> GetCityInformation();

        Task<CustomWeathermodel?> GetCityWeather(double? Latitude, double? Longitude);

       // Task<string> GetStringAsync(string url);

    }
}
