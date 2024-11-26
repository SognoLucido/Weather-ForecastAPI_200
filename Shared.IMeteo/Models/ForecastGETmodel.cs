using Shared.MeteoData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.MeteoData.Models
{
    public class ForecastGETmodel
    {
 
        public List<List> list { get; set; }
        public City city { get; set; }

    }


    public class City
    {   
        public string name { get; set; }
        public Coord coord { get; set; }
        public string country { get; set; }

    }

 

    public class Coord
    {
        public double lat { get; set; }
        public double lon { get; set; }
    }

    public class List
    {
        public Main main { get; set; } //temp
        public List<Weather> weather { get; set; }    //meteo
        public string dt_txt { get; set; }  // time
    }

    public class Main
    {
        public double temp { get; set; }
      
    }



    public class Weather
    {
       
      //  public string main { get; set; }
        public string description { get; set; }
       
    }


}


[JsonSerializable(typeof(ForecastGETmodel))]
public partial class ForecastSGmodel : JsonSerializerContext { }




