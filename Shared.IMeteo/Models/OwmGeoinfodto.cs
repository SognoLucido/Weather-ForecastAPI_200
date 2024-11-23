using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.MeteoData.Models
{
    public class OwmGeoinfodto
    {
        public string name { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public string country { get; set; }
        public string state { get; set; }
    }
}
