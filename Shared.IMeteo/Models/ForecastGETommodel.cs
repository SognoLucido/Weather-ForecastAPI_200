﻿using Shared.MeteoData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.MeteoData.Models
{
    public class ForecastGETommodel
    {

        public double latitude { get; set; }
        public double longitude { get; set; }
        //public double generationtime_ms { get; set; }
        //public int utc_offset_seconds { get; set; }
        //public string timezone { get; set; }
        //public string timezone_abbreviation { get; set; }
        //public double elevation { get; set; }
        //public HourlyUnits hourly_units { get; set; }
        public Hourly hourly { get; set; }

    }


    public class Hourly
    {
        public List<string> time { get; set; }
        public List<double> temperature_2m { get; set; }
        public List<int> weather_code { get; set; }
    }

    //public class HourlyUnits
    //{
    //    public string time { get; set; }
    //    public string temperature_2m { get; set; }
    //    public string weather_code { get; set; }
    //}

}


[JsonSerializable(typeof(ForecastGETommodel))]
public partial class ForecastomVariantSGmodel : JsonSerializerContext { }
