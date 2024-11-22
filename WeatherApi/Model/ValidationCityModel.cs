using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WeatherApi.Model
{
    public class ValidationCityModel
    {
        [FromRoute(Name = "City")]
        [Required(ErrorMessage = "City is required.")]
        [Length(3, 20, ErrorMessage = " City Minimumlength 3 and Max 20")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Only alphabetic characters are permitted")]
        public string CityValidation { get; set; }

    }

}
