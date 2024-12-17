using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace WeatherApi.Integration.test
{
    public class ForecastMockAPI : IDisposable
    {

        const string GeoinfoOpenApiGenoa = @"{""results"":[{""id"":3176219,""name"":""Genoa"",""latitude"":44.40478,""longitude"":8.94439,""elevation"":19.0,""feature_code"":""PPLA"",""country_code"":""IT"",""admin1_id"":3174725,""admin3_id"":6542282,""timezone"":""Europe/Rome"",""population"":580223,""country_id"":3175395,""country"":""Italy"",""admin1"":""Liguria"",""admin3"":""Genova""},{""id"":4893601,""name"":""Genoa"",""latitude"":42.09725,""longitude"":-88.69287,""elevation"":254.0,""feature_code"":""PPL"",""country_code"":""US"",""admin1_id"":4896861,""admin2_id"":4889554,""admin3_id"":4893605,""timezone"":""America/Chicago"",""population"":5196,""postcodes"":[""60135""],""country_id"":6252001,""country"":""United States"",""admin1"":""Illinois"",""admin2"":""DeKalb"",""admin3"":""Genoa Township""},{""id"":4833348,""name"":""Genoa"",""latitude"":41.51811,""longitude"":-83.35909,""elevation"":190.0,""feature_code"":""PPL"",""country_code"":""US"",""admin1_id"":5165418,""admin2_id"":5165803,""admin3_id"":5150469,""timezone"":""America/New_York"",""population"":2306,""postcodes"":[""43430""],""country_id"":6252001,""country"":""United States"",""admin1"":""Ohio"",""admin2"":""Ottawa"",""admin3"":""Clay Township""}],""generationtime_ms"":0.8029938}";


        //private readonly Testurls testurls;

        //public ForecastMockAPI(Testurls test) 
        //{
        //    testurls = test;
        //}    

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



        public void CreateMockGeolocation(string Cityname)
        {
            string body = string.Empty;

            body = Cityname.ToLower() switch
            {
                "genoa" => GeoinfoOpenApiGenoa,
                _ => throw new NotImplementedException(),
            };

            Apimock?.Given(
               Request.Create()
               .WithPath("/search") // Path without query parameters
               .WithParam("name", Cityname)
               //.WithParam("count", "3")
               .WithParam("format", "json")
               .UsingGet())
               .RespondWith(Response.Create()
               .WithHeader("Content-Type", "application/json")
               .WithBody(body)
               .WithStatusCode(200)
               )
               ;
        }











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
