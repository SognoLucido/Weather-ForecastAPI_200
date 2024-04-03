using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenWeatherMapLogic;
using OpenWeatherMapLogic.JsonModelApi;
using WeatherApi_console.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;




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
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any , NoStore = false)]
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

            //Response Caching Middleware only caches server responses that result in a 200(OK) status code.
            //Any other responses, including error pages, are ignored by the middleware.

            if (pep.Count == 0)
            {
                return Ok("Invalid City");
            }

            Latitude = pep[0].Lat;
            Longitude = pep[0].Lon;


            var Result = await _serviceLink.GetCityWeather(Latitude, Longitude);

            if (Result is null)
            {
                return UnprocessableEntity();
            }
            else
            {
                return Ok(Result);
            }


           
            
        }











    }
}
