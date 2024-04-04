
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OpenWeatherMapLogic.JsonModelApi;
using System.Data;


namespace OpenWeatherMapLogic
{
    public class MainOpenW : IServiceLink 
    {
       private static string? Apikey { get; set; }
       static public bool notValidApi => String.IsNullOrEmpty(MainOpenW.Apikey) || (MainOpenW.Apikey.Length < 20);
       static public string? CityName { get; set; }
      

        private readonly HttpClient _httpClient;

        public MainOpenW(HttpClient? httpClient ,IConfiguration config)
        {
            if (httpClient is null) _httpClient = new HttpClient();
            else _httpClient = httpClient;

             Apikey = config["OpenweathermapApi:ApiKey"];
           

        }



        public async Task<List<ApiModels.City>?> GetCityInformation(string city)
        {

            string url = @$"https://api.openweathermap.org/geo/1.0/direct?q={city}&appid={Apikey}";

            string response = string.Empty;

            try
            {
               // HttpResponseMessage responsev2 = await _httpClient.GetAsync(url);

                 response = await _httpClient.GetStringAsync(url);


            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }


            //  response = await _httpClient.GetStringAsync("https://api.ipify.org");

            if (String.IsNullOrEmpty(response))
            {
                await Console.Out.WriteLineAsync("something went wrong : response string is empty");
                return null;
            }
            else
            {
               //var x = JsonConvert.DeserializeObject<List<ApiModels.City>>(response);

                return JsonConvert.DeserializeObject<List<ApiModels.City>>(response);
               
            }

        }



        public async Task<CustomWeathermodel?> GetCityWeather(double? Latitude, double? Longitude , string City)
        {
            string url = @$"https://api.openweathermap.org/data/2.5/forecast?lat={Latitude}&lon={Longitude}&appid={Apikey}";

            string response = string.Empty;

            try
            {
                // HttpResponseMessage response = await _httpClient.GetAsync("https://api.ipify.org");
                response = await _httpClient.GetStringAsync(url);


            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return null;
            }


            //  response = await _httpClient.GetStringAsync("https://api.ipify.org");

                using (apiModeltoModelconversion convMtoM = new apiModeltoModelconversion())
                {                  
                    CustomWeathermodel custom = convMtoM.conversion(response, City);   
                
                    custom.Datafrom = "Api";
                    return custom;

                }
             
        }


        public async Task<bool> QuickvalidCheck() //TODO
        {
            string url = $@"https://api.openweathermap.org/data/2.5/forecast?appid={Apikey}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);



            return false;
        }




    }
}
