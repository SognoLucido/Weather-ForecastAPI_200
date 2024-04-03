
using Dapper;
using DapperSqlite.Model;
using Microsoft.Extensions.Configuration;
using OpenWeatherMapLogic.JsonModelApi;
using System.Data.SQLite;


namespace DapperSqlite;

public class DataManager : IDataServiceLink
{
    private readonly IConfiguration _config;

    public DataManager(IConfiguration config)
    {
        _config = config;
    }




    public async Task UpdateDbfreshvalues(CustomWeathermodel weathermodel)
    {



        using (var connection = new SQLiteConnection(_config.GetConnectionString("Sqlite"))) // TODO 
        {


            try
            {

              


                var sql = "INSERT INTO MainCheck (City, Timeout) VALUES (@CityNameModel, @Datatime)";

                var id = connection.Execute(sql, new { CityNameModel = weathermodel.CityNameModel, Datatime = weathermodel.CnameWeathers[0].Datatime });


                
                // Console.WriteLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }





        }




    }






    public async Task<CustomWeathermodel?> CityExist(string Cityinmethod)
    {


        using (var connection = new SQLiteConnection(_config.GetConnectionString("Sqlite")))
        {
            var sql = "SELECT * FROM MainCheck WHERE City=@city ORDER BY Timeout ASC";

            try
            {
                var x = await connection.QueryFirstOrDefaultAsync<Checkreturn>(sql, new { city = Cityinmethod });

                if (x != null)
                {

                    DateTime dbDateTime;

                    if (DateTime.TryParse(x.Timeout, out dbDateTime))
                    {

                        // TimeSpan difference = DateTime.UtcNow - dbDateTime;

                        var y = DateTime.UtcNow.Hour - dbDateTime.Hour;

                        if (y < 4)
                        {
                            var customWeathermodel = new CustomWeathermodel();

                            sql = "SELECT * FROM Datacity WHERE id=@id ORDER BY data_time ASC";

                            var pep = connection.Query<ReturnTableDatacitymodel>(sql, new { id = x.Id }).ToList();



                            customWeathermodel = new()
                            {
                                CityNameModel = x.City,
                                CnameWeathers = new CustomWeather[pep.Count()]
                            };

                            for (int i = 0; i < pep.Count(); i++)
                            {
                                customWeathermodel.CnameWeathers[i] = new CustomWeather
                                {
                                    Datatime = pep[i].data_time,
                                    Main = pep[i].simple_description,
                                    Description = pep[i].detailed_description,

                                    Temperatures = new List<Temp>
                                    {
                                       new Temp
                                       {
                                           Celsius = pep[i].temp_C,
                                            Fahrenheit = pep[i].temp_F,
                                            Kelvin = pep[i].temp_K
                                       }


                                    }

                                };
                            }



                            return customWeathermodel;



                        }
                        else return null;




                    }
                    else
                    {
                        Console.WriteLine("Error to datatime tryparse");
                        return null;
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
          //  Console.WriteLine();

        }


        return null;


    }


}
