using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Shared.MeteoData
{
    public class MeteoApisBaseurls
    {
        public string GETGeocodingOpenApi { get;  }
        public string GETGeocodingOpenWeatherMap { get;  }

        public string GETForecastOpenApi {  get; }
        public string GetForecastOpenWeatherMap { get; }

        public MeteoApisBaseurls(string GETGeoInfoOM, string GETGeoInfoOWM ,string GETForeOM,string GETForeOWM) 
        {
            GETGeocodingOpenApi = GETGeoInfoOM;
            GETGeocodingOpenWeatherMap = GETGeoInfoOWM;
            GETForecastOpenApi = GETForeOM;
            GetForecastOpenWeatherMap = GETForeOWM;
        }


    }
}
