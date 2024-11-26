


namespace DapperSqlite;

/*

public class DataManager : IDataServiceLink
{
    private readonly IConfiguration _config;

    public DataManager(IConfiguration config)
    {
        _config = config;
    }




    public async Task UpdateDbvalues(CustomWeathermodel weathermodel)
    {


        using (var connection = new SQLiteConnection(_config.GetConnectionString("Sqlite"))) 
        {


           
                var sql = "INSERT INTO MainCheck (City, Timeout) VALUES (@CityNameModel, @Datatime); SELECT last_insert_rowid(); ";

                var idreturn = connection.QuerySingle<int>(sql, new { CityNameModel = weathermodel.CityNameModel, Datatime = weathermodel.CnameWeathers[0].Datatime });



                sql = "INSERT INTO Datacity (id, data_time, simple_description, detailed_description, temp_C, temp_F, temp_K) VALUES(@Mainid, @Time, @Simpledescr, @Detaileddescr, @tempC, @tempF, @tempK); ";


                try 
                {

                   foreach(var item in weathermodel.CnameWeathers) 
                    {

                     await connection.ExecuteAsync(sql, new { Mainid = idreturn, Time = item.Datatime , Simpledescr = item.Main , Detaileddescr = item.Description , tempC = item.Temperatures[0].Celsius , tempF = item.Temperatures[0].Fahrenheit , tempK = item.Temperatures[0].Kelvin });
                    }

                   

                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                        
       
        }


    }






    public async Task<CustomWeathermodel?> CityExist(string Cityinmethod)
    {


        using (var connection = new SQLiteConnection(_config.GetConnectionString("Sqlite")))
        {
            var sql = "SELECT * FROM MainCheck WHERE City=@city ORDER BY Timeout DESC , id DESC";

            try
            {
                var x = await connection.QueryFirstOrDefaultAsync<TableMainCheckdbmodel>(sql, new { city = Cityinmethod });

                if (x != null)
                {

                    DateTime dbDateTime;

                    if (DateTime.TryParse(x.Timeout, out dbDateTime))
                    {

                        // TimeSpan difference = DateTime.UtcNow - dbDateTime;

                        var y = DateTime.UtcNow - dbDateTime;     
                      

                        if (y.TotalHours < 2)  // care : update the database with the new valor or not
                        {
                            var customWeathermodel = new CustomWeathermodel();

                            sql = "SELECT * FROM Datacity WHERE id=@id ORDER BY data_time ASC";

                            var pep = connection.Query<TableDatacitydbmodel>(sql, new { id = x.Id }).ToList();



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

                            customWeathermodel.Datafrom = "sqlserver";

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
*/