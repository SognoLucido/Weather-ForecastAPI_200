using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.MeteoData.Models
{
   
    public class GeoinfoModel 
    {
        //public string Name { get; set; }
        //public string Country_Code { get; set; }

        //public string? State { get; set; }

        public string name { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public string country { get; set; }
        public string? state { get; set; }
    }
}
