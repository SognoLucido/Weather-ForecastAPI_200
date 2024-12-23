
using Shared.MeteoData.Models;
using System.Text.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace WeatherApi.Integration.test
{
    public class ForecastMockAPI : IDisposable
    {

        const string GeoinfoOpenApiGenoa = @"{""results"":[{""id"":3176219,""name"":""Genoa"",""latitude"":44.40478,""longitude"":8.94439,""elevation"":19.0,""feature_code"":""PPLA"",""country_code"":""IT"",""admin1_id"":3174725,""admin3_id"":6542282,""timezone"":""Europe/Rome"",""population"":580223,""country_id"":3175395,""country"":""Italy"",""admin1"":""Liguria"",""admin3"":""Genova""},{""id"":4893601,""name"":""Genoa"",""latitude"":42.09725,""longitude"":-88.69287,""elevation"":254.0,""feature_code"":""PPL"",""country_code"":""US"",""admin1_id"":4896861,""admin2_id"":4889554,""admin3_id"":4893605,""timezone"":""America/Chicago"",""population"":5196,""postcodes"":[""60135""],""country_id"":6252001,""country"":""United States"",""admin1"":""Illinois"",""admin2"":""DeKalb"",""admin3"":""Genoa Township""},{""id"":4833348,""name"":""Genoa"",""latitude"":41.51811,""longitude"":-83.35909,""elevation"":190.0,""feature_code"":""PPL"",""country_code"":""US"",""admin1_id"":5165418,""admin2_id"":5165803,""admin3_id"":5150469,""timezone"":""America/New_York"",""population"":2306,""postcodes"":[""43430""],""country_id"":6252001,""country"":""United States"",""admin1"":""Ohio"",""admin2"":""Ottawa"",""admin3"":""Clay Township""}],""generationtime_ms"":0.8029938}";
        const string GeoInfoOWMApiGenoa = @"[{""name"":""Genoa"",""local_names"":{""zh"":""热那亚"",""ar"":""جنوة"",""pt"":""Génova"",""kn"":""ಜೆನೋವ"",""el"":""Γένοβα"",""en"":""Genoa"",""it"":""Genova"",""pl"":""Genua"",""mi"":""Tenoa"",""ru"":""Генуя"",""es"":""Génova"",""eo"":""Ĝenovo"",""la"":""Genua"",""oc"":""Gènoa"",""sl"":""Genova"",""ca"":""Gènova"",""sv"":""Genua"",""cs"":""Janov"",""sk"":""Janov"",""fa"":""جنوا"",""de"":""Genua"",""gd"":""Genua"",""be"":""Генуя"",""nl"":""Genua"",""uk"":""Генуя"",""hr"":""Genova"",""gl"":""Xénova"",""fr"":""Gênes""},""lat"":44.40726,""lon"":8.9338624,""country"":""IT"",""state"":""Liguria""},{""name"":""Town of Genoa"",""local_names"":{""en"":""Town of Genoa""},""lat"":42.667874,""lon"":-76.5357634,""country"":""US"",""state"":""New York""},{""name"":""Genoa"",""lat"":41.5181064,""lon"":-83.359094,""country"":""US"",""state"":""Ohio""}]";
        const string ForecastInfoOM = @"{""latitude"": 44.4,""longitude"": 8.940001,""generationtime_ms"": 0.07998943328857422,""utc_offset_seconds"": 0,""timezone"": ""GMT"",""timezone_abbreviation"": ""GMT"",""elevation"": 3,""hourly_units"": {""time"": ""iso8601"",""temperature_2m"": ""°C"",""weather_code"": ""wmo code""},""hourly"": {""time"": [""2024-12-20T00:00"",""2024-12-20T01:00"",""2024-12-20T02:00"",""2024-12-20T03:00"",""2024-12-20T04:00"",""2024-12-20T05:00"",""2024-12-20T06:00"",""2024-12-20T07:00"",""2024-12-20T08:00"",""2024-12-20T09:00"",""2024-12-20T10:00"",""2024-12-20T11:00"",""2024-12-20T12:00"",""2024-12-20T13:00"",""2024-12-20T14:00"",""2024-12-20T15:00"",""2024-12-20T16:00"",""2024-12-20T17:00"",""2024-12-20T18:00"",""2024-12-20T19:00"",""2024-12-20T20:00"",""2024-12-20T21:00"",""2024-12-20T22:00"",""2024-12-20T23:00""],""temperature_2m"": [9.6,8.9,8.5,9.2,9.4,9.2,9.5,8.9,9.1,10.2,11,11.7,12.5,13.1,13,12.2,10.7,9.7,9.3,9.2,9.5,9.3,9,8.9],""weather_code"": [3,61,3,3,3,3,3,3,3,3,3,3,1,0,0,0,0,0,0,0,0,0,0,0]}}";
        const string ForecastInfoOWM = @"{""cod"": ""200"",""message"": 0,""cnt"": 40,""list"": [{""dt"": 1734998400,""main"": {""temp"": 7.12,""feels_like"": 3.48,""temp_min"": 7.12,""temp_max"": 7.12,""pressure"": 1011,""sea_level"": 1011,""grnd_level"": 976,""humidity"": 50,""temp_kf"": 0},""weather"": [{""id"": 800,""main"": ""Clear"",""description"": ""clear sky"",""icon"": ""01n""}],""clouds"": {""all"": 0},""wind"": {""speed"": 6.3,""deg"": 331,""gust"": 13.28},""visibility"": 10000,""pop"": 0,""sys"": {""pod"": ""n""},""dt_txt"": ""2024-12-24 00:00:00""},{""dt"": 1735009200,""main"": {""temp"": 5.89,""feels_like"": 2.21,""temp_min"": 5.89,""temp_max"": 5.89,""pressure"": 1013,""sea_level"": 1013,""grnd_level"": 977,""humidity"": 53,""temp_kf"": 0},""weather"": [{""id"": 800,""main"": ""Clear"",""description"": ""clear sky"",""icon"": ""01n""}],""clouds"": {""all"": 0},""wind"": {""speed"": 5.55,""deg"": 333,""gust"": 11.41},""visibility"": 10000,""pop"": 0,""sys"": {""pod"": ""n""},""dt_txt"": ""2024-12-24 03:00:00""},{""dt"": 1735020000,""main"": {""temp"": 4.65,""feels_like"": 0.58,""temp_min"": 4.65,""temp_max"": 4.65,""pressure"": 1015,""sea_level"": 1015,""grnd_level"": 979,""humidity"": 56,""temp_kf"": 0},""weather"": [{""id"": 800,""main"": ""Clear"",""description"": ""clear sky"",""icon"": ""01n""}],""clouds"": {""all"": 0},""wind"": {""speed"": 5.7,""deg"": 345,""gust"": 8.87},""visibility"": 10000,""pop"": 0,""sys"": {""pod"": ""n""},""dt_txt"": ""2024-12-24 06:00:00""},{""dt"": 1735030800,""main"": {""temp"": 6.34,""feels_like"": 2.86,""temp_min"": 6.34,""temp_max"": 6.34,""pressure"": 1017,""sea_level"": 1017,""grnd_level"": 981,""humidity"": 51,""temp_kf"": 0},""weather"": [{""id"": 800,""main"": ""Clear"",""description"": ""clear sky"",""icon"": ""01d""}],""clouds"": {""all"": 0},""wind"": {""speed"": 5.34,""deg"": 354,""gust"": 7.79},""visibility"": 10000,""pop"": 0,""sys"": {""pod"": ""d""},""dt_txt"": ""2024-12-24 09:00:00""},{""dt"": 1735041600,""main"": {""temp"": 8.72,""feels_like"": 5.73,""temp_min"": 8.72,""temp_max"": 8.72,""pressure"": 1018,""sea_level"": 1018,""grnd_level"": 983,""humidity"": 43,""temp_kf"": 0},""weather"": [{""id"": 800,""main"": ""Clear"",""description"": ""clear sky"",""icon"": ""01d""}],""clouds"": {""all"": 0},""wind"": {""speed"": 5.68,""deg"": 338,""gust"": 7.01},""visibility"": 10000,""pop"": 0,""sys"": {""pod"": ""d""},""dt_txt"": ""2024-12-24 12:00:00""},{""dt"": 1735052400,""main"": {""temp"": 7.34,""feels_like"": 4,""temp_min"": 7.34,""temp_max"": 7.34,""pressure"": 1020,""sea_level"": 1020,""grnd_level"": 984,""humidity"": 53,""temp_kf"": 0},""weather"": [{""id"": 800,""main"": ""Clear"",""description"": ""clear sky"",""icon"": ""01d""}],""clouds"": {""all"": 0},""wind"": {""speed"": 5.65,""deg"": 339,""gust"": 7.2},""visibility"": 10000,""pop"": 0,""sys"": {""pod"": ""d""},""dt_txt"": ""2024-12-24 15:00:00""},{""dt"": 1735063200,""main"": {""temp"": 5.01,""feels_like"": 1.14,""temp_min"": 5.01,""temp_max"": 5.01,""pressure"": 1022,""sea_level"": 1022,""grnd_level"": 985,""humidity"": 58,""temp_kf"": 0},""weather"": [{""id"": 800,""main"": ""Clear"",""description"": ""clear sky"",""icon"": ""01n""}],""clouds"": {""all"": 0},""wind"": {""speed"": 5.46,""deg"": 356,""gust"": 6.54},""visibility"": 10000,""pop"": 0,""sys"": {""pod"": ""n""},""dt_txt"": ""2024-12-24 18:00:00""},{""dt"": 1735074000,""main"": {""temp"": 5.46,""feels_like"": 1.9,""temp_min"": 5.46,""temp_max"": 5.46,""pressure"": 1023,""sea_level"": 1023,""grnd_level"": 987,""humidity"": 53,""temp_kf"": 0},""weather"": [{""id"": 800,""main"": ""Clear"",""description"": ""clear sky"",""icon"": ""01n""}],""clouds"": {""all"": 0},""wind"": {""speed"": 5.03,""deg"": 355,""gust"": 5.99},""visibility"": 10000,""pop"": 0,""sys"": {""pod"": ""n""},""dt_txt"": ""2024-12-24 21:00:00""}],""city"": {""id"": 3176219,""name"": ""Genoa"",""coord"": {""lat"": 44.4,""lon"": 8.93},""country"": ""IT"",""population"": 601951,""timezone"": 3600,""sunrise"": 1734937099,""sunset"": 1734968915}}";
        public static WireMockServer? Apimock { get; set; }

        public string BaseUrl => Apimock.Urls[0];

        public void Dispose()
        {
            Apimock.Stop();
        }

        public void InitWireMock()
        {
            Apimock = WireMockServer.Start();
        }
        public void CreateMockGeolocation(string Cityname, MeteoService meteoServ)
        {
            string body = string.Empty;

            

            switch (Cityname.ToLower())
            {
                case "genoa":
                    {
                        if (meteoServ == MeteoService.OpenMeteo) body = GeoinfoOpenApiGenoa;
                        else if (meteoServ == MeteoService.OpenWeathermap) body = GeoInfoOWMApiGenoa;
                    };
                    break;
                default: throw new NotImplementedException("city not configurated in wiremock");
            }


            Apimock?.Given(
               Request.Create()
               .WithPath(SetMeteoGeoPath(meteoServ)) 
               .WithParam(SetMeteoGeoCityquery(meteoServ), Cityname)
               .UsingGet())
               .RespondWith(Response.Create()
               .WithHeader("Content-Type", "application/json")
               .WithBody(body)
               .WithStatusCode(200)
               )
               ;
        }


        public void CreateMockForecast(double lat ,double lon, MeteoService meteoServ) 
        {

            string body = string.Empty;

            lat = Math.Truncate(lat * 100) / 100;
            lon = Math.Truncate(lon * 100) / 100;

            var coor = (lat, lon);

            //switch (coor) 
            //{
            //    case { lat :44.40, lon: 8.94 }: 
            //        {
            //            if (meteoServ == MeteoService.OpenMeteo) /*body = GeoinfoOpenApiGenoa*/;
            //            else if (meteoServ == MeteoService.OpenWeathermap) /*body = GeoInfoOWMApiGenoa*/;
            //        } break;    
            //        default : throw new NotImplementedException("coordinates not found ,wiremocksetup");
            //}


            //ForecastGETommodel DesClass = JsonSerializer.Deserialize<ForecastGETommodel>(ForecastInfoOM)!;

            //var BaseDate = DateTime.UtcNow;
            //BaseDate = BaseDate.AddDays(1);

            //for(int i = 0; i<  DesClass.hourly.time.Count; i++)
            //{
            //    var dateparse = DateTime.Parse(DesClass.hourly.time[i]);


            //    DesClass.hourly.time[i] = new DateTime
            //        (
            //          dateparse.Year,
            //          dateparse.Month,
            //          BaseDate.Day,
            //          dateparse.Hour,
            //          dateparse.Minute,
            //          dateparse.Second
            //        ).ToString("yyyy-MM-ddTHH:mm");

            //}

          //string wtf =  JsonSerializer.Serialize(DesClass);


            Apimock?.Given(
              Request.Create()
              .WithPath("/forecast")
              .WithParam(SetForecastqueryPar(meteoServ)[0])
              .WithParam(SetForecastqueryPar(meteoServ)[1])
              .UsingGet())
              .RespondWith(Response.Create()
              .WithHeader("Content-Type", "application/json")
              .WithBody(Bodymessage(meteoServ))
              .WithStatusCode(200)
              )
              ;



        }


        private string Bodymessage(MeteoService meteo)
        {
            if (meteo == MeteoService.OpenMeteo)
            {

                ForecastGETommodel DesClass = JsonSerializer.Deserialize<ForecastGETommodel>(ForecastInfoOM)!;

                var BaseDate = DateTime.UtcNow;
                BaseDate = BaseDate.AddDays(1);

                for (int i = 0; i < DesClass.hourly.time.Count; i++)
                {
                    var dateparse = DateTime.Parse(DesClass.hourly.time[i]);


                    DesClass.hourly.time[i] = new DateTime
                        (
                          dateparse.Year,
                          dateparse.Month,
                          BaseDate.Day,
                          dateparse.Hour,
                          dateparse.Minute,
                          dateparse.Second
                        ).ToString("yyyy-MM-ddTHH:mm");

                }


                return JsonSerializer.Serialize(DesClass);
            }
            else if (meteo == MeteoService.OpenWeathermap)
            {
                return ForecastInfoOWM;
            }
            else throw new NotImplementedException();
        }

        private string[] SetForecastqueryPar(MeteoService meteo)
        {
           var queryPar = new string[2];

            if (meteo == MeteoService.OpenMeteo)
            {
                queryPar[0] = "latitude";
                queryPar[1] = "longitude";
            }
            else if (meteo == MeteoService.OpenWeathermap)
            {
                queryPar[0] = "lat";
                queryPar[1] = "lon";
            }


            return queryPar;
        }


        private string SetMeteoGeoPath(MeteoService meteo) 
        {
            if (meteo == MeteoService.OpenMeteo)
            {
                return "/search";
            }
            else if (meteo == MeteoService.OpenWeathermap)
            {
                return "/direct";
            }

            throw new ArgumentException("Invalid MeteoService value");
        }

        private string SetMeteoGeoCityquery(MeteoService meteo)
        {
            if (meteo == MeteoService.OpenMeteo)
            {
                return "name";
            }
            else if (meteo == MeteoService.OpenWeathermap)
            {
                return "q";
            }

            throw new ArgumentException("Invalid MeteoService value");
        }





        ////OVERKILL NOT USED
        //private Dictionary<string,string> MeteoUrlParam(MeteoService meteo , string city)  // [0] = path 
        //{
        //    Dictionary<string, string> test = [] ;

        //    switch (meteo)
        //    {
        //        case MeteoService.OpenMeteo: 
        //            {
        //                test = new()
        //                {
        //                    {"path", "/search" },
        //                    {"name", city },
        //                    {"format", "json"}
        //                };
        //            }break;
        //        case MeteoService.OpenWeathermap: 
        //            {
        //                test = new()
        //                {
        //                    {"path", "/direct" },
        //                    {"q", city },

        //                };
        //            }break;
        //    }



        //    return test;
        //}


        //public void WeatherMocApiSetup()
        //{
        //    Apimock?.Given(
        //        Request.Create()
        //        .WithPath("/search") // Path without query parameters
        //        .WithParam("name", "Genoa")
        //        .WithParam("count", "3")
        //        .WithParam("format", "json")
        //        .UsingGet())
        //        .RespondWith(Response.Create()
        //        .WithHeader("Content-Type", "application/json")
        //        .WithBody("LOCK YES")
        //        .WithStatusCode(200)
        //        )
        //        ;

        //}





    }





}





