
using WeatherApi; 

namespace OpenWeatherMapLogic.JsonModelApi;

public class apiModeltoModelconversion : IDisposable
{

    ApiModels.Root? myDeserializedClass;
    CustomWeathermodel? custoWmodel;


    //public CustomWeathermodel conversion(string respose , string city) 
    //{
       

    //    // myDeserializedClass = JsonConvert.DeserializeObject<ApiModels.Root>(respose);

    //    // custoWmodel = new()
    //    //{
    //    //    CityNameModel = city,
    //    //    CnameWeathers = new CustomWeather[myDeserializedClass.list.Count]
    //    //};



    //    //for (int i = 0; i < myDeserializedClass.list.Count; i++)
    //    //{


    //    //    if (myDeserializedClass.list[i].dt_txt.Contains("6:00:00") || myDeserializedClass.list[i].dt_txt.Contains("12:00:00") || myDeserializedClass.list[i].dt_txt.Contains("18:00:00"))
    //    //    {

    //    //        custoWmodel.CnameWeathers[i] = new CustomWeather
    //    //        {
    //    //            Datatime = myDeserializedClass.list[i].dt_txt,
    //    //            Main = myDeserializedClass.list[i].weather[0].main,
    //    //            Description = myDeserializedClass.list[i].weather[0].description,

    //    //            Temperatures = new List<Temp>
    //    //            {
    //    //               new Temp
    //    //               {
    //    //                   Celsius = TempConversion.TempConversionKtoC(myDeserializedClass.list[i].main.temp),
    //    //                    Fahrenheit = TempConversion.TempConversionKtoF(myDeserializedClass.list[i].main.temp),
    //    //                    Kelvin = TempConversion.Tempcastingtoint(myDeserializedClass.list[i].main.temp)
    //    //               }


    //    //            }

    //    //        };

    //    //    }
           

    //    }

    //    custoWmodel.CnameWeathers = custoWmodel.CnameWeathers.Where(item => item is not null).ToArray();

    //    return custoWmodel;

   
    //}

    public void Dispose()
    {
        myDeserializedClass = null;
        custoWmodel = null;
    }
}
