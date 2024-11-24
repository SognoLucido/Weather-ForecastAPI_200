using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.MeteoData.Models.Dto
{


    public class GeoinfoplusProvider
    {
        public string MeteoProvider { get; set; } 

        public List<GeoinfoModel>? Geoinfo { get; set; }
    }

    [JsonSerializable(typeof(Int32?))]
    [JsonSerializable(typeof(GeoinfoplusProvider))]
    public partial class GeoinfoResposte : JsonSerializerContext { }


}
