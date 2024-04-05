using DapperSqlite;
using OpenWeatherMapLogic;
using OpenWeatherMapLogic.JsonModelApi;

namespace WeatherApi_console.Controllers
{
    public partial class WeatherForecastController
    {

        bool Enablesqlitefetch = false;

        Double? Latitude;
        Double? Longitude;


        private readonly IServiceLink _serviceLink;
        private readonly IDataServiceLink _dataServiceLink;
        private readonly IConfiguration _configuration;

        public WeatherForecastController(IServiceLink serviceLink , IDataServiceLink dblink, IConfiguration config)
        {
           _serviceLink = serviceLink;
            _dataServiceLink = dblink;
            _configuration = config;

            if (config["EnableSqlite"].ToLower() == "true")
            {
                Enablesqlitefetch = true;
            }

            if (MainOpenW.notValidApi)
            {
                Console.WriteLine("invalid api key, please put a valid API key in appsettings.json");
                return;
            }

        }



        private async Task<bool> Checkinvalidcity()
        {
            List<ApiModels.City> pep = await _serviceLink.GetCityInformation(Cityname);

            if (pep.Count == 0) return true;
            else
            {
                Latitude = pep[0].Lat;
                Longitude = pep[0].Lon;
                return false;
            }
            
            
        }

       


        private async Task<CustomWeathermodel?> Fetchdbdata(string city)
        {
            CustomWeathermodel? custommodel = null ;

            if (Enablesqlitefetch)
            {
                custommodel = await _dataServiceLink.CityExist(city);

                return custommodel;

            }
            else return custommodel;

        }




        private async Task Saveresulttodb(CustomWeathermodel custoWmodel )
        {

            await _dataServiceLink.UpdateDbvalues(custoWmodel);
        }



        private string UpcaseCityname(string Cityn)
        {
            Cityn = Cityn.ToLower();
            Cityn = Cityn[0].ToString().ToUpper() + Cityn.Substring(1);

            return Cityn;
        }



    }
}
