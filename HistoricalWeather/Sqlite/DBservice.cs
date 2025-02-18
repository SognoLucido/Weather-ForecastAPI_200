﻿using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.IMeteo;
using Shared.MeteoData.Models;
using Shared.MeteoData.Models.Dto;

[module: DapperAot]
namespace HistoricalWeather.Sqlite
{

    public enum Tables
    {

        MeteoInfo,
        Details

    }


    public class DBservice(IServiceProvider serviceProvider, IConfiguration config)
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
                    //await db.OpenAsync();

                    var CreateMainTable = $@"
                    CREATE TABLE IF NOT EXISTS {Tables.MeteoInfo} (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,                      
                        Date DATE NOT NULL,
                        Lat REAL NOT NULL,
                        Lon REAL NOT NULL, 
                        Provider TEXT 
                       
                    );";

                    var ForecastDetails = $@"
                    CREATE TABLE IF NOT EXISTS {Tables.Details} (
                     
                        MeteoId INTAGER NOT NULL,     
                        Time TIME ,
                        Description TEXT ,
                        Temp REAL ,                    
                        
                        FOREIGN KEY (MeteoId) REFERENCES {Tables.MeteoInfo} (Id)
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

        public async Task<bool> Fetchdata(double[]? lat, double[]? lon, MeteoService meteo) //  forecast of the day
        {
            ForecastDto[] Data = new ForecastDto[lat.Length];
            //throw new NotImplementedException();    
            MeteoService meteoservice = meteo;

            using (var scope = _serviceProvider.CreateScope())
            {
                var provider = scope.ServiceProvider;



                IMeteoProvider? request = null;
                string? key = null;


                switch (meteo)
                {
                    case MeteoService.OpenMeteo:
                        {

                            request = provider.GetKeyedService<IMeteoProvider>("openmeteo"); break;

                        }
                    case MeteoService.OpenWeathermap:
                        {

                            key = config.GetValue<string>("OpenweathermapApi:ApiKey");

                            if (key is null)
                            {
                                Console.WriteLine("Background serviceapikey Required check appsettings.json");
                                return false;
                            }

                            request = provider.GetKeyedService<IMeteoProvider>("openweatermap");


                        }; break;
                }



                for (int i = 0; i < Data.Length; i++)
                {
                    Data[i] = await request.Forecast(lat[i], lon[i], 1, key);
                }

            }


            var checkexist = $"SELECT Id FROM {Tables.MeteoInfo} WHERE Date = @Date AND Lat = @Lat AND Lon = @Lon AND Provider = @Provider ;";


           
            using (var db = new SqliteConnection(Connstring))
            {

                DateOnly? startdate = null;



                foreach (var item in Data)
                {
                    startdate ??= item.datas[0].Date;


                    var templat = Math.Truncate(item.lat * 100) / 100;
                    var templon = Math.Truncate(item.lon * 100) / 100;

                    var parameters = new
                    {
                        Date = startdate,
                        Lat = templat,
                        Lon = templon,
                        Provider = meteoservice.ToString(),
                    };

                    var existingId = await db.QueryFirstOrDefaultAsync<int?>(checkexist, parameters);



                    if (existingId is null)
                    {


                        var Maininsertsql =
                            $"INSERT INTO {Tables.MeteoInfo} (Date,Lat,Lon,Provider)" +
                            $"VALUES (@Date,@Lat,@Lon,@Provider);" +
                            $"SELECT last_insert_rowid();";


                        var id = await db.ExecuteScalarAsync(Maininsertsql,
                            new { Date = item.datas[0].Date, Lat = templat, Lon = templon, Provider = meteoservice.ToString() } );


                        var rowdetailInsertsql =
                      $"INSERT INTO {Tables.Details} (MeteoId ,Time,Description,Temp)" +
                      $" VALUES ({id},@Time,@Description,@Temp)";

                        foreach (var date in item.datas[0].Day) // why dapper ,you were the chosen one
                        {
                           
                            await db.ExecuteAsync(rowdetailInsertsql, 
                                new { Time = date.Time , Description = date.Description , Temp = date.Temp });


                        }
                    }


                }


            }



            return true;
        }

    }
}
