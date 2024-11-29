


namespace WeatherApi;

public static class  TempConversion
{


    //public static int Tempcastingtoint(double temp) => (int)temp;


    public static double TempKtoC(this double temp) => temp - 273.15;

    public static int TempConversionKtoC(double temp) => (int)(temp - 273.15);

    //public static int TempConversionKtoF(double temp) => (int)((temp * 9 / 5) - 459.67);


}
