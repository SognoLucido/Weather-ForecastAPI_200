using OpenWeatherMapLogic;

namespace WeatherApi_console.ConsoleOption
{
    public  class ApiValidation : ConsoleStartOptions 
    {


        //public ApiValidation(IServiceLink _servicelink)
        //{

        //}

        public async Task Validationgetapi(IServiceLink service)
        {


            if (MainOpenW.notValidApi)
            {
                Console.WriteLine("Invalid Api key , invalid APikey , please put a valid API key in appsettings.json");
                return;
            }
            else if(await service.QuickvalidCheck()) 
            {
                Console.WriteLine($"{service.GetApishow} is valid ");
            }
            else
            {
                Console.WriteLine($"{service.GetApishow} invalid api key ");
            }
            

        }


    }
}
