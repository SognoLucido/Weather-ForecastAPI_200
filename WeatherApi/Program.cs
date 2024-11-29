
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using OpenMeteoMain;
using OpenWeathermapMain;
using Scalar.AspNetCore;
using Shared.IMeteo;
using Shared.MeteoData.Models;
using Shared.MeteoData.Models.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;





var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Add(MeteoEnumJsonSerializerContext.Default);
    options.SerializerOptions.TypeInfoResolverChain.Add(GeoinfoResposte.Default);
    options.SerializerOptions.TypeInfoResolverChain.Add(ForecastDtoSGmodel.Default);
});

//builder.Services.AddOutputCache(options =>
//{
//    options.AddBasePolicy(policy => policy.Expire(TimeSpan.FromMinutes(10)));
//});

builder.Services.Configure<RouteHandlerOptions>(o => o.ThrowOnBadRequest = false);

builder.Services.AddOpenApi(opt =>
{

    opt.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Contact = new OpenApiContact
        {
            Name = "Francesco Barbano",
            Url = new Uri("https://github.com/SognoLucido/Weather-ForecastAPI_200")

        };
        return Task.CompletedTask;
    });



    opt.AddSchemaTransformer((schema, context, cancellationToken) =>
    {
        if (context.JsonTypeInfo.Type.IsEnum)
        {

            schema.Enum = Enum.GetNames(context.JsonTypeInfo.Type)
            .Select(name => new OpenApiString(name))
            .ToList<IOpenApiAny>();

        }
        return Task.CompletedTask;
    });
});

builder.Services.AddHttpClient();


builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "weather";

});

#pragma warning disable EXTEXP0018 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
builder.Services.AddHybridCache(opt =>
{
    opt.DefaultEntryOptions = new HybridCacheEntryOptions
    {
        Expiration = TimeSpan.FromMinutes(10),
        LocalCacheExpiration = TimeSpan.FromMinutes(5)
    };
});
#pragma warning restore EXTEXP0018 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.



builder.Services.AddKeyedScoped<IMeteoProvider, OpenMeteo>("openmeteo");
builder.Services.AddKeyedScoped<IMeteoProvider, OpenWeathermap>("openweatermap");



var app = builder.Build();


app.MapOpenApi();

app.MapScalarApiReference(opt =>
{
    opt.WithTitle("WeatherForecast API");


});



//var mt = app.MapGroup("api").WithTags("MeteoAPIs");


app.MapGet("/geocoding/{City}", async (
    string City,    
    MeteoService meteoservice,
    HybridCache _cache,
    IConfiguration config,
    IServiceProvider provider,
    CancellationToken ct,
    [Range(1,100)]int? limit = 3
    ) =>
{
    if (limit <= 0) return Results.BadRequest("Limit must be greater than 0.");

    IMeteoProvider? request = null;
    string? key = null;

    switch (meteoservice)
    {
        case MeteoService.OpenMeteo: request = provider.GetKeyedService<IMeteoProvider>("openmeteo"); break;
        case MeteoService.OpenWeathermap:
            {

                 key = config.GetValue<string>("OpenweathermapApi:ApiKey");

                if (key is null) return TypedResults.BadRequest("apikey Required check appsettings.json");

                request = provider.GetKeyedService<IMeteoProvider>("openweatermap");

            }; break;
    }


    var result = await request.GeoinfoModel(City,key) ;


    return result is null ?  Results.NotFound() : TypedResults.Ok(result);

    
   

})
    
    .WithDescription("Get Geolocation Position: Latitude and Longitude")
    .Produces<GeoinfoplusProvider>(200)
    .Produces<string>(400)
    .Produces(404);






app.MapGet("/forecast", async (
    double lat,
    double lon,
    MeteoService meteoservice,
    HybridCache _cache,
    IConfiguration config,
    IServiceProvider provider,
    CancellationToken ct,
    int? limit
    ) =>
{


    if (limit <= 0) return Results.BadRequest("Limit must be greater than 0.");

    IMeteoProvider? request = null;
    string? key = null;

    switch (meteoservice)
    {
        case MeteoService.OpenMeteo: request = provider.GetKeyedService<IMeteoProvider>("openmeteo"); break;
        case MeteoService.OpenWeathermap:
            {

                key = config.GetValue<string>("OpenweathermapApi:ApiKey");

                if (key is null) return TypedResults.BadRequest("apikey Required check appsettings.json");

                request = provider.GetKeyedService<IMeteoProvider>("openweatermap");

            }; break;
    }


    //double LAT = 44.40726;
    //double LON = 8.9338624;



    var result = await request.Forecast(lat, lon, limit, key);

    return result is null ? Results.NotFound() : TypedResults.Ok(result);

})
    .WithDescription("Forecast for multiple days (limited based on MeteoService and subscription tier) requires latitude and longitude")
    .Produces<ForecastDto>(200)
    .Produces<string>(400)
    .Produces(404);






app.MapGet("/oneshot/{City}", async (
    string City,
    MeteoService meteoservice,
    HybridCache _cache,
    IConfiguration config,
    IServiceProvider provider,
    int? limit,
    CancellationToken ct
    
    ) =>
{

    if (limit <= 0) return Results.BadRequest("Limit must be greater than 0.");

    IMeteoProvider? request = null;
    string? key = null;

    switch (meteoservice)
    {
        case MeteoService.OpenMeteo: request = provider.GetKeyedService<IMeteoProvider>("openmeteo"); break;
        case MeteoService.OpenWeathermap:
            {

                key = config.GetValue<string>("OpenweathermapApi:ApiKey");

                if (key is null) return TypedResults.BadRequest("apikey Required check appsettings.json");

                request = provider.GetKeyedService<IMeteoProvider>("openweatermap");

            }; break;
    }



    var Geoinfo = await request.GeoinfoModel(City, key);

    if (Geoinfo is null || Geoinfo.Geoinfo?.Count < 1) return TypedResults.NotFound();


    var result = await request.Forecast(Geoinfo.Geoinfo[0].lat, Geoinfo.Geoinfo[0].lon, limit, key);

    if(result is null) return TypedResults.NotFound();

    if(request is OpenMeteo)
    {
        result.City = Geoinfo.Geoinfo[0].name;
        result.Country_code = Geoinfo.Geoinfo[0].country;
    }
    

    //return result is null ? Results.NotFound() : TypedResults.Ok(result);

    return Results.Ok(result);

})
    .WithDescription("Combine geolocation and forecast (results may be inaccurate *wrong city)")
    .Produces<ForecastDto>(200)
    .Produces<string>(400)
    .Produces(404);








app.Run();




//static async Task<string> GetDataFromTheSourceAsync(string name, CancellationToken token)
//{
//    Console.WriteLine("RAW data GET");
//    string someInfo = $"someinfo-{name}";
//    return someInfo;
//}









