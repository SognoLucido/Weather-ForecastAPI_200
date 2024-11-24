using System.Text.Json.Serialization;

namespace Shared.MeteoData.Models
{
    public class GeoinfoOpenmeteoVariant
    {
        public string name { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int country_id { get; set; }
        public string admin1 { get; set; }
    }

    [JsonSerializable(typeof(List<GeoinfoOpenmeteoVariant>))]
    public partial class GeoinfoOpenmeteoVariantSGmodel : JsonSerializerContext { }



    /* 
     * // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Result
    {
        public int id { get; set; }
        public string name { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double elevation { get; set; }
        public string feature_code { get; set; }
        public string country_code { get; set; }
        public int admin1_id { get; set; }
        public int admin2_id { get; set; }
        public int admin3_id { get; set; }
        public string timezone { get; set; }
        public int population { get; set; }
        public List<string> postcodes { get; set; }
        public int country_id { get; set; }
        public string country { get; set; }
        public string admin1 { get; set; }
        public string admin2 { get; set; }
        public string admin3 { get; set; }
    }

    public class Root
    {
        public List<Result> results { get; set; }
        public double generationtime_ms { get; set; }
    }


     * */
}
