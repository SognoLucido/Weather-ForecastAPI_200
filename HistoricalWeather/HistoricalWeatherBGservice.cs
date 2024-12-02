using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;


namespace HistoricalWeather
{
    public class HistoricalWeatherBGservice : BackgroundService
    {

        TimeOnly Time;
        bool? isEnabled;
        readonly DBservice dbcall;
        double[]? lat;
        double[]? lot;
        string? connstr;

        public HistoricalWeatherBGservice
            (
            IConfiguration config,
            DBservice _dbcall
            )
        {
            dbcall = _dbcall;
            isEnabled = config["Historicaldb:Enabled"]?.ConvertstrtoB() ?? false;

          if(!TimeOnly.TryParse(config["Historicaldb:TimeToFetch"], out Time)) { isEnabled = false; }

            var geolat = config.GetSection("Historicaldb:GeolocationsLat");
            var geolot = config.GetSection("Historicaldb:GeolocationsLot");

            connstr = config["Historicaldb:Sqlite"];
            if (connstr is null)isEnabled = false;

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



            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("Hello");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);

            }
        }












    }

    //private bool convert(this string cv) => cv.ToLower() == "true" ?  true : false;

}
