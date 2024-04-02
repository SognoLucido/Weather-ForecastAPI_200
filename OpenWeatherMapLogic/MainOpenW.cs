
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OpenWeatherMapLogic.JsonModelApi;
using System;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace OpenWeatherMapLogic
{
    public class MainOpenW : IServiceLink 
    {
       static string? Apikey { get; set; }
       static public bool ValidApi => String.IsNullOrEmpty(Apikey);
       static public string? CityName { get; set; }
      

        private readonly HttpClient _httpClient;

        public MainOpenW(HttpClient? httpClient ,IConfiguration config)
        {
            if (httpClient is null) _httpClient = new HttpClient();
            else _httpClient = httpClient;

             Apikey = config["OpenweathermapApi:ApiKey"];
           

        }



        //private readonly IHttpClientFactory _httpClientFactory;

        //public MainOpenW(IServiceProvider.Getsevice httpClientFactory) =>
        //    _httpClientFactory = httpClientFactory;


        //public async Task<string> GetStringAsync(string url)
        //{
        //    try
        //    {
        //        HttpResponseMessage response = await _httpClient.GetAsync("https://api.ipify.org");
        //        response.EnsureSuccessStatusCode(); // Ensure success status code
        //        return await response.Content.ReadAsStringAsync();
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        // Handle request exceptions
        //        Console.WriteLine($"request failed: {ex.Message}");
        //        return string.Empty;
        //    }
        //}


        public async Task<List<ApiModels.City>?> GetCityInformation()
        {


            string url = @$"https://api.openweathermap.org/geo/1.0/direct?q={CityName}&appid={Apikey}";


            string response = string.Empty;

            try
            {
               // HttpResponseMessage response = await _httpClient.GetAsync("https://api.ipify.org");
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
               var x = JsonConvert.DeserializeObject<List<ApiModels.City>>(response);

                return JsonConvert.DeserializeObject<List<ApiModels.City>>(response);
               
            }

        }


        public async Task<CustomWeathermodel> GetCityWeather(double? Latitude, double? Longitude)
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
            }


            //  response = await _httpClient.GetStringAsync("https://api.ipify.org");

            if (String.IsNullOrEmpty(response))
            {
                await Console.Out.WriteLineAsync("something went wrong : response string is empty");
                return null;
            }
            else
            {


                using (apiModeltoModelconversion convMtoM = new apiModeltoModelconversion())
                {                  
                    CustomWeathermodel custom = convMtoM.conversion(response);           
                     return custom;

                }



            }
          
        }


       




    }
}
