using System.Text.Json.Serialization;

namespace Shared.MeteoData.Models
{

    public class GeoinfoOpenmeteoVariant
    {

        public List<Result> results { get; set; }
    }

    public record Result
    (
         string name ,
        double latitude ,
         double longitude ,
         string country_code,
         string admin1 
    );

    [JsonSerializable(typeof(GeoinfoOpenmeteoVariant))]
    public partial class GeoinfoOpenmeteoVariantSGmodel : JsonSerializerContext { }



}
