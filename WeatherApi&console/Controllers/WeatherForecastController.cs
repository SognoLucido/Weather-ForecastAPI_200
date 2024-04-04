using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenWeatherMapLogic;
using OpenWeatherMapLogic.JsonModelApi;
using WeatherApi_console.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static OpenWeatherMapLogic.JsonModelApi.ApiModels;




namespace WeatherApi_console.Controllers;

[Route("api/[controller]")]
[ApiController]
public partial class WeatherForecastController : ControllerBase 
{

    string Cityname = "";

   

    [HttpGet("{City}")]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any , NoStore = false)]
    public async Task<ActionResult<CustomWeathermodel>>  Get(ValidationCityModel cityval)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Cityname = UpcaseCityname(cityval.CityValidation);


         var citymodel = await Fetchdbdata(Cityname);


        if (citymodel is not null )
        {        
            return Ok(citymodel);
        }
        else
        {

 
            //Response Caching Middleware only caches server responses that result in a 200(OK) status code.
            //Any other responses, including error pages, are ignored by the middleware.

            if (await Checkinvalidcity())
            {
                return Ok("Invalid City");
            }




            var Result = await _serviceLink.GetCityWeather(Latitude, Longitude, Cityname);

     

            if (Result is null)
            {
                return UnprocessableEntity();
            }
            else
            {
                if (Enablesqlitefetch)
                {
                    await Saveresulttodb(Result);
                }

                return Ok(Result);
            }


        }








    }











}
