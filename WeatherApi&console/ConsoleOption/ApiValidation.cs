using OpenWeatherMapLogic;

namespace WeatherApi_console.ConsoleOption
{
    public  class ApiValidation : ConsoleStartOptions
    {
        


        public async Task akfwja()
        {
            if (MainOpenW.notValidApi)
            {
                Console.WriteLine("Api key not valid");
                return;
            }
            else if(await _serviceLink.QuickvalidCheck()) //TODO
            {

            }
            

        }


    }
}
