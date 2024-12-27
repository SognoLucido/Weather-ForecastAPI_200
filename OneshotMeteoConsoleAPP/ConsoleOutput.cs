using Microsoft.Extensions.Configuration;
using OpenWeathermapMain;
using Shared.IMeteo;
using System.Text.RegularExpressions;

namespace OneshotMeteoConsoleAPP
{
    internal class ConsoleOutput
    {
        private readonly IMeteoProvider meteo;
        private string? key;

        public ConsoleOutput(IMeteoProvider _meteo, IConfiguration conf)
        {
            if (_meteo is OpenWeathermap)
            {
                key = conf.GetValue<string>("OpenweathermapApi:ApiKey");
                if (string.IsNullOrEmpty(key)) throw new InvalidOperationException("api key not found (OpenweathermapApi:ApiKey) in appsettings.json");
            }

            meteo = _meteo;
        }

        public async Task MeteoCall(Mode? mode)
        {
            while (true)
            {
                Console.Clear();

                switch (mode)
                {
                    case Mode.Geolocation:
                        await Geologic();
                        break;
                    case Mode.Forecast:
                        await Forecastlogic();
                        break;
                    default: throw new NotImplementedException();
                }


                Console.WriteLine();
                Console.WriteLine("Press enter to exit , else Continue");
                var input = Console.ReadKey(intercept: true);

                if(input.Key == ConsoleKey.Enter) break;

                Console.Clear();
            }

            
        }



        private async Task Geologic()
        {

            Regex regex = new("^[a-zA-Z]+$");

            while (true)
            {
                Console.WriteLine("City name : ");
                var input = Console.ReadLine();

                if (!regex.IsMatch(input ?? ""))
                {
                    Console.WriteLine("Invalid input. Only alphabetic characters are allowed.");
                    continue;
                }

                var Data = await meteo.GeoinfoModel(input!, key);

                if (Data is null)
                {
                    Console.WriteLine("unable to fetch the geolocation,try again");
                    continue;
                }

                Console.WriteLine($"MeteoProvider :{Data.MeteoProvider}");

                foreach (var item in Data.Geoinfo)
                {
                    Console.WriteLine($"City: {item.name} - lat: {item.lat} - lon: {item.lon} - country: {item.country} - state/other {item.state ?? "*empty*"} ");
                }

                break;
            }







        }



        private async Task Forecastlogic()
        {

            double lat, lon;

            for (; ; )
            {


                while (true)
                {
                    Console.Write("Latitude : ");
                    var input = Console.ReadLine();

                    if (double.TryParse(input, out lat)) break;
                    else Console.WriteLine("invalid input ,try again");


                }

                while (true)
                {
                    Console.Write("Longitude : ");
                    var input = Console.ReadLine();

                    if (double.TryParse(input, out lon)) break;
                    else Console.WriteLine("invalid input ,try again");

                }

                var data = await meteo.Forecast(lat, lon, null, key);

                if (data is null)
                {
                    Console.WriteLine("unable to fetch the geolocation,try again");
                    continue;
                }

                Console.Clear();

                Console.WriteLine("MeteoProvider : " +  data.MeteoProvider + " time : UTC" + " temp : C°");

                if (meteo is OpenWeathermap) Console.Write($"City: {data.City} -  Country code: {data.Country_code} - ");

                Console.WriteLine($"lat: {data.lat} - lon: {data.lon}");

                foreach (var item in data.datas)
                {
                    Console.WriteLine(item.Date);

                    foreach (var item2 in item.Day)
                    {
                        Console.WriteLine($"Time:{item2.Time} - Temp:{item2.Temp} - Description: {item2.Description}");
                    }

                }



                break;
            }

        }



    }
}
