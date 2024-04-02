using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenWeatherMapLogic;
using OpenWeatherMapLogic.JsonModelApi;
using System.ComponentModel.DataAnnotations;
using WeatherApi_console.ConsoleOption;
using WeatherApi_console.Model;




namespace WeatherApi_console.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherForecastController : ControllerBase 
    {
        private readonly IServiceLink _serviceLink;

        public WeatherForecastController(IServiceLink serviceLink)
        {
            _serviceLink = serviceLink;
        }





        [HttpGet("{City}")]
        [ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Any , NoStore = false)]
        public async Task<ActionResult<CustomWeathermodel>>  Get(ValidationCityModel cityval)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

             MainOpenW.CityName = cityval.CityValidation;

            

            List<ApiModels.City> pep =  await _serviceLink.GetCityInformation();

            Double? Latitude;
            Double? Longitude;

            Latitude = pep[0].Lat;
            Longitude = pep[0].Lon;


            // Your action code
            return Ok(await _serviceLink.GetCityWeather(Latitude, Longitude));
        }











    }
}
