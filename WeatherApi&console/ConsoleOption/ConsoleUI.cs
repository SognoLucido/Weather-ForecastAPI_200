using OpenWeatherMapLogic;
using OpenWeatherMapLogic.JsonModelApi;
using Newtonsoft.Json;
using WeatherApi_console;
using DapperSqlite;
using System.Diagnostics.Eventing.Reader;


namespace WeatherApi_console.ConsoleOption
{
    public class ConsoleUI : ConsoleStartOptions, IDisposable
    {


        Double? Latitude;
        Double? Longitude;


        private readonly IServiceLink? _serviceLink;
        private readonly IDataServiceLink _dblink;


        public ConsoleUI(IServiceLink serviceLink , IDataServiceLink dblink)
        {

            _serviceLink = serviceLink;
            _dblink =  dblink;

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

                MainOpenW.CityName = Console.ReadLine();

                Console.Clear();

                if (MainOpenW.CityName != "") break;

            }



             CustomWeathermodel? custommodel = await _dblink.CityExist(MainOpenW.CityName);

           // CustomWeathermodel? custommodel = null;

            if (custommodel is null)
            {
                await DisplayCityInfo();
                await DisplayWeatherInfo(custommodel);
            }
            else
            {
                await DisplayWeatherInfo(custommodel);
            }













            //  string response = await serviceLink.GetStringAsync("wtf");    
            //  HttpResponseMessage response = await _httpClient.GetAsync("https://api.ipify.org");
            //  response.EnsureSuccessStatusCode(); // Ensure success status code
            //string pep = await response.Content.ReadAsStringAsync();


        }



        public async Task DisplayCityInfo()
        {

            List<ApiModels.City>? pep = await _serviceLink.GetCityInformation();


            if (pep is null || (pep[0].Lat) is null || (pep[0].Lon) is null) return;
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
                Console.WriteLine($"Date time UTC now :{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")} ");
            }
            Console.WriteLine("---------------------------------------------------------------------");



        }



        public async Task DisplayWeatherInfo(CustomWeathermodel custoWmodel)
        {


           

            string? response = string.Empty;


            if(custoWmodel is null)
            {
                custoWmodel = await _serviceLink.GetCityWeather(Latitude, Longitude);


               await _dblink.UpdateDbfreshvalues(custoWmodel);

            }
           


         


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
           
          //  _serviceLink = null;
        }









        //public async Task DisplayWeatherInfo() // todoooooo
        //{

        //   string response = string.Empty;

        //    if (pep is null) return;

        //    foreach (var cityinfo in pep)
        //    {
        //        Console.WriteLine($"Name: {cityinfo.Name}");
        //        Console.WriteLine($"Latitude: {cityinfo.Lat}");
        //        Console.WriteLine($"Longitude: {cityinfo.Lon}");
        //        Console.WriteLine($"Country: {cityinfo.Country}");
        //        Console.WriteLine($"State: {cityinfo.State}");
        //    }
        //    Console.WriteLine("-----------------------------------------------------------");
        //}





    }

    }
