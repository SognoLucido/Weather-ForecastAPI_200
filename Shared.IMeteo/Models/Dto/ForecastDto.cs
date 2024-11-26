using Shared.MeteoData.Models;
using Shared.MeteoData.Models.Dto;
using System.Text.Json.Serialization;

namespace Shared.MeteoData.Models.Dto
{
    public class ForecastDto
    {
        public string City { get; set; }
        public string Country_code { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public List<Data> datas { get; set; }

    }

    public class Data
    {
        public DateOnly Date { get; set; }
        public List<Day> Day { get; set; } = [];
    };



    public record Day
        (
            string Time,
            double Temp,
            string Description
        );

}

[JsonSerializable(typeof(ForecastDto))]
public partial class ForecastDtoSGmodel : JsonSerializerContext { }

