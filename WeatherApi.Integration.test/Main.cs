using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Shared.MeteoData;


namespace WeatherApi.Integration.test
{
    public class ProgramTestApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {

        private readonly ForecastMockAPI forecastMockAPI = new();


        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

            builder.ConfigureServices(services =>
            {

                var backgroundsrv = services.FirstOrDefault( 
                    d => d.ServiceType == typeof(IHostedService) && 
                    d.ImplementationType == typeof(HistoricalWeather.Main)
                    );
                services.Remove(backgroundsrv);

                //var httpClientService = services.FirstOrDefault( d => d.ServiceType == typeof(IHttpClientFactory));      
                //services.Remove(httpClientService);

                var testurl = services.FirstOrDefault( d => d.ServiceType == typeof(MeteoApisBaseurls));
                services.Remove(testurl);

                var Redis = services.FirstOrDefault(d => d.ServiceType == typeof(IDistributedCache));
                services.Remove(Redis);


                services.AddSingleton(opt =>
                {
                    return new MeteoApisBaseurls(forecastMockAPI.BaseUrl, forecastMockAPI.BaseUrl, forecastMockAPI.BaseUrl, forecastMockAPI.BaseUrl);
                });

                services.AddSingleton<ForecastMockAPI>();

                //services.AddHttpClient("DefaultClient", opt =>
                // {
                //     opt.BaseAddress = new Uri(forecastMockAPI.BaseUrl);

                // });

            });
        }


        public async Task InitializeAsync()
        {
            forecastMockAPI.InitWireMock();
            //forecastMockAPI.WeatherMocApiSetup();
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            forecastMockAPI.Dispose();
        }
    }
}
