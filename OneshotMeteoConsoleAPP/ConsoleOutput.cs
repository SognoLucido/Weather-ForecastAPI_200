using Microsoft.Extensions.Configuration;
using OpenMeteoMain;
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
                    case Mode.All:
                        await GeoplusForecast();
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



        private async Task GeoplusForecast()
        {

            var Geodata = await Geologic(true);

            await Forecastlogic(Geodata);

        }



        private async Task Geologic()
        {
            await Geologic(false);
        }

        private async Task<(double,double,(string,string)?)?> Geologic(bool Enablereturn )
        {

          

            Regex regex = new("^[a-zA-Z]+$");

            while (true)
            {
                Console.Write("City name : ");
                var input = Console.ReadLine();

                if (!regex.IsMatch(input ?? ""))
                {
                    Console.WriteLine("Invalid input. Only alphabetic characters are allowed.");
                    continue;
                }

                var Data = await meteo.GeoinfoModel(input!, key);

                if (Data is null)
                {
                    Console.WriteLine("unable to fetch the geolocation");
                    continue;
                }
                else if (Enablereturn) 
                {

                    return (Data.Geoinfo[0].lat, Data.Geoinfo[0].lon, (
                        meteo is OpenMeteo ? (Data.Geoinfo[0].name, Data.Geoinfo[0].country) : null
                        ));
                }

                Console.WriteLine($"MeteoProvider :{Data.MeteoProvider}");

                foreach (var item in Data.Geoinfo)
                {
                    Console.WriteLine($"City: {item.name} - lat: {item.lat} - lon: {item.lon} - country: {item.country} - state/other {item.state ?? "*empty*"} ");
                }

                break;
            }


            return null;


        }



        //private async Task Forecastlogic()
        //{
        //    await Forecastlogic(null);
        //}

        private async Task Forecastlogic((double, double, (string, string)?)? datax = null)
        {
           
            double lat = 0, lon = 0;

            if (datax is not null)
            {
                lat = datax.Value.Item1;
                lon = datax.Value.Item2;
            }

            for (; ; )
            {
                if (datax is null)
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
                }
                var data = await meteo.Forecast(lat, lon, null, key);

                if (data is null)
                {
                    Console.WriteLine("unable to fetch the forecast");
                    continue;
                }

                Console.Clear();

                //dont look here
                if (datax is not null && meteo is OpenMeteo)
                {
                    data.City = datax.Value.Item3.Value.Item1;
                    data.Country_code = datax.Value.Item3.Value.Item2;
                }


                Console.WriteLine("MeteoProvider : " +  data.MeteoProvider + " time : UTC" + " temp : C°");

                if (meteo is OpenWeathermap) Console.Write($"City: {data.City} -  Country code: {data.Country_code} - ");
                else if(meteo is OpenMeteo && datax is not null) Console.Write($"City: {data.City} -  Country code: {data.Country_code} - ");

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
