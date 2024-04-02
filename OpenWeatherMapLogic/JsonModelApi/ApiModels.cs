using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeatherMapLogic.JsonModelApi;

public class ApiModels // the original Openweather api model MAP
{

    public record City(string Name, double? Lat, double? Lon, string Country, string State);


    public class List
    {

        public List<Weather> weather { get; set; }
        public string dt_txt { get; set; }
        public Main main { get; set; }
    }

    public class Main
    {
        public double temp { get; set; }

    }

    public class Root
    {
        public List<List> list { get; set; }

    }

    public class Weather
    {
        public string main { get; set; }
    }

}
