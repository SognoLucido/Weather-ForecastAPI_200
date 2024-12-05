using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Shared.IMeteo;
using Shared.MeteoData.Models;
using Shared.MeteoData.Models.Dto;

namespace HistoricalWeather.Sqlite
{

    public enum Tables
    {
        OpenWeatherMap,
        OpenWeatherMap_Details,
        OpenMeteo,
        OpenMeteo_Details

    }


    public class DBservice(IServiceProvider serviceProvider)
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        internal string Connstring { get; set; }

        public async Task Init()
        {

            //string test = "Data Source=weather.db;";

            try
            {
                using (var db = new SqliteConnection(Connstring))
                {

                    var CreateMainTable = $@"
                    CREATE TABLE IF NOT EXISTS {Tables.OpenMeteo} (
                        Date DATE NOT NULL,
                        Lat REAL NOT NULL,
                        Lon REAL NOT NULL,  
                        PRIMARY KEY (Date, Lat,Lon)
                    );";

                    var ForecastDetails = $@"
                    CREATE TABLE IF NOT EXISTS {Tables.OpenMeteo_Details} (
                        Lat REAL NOT NULL,
                        Lon REAL NOT NULL,        
                        Time TIME NOT NULL,
                        Description TEXT NOT NULL,
                        Temp REAL NOT NULL,                    
                        
                        FOREIGN KEY (Lat,Lon) REFERENCES {Tables.OpenMeteo} (Lat,Lon)
                    );";

                    await db.ExecuteAsync(CreateMainTable);
                    await db.ExecuteAsync(ForecastDetails);


                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task<bool> Fetchdata(double[] lat , double[] lon , MeteoService? meteo) //  forecast of the day
        {

            throw new NotImplementedException();    

            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedProvider = scope.ServiceProvider;

                var Ometeo = scopedProvider.GetKeyedService<IMeteoProvider>("openmeteo");
                var OWmeteo = scopedProvider.GetKeyedService<IMeteoProvider>("openweatermap");



                var pep = new ForecastDto[lat.Length];

                for (int i = 0; i < lat.Length; i++) 
                {

                }






            }

            return true;
        }

    }
}
