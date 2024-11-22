using OpenWeatherMapLogic;
using OpenWeatherMapLogic.JsonModelApi;
using Newtonsoft.Json;
using WeatherApi;
using DapperSqlite;
using System.Diagnostics.Eventing.Reader;


namespace WeatherApi.ConsoleOption;

public class ConsoleUI : ConsoleStartOptions, IDisposable
{
    CustomWeathermodel? custommodel;

    string? Cityname = "";

    Double? Latitude;
    Double? Longitude;

    bool Enablesqlitefetch = false;
   

    
    private readonly IDataServiceLink _dblink;
    

    public ConsoleUI(IServiceLink serviceLink , IDataServiceLink dblink ,IConfiguration config)
    {

      

        _serviceLink = serviceLink;
        _dblink =  dblink;
        _config = config;


        if (_config["EnableSqlite"].ToLower() == "true" )
        {
            Enablesqlitefetch = true;
        }
        
        if (MainOpenW.notValidApi)
        {
            ProgramLogicQuit = true;
            return;
        }
      
    }
    public override async Task ConsoleLogic()
    {
 
        while (true)
        {

            Console.Write("Insert City Name : ");

            Cityname = Console.ReadLine();

            Console.Clear();

            if (!String.IsNullOrEmpty(Cityname))
            {
                Cityname = Cityname.ToLower();
                Cityname = Cityname[0].ToString().ToUpper() + Cityname.Substring(1);
                break;
            }
                


        }


        if(Enablesqlitefetch)
        {
             custommodel = await _dblink.CityExist(Cityname!);
        }
        else
        {
             custommodel = null;
        }

      

        if (custommodel is null )
        {
            if(await DisplayCityInfo()) return;
            await DisplayWeatherInfo(custommodel);
        }
        else
        {
            await DisplayWeatherInfo(custommodel);
        }


    }



    public async Task<bool> DisplayCityInfo()
    {

        List<ApiModels.City>? pep = await _serviceLink.GetCityInformation(Cityname);


        if (pep.Count == 0)
        {
            Console.WriteLine("Invalid City");
           return true;
            
        }        
        else
        {
            Latitude = pep[0].Lat;
            Longitude = pep[0].Lon;
        }

        foreach (var cityinfo in pep)
        {
            Console.WriteLine($"Name: {cityinfo.Name}");
            Console.WriteLine($"Latitude: {cityinfo.Lat}");
            Console.WriteLine($"Longitude: {cityinfo.Lon}");
            Console.WriteLine($"Country: {cityinfo.Country}");
            Console.WriteLine($"State: {cityinfo.State}");
        }
        

        return false;
    }



    public async Task DisplayWeatherInfo(CustomWeathermodel custoWmodel)
    {
    

     
        string? response = string.Empty;


        if(custoWmodel is null )
        {
            custoWmodel = await _serviceLink.GetCityWeather(Latitude, Longitude, Cityname);

            if (Enablesqlitefetch) { 
                await _dblink.UpdateDbvalues(custoWmodel);
                }

            Console.WriteLine("Fetched from the Api");

        }
        else
        {
            Console.WriteLine("Fetched from the database");
        }



        
        Console.WriteLine($"Date time UTC now :{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")} ");
        Console.WriteLine("---------------------------------------------------------------------");

        Console.WriteLine("{0,-25} | {1,-24} | {2,-13}|", "WEATHER - DESCRIPTION", "TIME(yyyy-MM-dd) UTC/GMT", "TEMP");

        Console.WriteLine("--------------------------------------------------------------------|");

        foreach (var c in custoWmodel.CnameWeathers)
        {

            if (c.Datatime.Contains("12:00:00") || c.Datatime.Contains("18:00:00") || c.Datatime.Contains("6:00:00"))
            {
                Console.WriteLine("{0,-25} | {1,-24} | {2,-13}|", $"{c.Main} - {c.Description}", c.Datatime, $"{c.Temperatures[0].Celsius}C {c.Temperatures[0].Kelvin}K {c.Temperatures[0].Fahrenheit}F");
            }
            if (c.Datatime.Contains("18:00:00")) Console.WriteLine("--------------------------------------------------------------------|");
        }


        if (!custoWmodel.CnameWeathers[custoWmodel.CnameWeathers.Length - 1].Datatime.Contains("18:00:00")) Console.WriteLine("---------------------------------------------------------------------");


    }

    public void Dispose()
    {
        custommodel = null;
        Cityname = null;
        Latitude = null; 
        Longitude = null;
        _config = null;
        //_serviceLink = null;
    }




}


