using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Shared.MeteoData
{
    public class Testurls
    {
        public string? GETGeocodingOpenApi { get; set; }

        public Testurls(string _GETGeocodingOpenApi) 
        {
            GETGeocodingOpenApi = _GETGeocodingOpenApi;
        }


        //public void pep(string ut)
        //{

        //}
    }
}
