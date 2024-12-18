﻿using HistoricalWeather.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.IMeteo;
using Shared.MeteoData.Models;
using System;
using System.Timers;


namespace HistoricalWeather
{
    public class Main : BackgroundService
    {
        private static System.Timers.Timer aTimer;

        TimeOnly TimeToreach = new TimeOnly(00, 00); //time to start fetching after startup . UTC only ;
        bool? isEnabled;
        readonly DBservice dbcall;
        double[]? lat;
        double[]? lon;
        string? connstr;
        private static bool Onetime;
        private static TimeSpan Timerange  = TimeSpan.FromDays(1); // the histodb will fetch data every this , from Time(startdate)
        private TimeSpan OffsetFromSettingsTime;
        private MeteoService meteoService;

        public Main
            (
            IConfiguration config,
            DBservice _dbcall
            )
        {
            dbcall = _dbcall;
            isEnabled = config["Historicaldb:Enabled"]?.ConvertstrtoB() ?? false;

            switch (config["Historicaldb:MeteoProvider"])
            {
                case "OM": meteoService = MeteoService.OpenMeteo;  break;
                case "OWM": meteoService = MeteoService.OpenWeathermap; break;
                    default: 
                    { 
                        isEnabled = false;
                        return; 
                    }
            }

            // if (!TimeOnly.TryParse(config["Historicaldb:TimeToFetch"], out Time)) { isEnabled = false; }



            var geolat = config.GetSection("Historicaldb:GeolocationsLat");
            var geolot = config.GetSection("Historicaldb:GeolocationsLon");

            connstr = config["Historicaldb:Sqlite"];
            if (connstr is null) isEnabled = false;


            try
            {


                lat = (double[]?)geolat
                    .AsEnumerable()
                    .Where(w => w.Value is not null)
                    .Select(s => double.Parse(s.Value))
                    .ToArray();

                lon = (double[]?)geolot
                   .AsEnumerable()
                   .Where(w => w.Value is not null)
                   .Select(s => double.Parse(s.Value))
                   .ToArray();


                if (lat is null || lon is null || (lat.Length != lon.Length)) throw new FormatException("invalid appsettings: Geolocalist");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                isEnabled = false;
            }

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (isEnabled is null || isEnabled == false)
            {
                Console.WriteLine("failed to initialize the Bg service; it will be disabled");
                return;
            }

            dbcall.Connstring = connstr;

            DateTime now = DateTime.UtcNow;
           
            if (now.Hour <= TimeToreach.Hour && now.Minute < TimeToreach.Minute)
            {

                now =
                    new DateTime
                        (
                        now.Year,
                        now.Month,
                        now.Day,
                        TimeToreach.Hour,
                        TimeToreach.Minute,
                        0
                       );


                OffsetFromSettingsTime = now - DateTime.UtcNow;


            }
            else 
            {

              


               DateTime pep =
                    new DateTime
                        (
                        now.Year,
                        now.Month,
                        now.Day,
                        TimeToreach.Hour,
                        TimeToreach.Minute,
                        0  
                       );

                pep = pep.AddDays(1);

                OffsetFromSettingsTime = pep - now;

         
            }



            

            while (!stoppingToken.IsCancellationRequested)
            {

                dbcall.Connstring = connstr ?? throw new NotImplementedException("Connstring missing");

                await dbcall.Init();

                ////temp 
                //await dbcall.Fetchdata(lat, lon, meteoService);


                TimerSetup(OffsetFromSettingsTime);
                await Task.Delay(-1, stoppingToken);

            }

            Tdispose();

        }




        private async  void OnTimedEvent(Object? source, ElapsedEventArgs e)
         {

            if (!Onetime)
            {
                Tdispose();
                TimerSetup(Timerange);
            }

            await dbcall.Fetchdata(lat, lon, meteoService);

            Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}", e.SignalTime);

        }



        private  void TimerSetup(TimeSpan pep)
        {
            aTimer = new System.Timers.Timer(pep);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }



        private static void Tdispose()
        {
            aTimer.Stop();
            aTimer.Dispose();
            Onetime = true;

        }


      






    }
}
