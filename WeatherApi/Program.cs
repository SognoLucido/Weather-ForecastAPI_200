
using DapperSqlite;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.OpenApi.Any;
using OpenMeteoMain;
using OpenWeatherMapLogic;
using OpenWeathermapMain;
using Scalar.AspNetCore;
using Shared.IMeteo;
using Shared.MeteoData.Models;
using Shared.MeteoData.Models.Dto;
using System.ComponentModel.DataAnnotations;




var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Add(MeteoEnumJsonSerializerContext.Default);
    options.SerializerOptions.TypeInfoResolverChain.Add(GeoinfoResposte.Default);

});

//builder.Services.AddOutputCache(options =>
//{
//    options.AddBasePolicy(policy => policy.Expire(TimeSpan.FromMinutes(10)));
//});

builder.Services.Configure<RouteHandlerOptions>(o => o.ThrowOnBadRequest = false);

builder.Services.AddOpenApi(opt =>
{
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

//builder.Services.AddControllers();

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


builder.Services.AddScoped<IDataServiceLink, DataManager>(); //old
builder.Services.AddHttpClient<IServiceLink, MainOpenW>(); //old

builder.Services.AddKeyedScoped<IMeteoProvider, OpenMeteo>("openmeteo");
builder.Services.AddKeyedScoped<IMeteoProvider, OpenWeathermap>("openweatermap");





var app = builder.Build();

//app.UseOutputCache();
app.MapOpenApi();/*.CacheOutput();*/
//app.UseSwaggerUI(opt => opt.SwaggerEndpoint("/openapi/v1.json","weather api"));
app.MapScalarApiReference(opt =>
{
    opt.WithTitle("WeatherForecast API");


    // opt.DefaultFonts = false;
});



//var mt = app.MapGroup("api").WithTags("MeteoAPIs");

//app.MapGet("hello", () => "hello");





app.MapGet("/Geocoding/{City}", async (
    string City,    
    MeteoService meteoservice,
    HybridCache _cache,
    IConfiguration config,
    IServiceProvider provider,
    CancellationToken ct,
    [Range(1,100)]int? limit = 3
    ) =>
{



    //IMeteoProvider? request = meteoservice switch
    //{
    //    MeteoService.OpenMeteo => provider.GetKeyedService<IMeteoProvider>("openmeteo"),
    //    MeteoService.OpenWeathermap => provider.GetKeyedService<IMeteoProvider>("openweatermap")

    //};

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

    
    //return  Results.Ok(await store.ez());

})
    .Produces<GeoinfoplusProvider>(200)
    .Produces(404);





//mt.MapGet("{City}",async (string City, HybridCache _cache,CancellationToken ct) => 
//{
//    City = "-"+City.ToLower();

//    var test = _cache;


//    //var entryOptions = new HybridCacheEntryOptions
//    //{
//    //    // Flags = HybridCacheEntryFlags.DisableLocalCache,
//    //    Expiration = TimeSpan.FromMinutes(1),

//    //};


//        var x = await _cache.GetOrCreateAsync(
//        City, // Unique key to the cache entry
//        async cancel => await GetDataFromTheSourceAsync(City, cancel),
//        cancellationToken: ct);


//    return x;




//});





//app.UseResponseCaching();

//app.UseAuthorization();

//app.MapControllers();

app.Run();




static async Task<string> GetDataFromTheSourceAsync(string name, CancellationToken token)
{
    Console.WriteLine("RAW data GET");
    string someInfo = $"someinfo-{name}";
    return someInfo;
}









