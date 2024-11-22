using Microsoft.AspNetCore.Mvc;
using OpenWeatherMapLogic;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.CompilerServices;
using System.Text;

namespace WeatherApi.ConsoleOption
{
    public abstract class ConsoleStartOptions
    {
        public bool ProgramLogicQuit = false;

       public  IConfiguration _config;

       public  IServiceLink _serviceLink {  get; init; }

      


        public async Task<bool> ConsoleStart()
        {


            int result = 0;

            while(true){ 
                Console.Write("Choose One : Console Weather OneShot(1), APi Weather server(2), Both(3), CheckAPIkey (4): ");

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
             

            if (MainOpenW.notValidApi)
            {
                Console.WriteLine("invalid APikey , please put a valid API key in appsettings.json");
                return true;
            }



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
                case 4: 
                    {
                        var apiValidation = new ApiValidation();

                        await apiValidation.Validationgetapi(_serviceLink);

                        return true;
                     };
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
