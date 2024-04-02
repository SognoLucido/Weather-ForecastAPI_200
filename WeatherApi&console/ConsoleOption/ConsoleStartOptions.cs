using Microsoft.AspNetCore.Mvc;
using OpenWeatherMapLogic;
using System.Diagnostics.Eventing.Reader;
using System.Text;

namespace WeatherApi_console.ConsoleOption
{
    public abstract class ConsoleStartOptions
    {
        public bool ProgramLogicQuit = false;

        public IServiceLink? _serviceLink;

       public string? Keyz {  get; init; }

       // public WeatherForecast? WeatherConvertion = new();


        public async Task<bool> ConsoleStart()
        {

            Console.Write("Choose One : Console Weather OneShot(1), APi Weather server(2), Both(3) : ");

           if( int.TryParse(Console.ReadLine(), out int result)) { }
               else { return true; }


            switch (result)
            {
                case 1:
                    {
                        await ConsoleLogic();
                        return true;                 
                   };
                case 2: return false;
                case 3:
                    {
                       await ConsoleLogic();
                        return ProgramLogicQuit;
                    }
                default: return true;
            };

           // Console.WriteLine(_config["OpenweathermapApi:ApiKey"]);
        }

        public virtual async Task ConsoleLogic() { }
        
   
        



    }






}
