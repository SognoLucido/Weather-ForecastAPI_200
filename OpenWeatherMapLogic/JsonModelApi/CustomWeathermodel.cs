﻿namespace OpenWeatherMapLogic.JsonModelApi;

public class CustomWeathermodel
{
   public string CityNameModel { get; set; }
   
   public CustomWeather[] CnameWeathers { get; set; }
   
}


public class CustomWeather
{
    public string Datatime { get; set; } 

    public string Main { get; set; }

    public List<Temp> Temperatures { get; set; } 

}


public class Temp
{
    public int Celsius { get; set; }
    public int Fahrenheit { get; set; }
    public int Kelvin { get; set; }
}
