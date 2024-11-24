using System.Text.Json.Serialization;

namespace Shared.MeteoData.Models
{
   public enum MeteoService
    {
        OpenMeteo,
        OpenWeathermap
    }


    [JsonSerializable(typeof(MeteoService))]
    public partial class MeteoEnumJsonSerializerContext : JsonSerializerContext { }
   

}
