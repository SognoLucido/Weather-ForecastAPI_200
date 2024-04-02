using Microsoft.AspNetCore.Mvc;
using OpenWeatherMapLogic;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.CompilerServices;
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


            int result = 0;

            while(true){ 
                Console.Write("Choose One : Console Weather OneShot(1), APi Weather server(2), Both(3) : ");

              string input = Console.ReadLine();

                if (String.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Invalid Input");
                    continue;
                }
                else
                {
                    if (int.TryParse(input, out result))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid Input");
                        continue;
                    }
                }
  
            }





            // Console.Write("Choose One : Console Weather OneShot(1), APi Weather server(2), Both(3) : ");

            //if( int.TryParse(Console.ReadLine(), out int result)) { }
            //    else 
            //     {
            //     Console.WriteLine("invalid Input");
            //     return true;
            //     }


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
                default: 
                    {
                        Console.WriteLine("Invalid Choice");
                        return true;
                    };
            };

           // Console.WriteLine(_config["OpenweathermapApi:ApiKey"]);
        }

        public virtual async Task ConsoleLogic() { }
        
   
        



    }






}
