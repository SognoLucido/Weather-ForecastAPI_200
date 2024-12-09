using HistoricalWeather.Sqlite;
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
       
        TimeOnly Time;
        bool? isEnabled;
        readonly DBservice dbcall;
        double[]? lat;
        double[]? lot;
        string? connstr;
        private static bool Onetime;
        private static TimeSpan Timerange  = TimeSpan.FromSeconds(10); // the histodb will fetch data every this , from Time(startdate)
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
            Time = new TimeOnly(23,26); //time to start fetching after startup



            var geolat = config.GetSection("Historicaldb:GeolocationsLat");
            var geolot = config.GetSection("Historicaldb:GeolocationsLot");

            connstr = config["Historicaldb:Sqlite"];
            if (connstr is null) isEnabled = false;


            try
            {


                lat = (double[]?)geolat
                    .AsEnumerable()
                    .Where(w => w.Value is not null)
                    .Select(s => double.Parse(s.Value))
                    .ToArray();

                lot = (double[]?)geolot
                   .AsEnumerable()
                   .Where(w => w.Value is not null)
                   .Select(s => double.Parse(s.Value))
                   .ToArray();


                if (lat is null || lot is null || (lat.Length != lot.Length)) throw new FormatException("invalid appsettings: Geolocalist");

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
            DateTime offsetcalc = DateTime.Now;
           

            if (offsetcalc.Hour <= Time.Hour && offsetcalc.Minute < Time.Minute)
            {

                offsetcalc =
                    new DateTime
                        (
                        offsetcalc.Year,
                        offsetcalc.Month,
                        offsetcalc.Day,
                        Time.Hour,
                        Time.Minute,
                        0
                       );


                OffsetFromSettingsTime = offsetcalc - DateTime.Now;


            }
            else 
            {
                offsetcalc =
                    new DateTime
                        (
                        offsetcalc.Year,
                        offsetcalc.Month,
                        offsetcalc.Day,
                        Time.Hour,
                        Time.Minute,
                        0
                       );
                offsetcalc.AddDays(1);

                OffsetFromSettingsTime = offsetcalc - DateTime.Now;
            }



            //offsetcalc.Hour = 

            while (!stoppingToken.IsCancellationRequested)
            {

                dbcall.Connstring = connstr ?? throw new NotImplementedException("Connstring missing");

                //await dbcall.Init();

                ////temp 
                //await dbcall.Fetchdata(lat, lot, meteoService);


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

            await dbcall.Fetchdata(lat, lot, meteoService);

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
