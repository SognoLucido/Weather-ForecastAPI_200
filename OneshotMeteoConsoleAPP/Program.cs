using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenMeteoMain;
using OpenWeathermapMain;
using Shared.IMeteo;
using Shared.MeteoData;
using Shared.MeteoData.Models;
using System;

namespace OneshotMeteoConsoleAPP
{
    internal class Program
    {

        static async Task Main(string[] args)
        {
            Console.Clear();

            MeteoService? meteoService = null;
            Mode? Modes = null;

            while (true)
            {
                meteoService = SelectMeteoservice();
                if (meteoService == null) Console.WriteLine("Invalid input. Please try again.");
                else break;
             
            }

            while (true)
            {
                Modes = SelectMode();
                if (Modes == null) Console.WriteLine("Invalid input. Please try again.");
                else break;

            }

            Console.WriteLine();
            Console.WriteLine($"You selected : {meteoService}");
            Console.WriteLine($"mode : {Modes}");
            Console.WriteLine("Press to continue");
            Console.ReadLine();


         

            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);


            builder.Services.AddHttpClient();


            switch (meteoService)
            {
                case MeteoService.OpenMeteo:
                    builder.Services.AddSingleton<IMeteoProvider, OpenMeteo>();
                    break;
                case MeteoService.OpenWeathermap:
                    builder.Services.AddSingleton<IMeteoProvider, OpenWeathermap>();
                    break;
                default: throw new NotImplementedException();
            }

            builder.Services.AddSingleton<ConsoleOutput>();

            builder.Services.AddSingleton(opt =>
            {
                var GETGeoInfoOM = "https://geocoding-api.open-meteo.com/v1/";
                var GETGeoInfoOWM = "https://api.openweathermap.org/geo/1.0/";

                var GETForecastOM = "https://api.open-meteo.com/v1/";
                var GETForecastOWM = "https://api.openweathermap.org/data/2.5/";

                return new MeteoApisBaseurls(GETGeoInfoOM, GETGeoInfoOWM, GETForecastOM, GETForecastOWM);
            });

            using IHost host = builder.Build();


            using var scope = host.Services.CreateScope();
            try
            {
                var consoleOutput = scope.ServiceProvider.GetRequiredService<ConsoleOutput>();
                await consoleOutput.MeteoCall(Modes);
            }
            catch(InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }




        static Mode? SelectMode()
        {
            Console.WriteLine("Select Mode (Geolocation : 1 , Forecast : 2 , all : 3)");

            var inputmeteoserv = Console.ReadLine();

            switch (inputmeteoserv?.ToLower())
            {
                case "1": return Mode.Geolocation;
                case "2": return Mode.Forecast;
                case "3": return Mode.All;
                default: return null;
            }

          
        }



        static MeteoService? SelectMeteoservice()
        {
            Console.WriteLine("Choose Meteo provider (OpenMeteo : OM , OpenweatherMap : OWM)");

            var inputmeteoserv = Console.ReadLine();

            switch (inputmeteoserv?.ToLower())
            {
                case "om": return MeteoService.OpenMeteo;
                case "owm": return MeteoService.OpenWeathermap;
                default: return null;
            }
        }


    }
}
