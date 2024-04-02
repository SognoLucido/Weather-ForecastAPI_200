using OpenWeatherMapLogic;
using OpenWeatherMapLogic.JsonModelApi;
using Newtonsoft.Json;
using WeatherApi_console;


namespace WeatherApi_console.ConsoleOption
{
    public class ConsoleUI : ConsoleStartOptions, IDisposable
    {


        Double? Latitude;
        Double? Longitude;

        string Cityok = string.Empty;
        //public ConsoleUI(IConfiguration config,IServiceLink serviceLink)
        //{
        //   _serviceLink = serviceLink;

        //    Keyz = config["OpenweathermapApi:ApiKey"];
        //    if(String.IsNullOrEmpty(Keyz))
        //    {
        //        Console.WriteLine("key not Found");
        //        ProgramLogicQuit = true;
        //        return;
        //    }

        //}


        public ConsoleUI(IServiceLink serviceLink)
        {


            _serviceLink = serviceLink;
            //serviceLink = services.GetRequiredService<IServiceLink>();
            if (MainOpenW.ValidApi)
            {
                ProgramLogicQuit = true;
                return;
            }
            //Keyz = config.GetValue<string>("OpenweathermapApi:ApiKey");
            //if (String.IsNullOrEmpty(Keyz) || Keyz.Length <= 25)
            //{
            //    Console.WriteLine("key not Found / Invalid key");
            //    ProgramLogicQuit = true;
            //    return;
            //}

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

            //_serviceLink.Apikey = Keyz;
            //_serviceLink.CityName = Cityok;


            await DisplayCityInfo();
            await DisplayWeatherInfo();








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
            }
            Console.WriteLine("---------------------------------------------------------------------");



        }



        public async Task DisplayWeatherInfo()
        {


            CustomWeathermodel custoWmodel = new CustomWeathermodel();

            string? response = string.Empty;


            custoWmodel = await _serviceLink.GetCityWeather(Latitude, Longitude);


            //ApiModels.Root myDeserializedClass = JsonConvert.DeserializeObject<ApiModels.Root>(response);



            //if (true) ;


            //CustomWeathermodel custoWmodel = new()
            //{
            //    CityNameModel = Cityok,
            //    CnameWeathers = new CustomWeather[myDeserializedClass.list.Count]
            //};







            //for (int i = 0; i < myDeserializedClass.list.Count; i++)
            //{

            //    custoWmodel.CnameWeathers[i] = new CustomWeather
            //    {
            //        Datatime = myDeserializedClass.list[i].dt_txt,
            //        Main = myDeserializedClass.list[i].weather[0].main,

            //        Temperatures = new List<Temp>
            //        {
            //           new Temp 
            //           { 
            //               Celsius = TempConversion.TempConversionKtoC(myDeserializedClass.list[i].main.temp),
            //                Fahrenheit = TempConversion.TempConversionKtoF(myDeserializedClass.list[i].main.temp),
            //                Kelvin = TempConversion.Tempcastingtoint(myDeserializedClass.list[i].main.temp)
            //           }
                     

            //        }

            //    };


            //    //custoWmodel.CnameWeathers[i].Main = myDeserializedClass.list[i].weather[0].main;

            //    //custoWmodel.CnameWeathers[i].Temperatures[0].Celsius = WeatherForecast.TempConversionKtoC(myDeserializedClass.list[i].main.temp);
            //    //custoWmodel.CnameWeathers[i].Temperatures[0].Fahrenheit = WeatherForecast.TempConversionKtoF(myDeserializedClass.list[i].main.temp);
            //    //custoWmodel.CnameWeathers[i].Temperatures[0].Kelvin = WeatherForecast.Tempcastingtoint(myDeserializedClass.list[i].main.temp);
            //    // custoWmodel.CnameWeathers[i] = myDeserializedClass.list[i].dt_txt;

            //}





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
    




            //for (int i = 0; i < myDeserializedClass.list.Count; i++)
            //{

            //    var x = custoWmodel.CnameWeathers[i];
            //   // custoWmodel.CnameWeathers[i] = myDeserializedClass.list[i].dt_txt;

            //}

              Console.WriteLine();

        }

        public void Dispose()
        {
           
            _serviceLink = null;
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
