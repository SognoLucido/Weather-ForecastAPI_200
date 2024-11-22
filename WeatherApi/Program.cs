
using DapperSqlite;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using OpenWeatherMapLogic;
using StackExchange.Redis;
using System;
using System.Xml.Linq;
using WeatherApi.ConsoleOption;



var builder = WebApplication.CreateSlimBuilder(args);

//builder.Services.ConfigureHttpJsonOptions(options =>
//{
//    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
//});

builder.Services.AddOpenApi();

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


builder.Services.AddScoped<IDataServiceLink, DataManager>();
builder.Services.AddHttpClient<IServiceLink, MainOpenW>();



var app = builder.Build();





app.MapOpenApi();
app.UseSwaggerUI(opt => opt.SwaggerEndpoint("/openapi/v1.json","weather api"));



app.MapGroup("api").WithTags("MeteoAPIs");

//app.MapGet("")






app.MapGet("{City}",async (string City, HybridCache _cache,CancellationToken ct) => 
{
    City = "-"+City.ToLower();

    var test = _cache;


    //var entryOptions = new HybridCacheEntryOptions
    //{
    //    // Flags = HybridCacheEntryFlags.DisableLocalCache,
    //    Expiration = TimeSpan.FromMinutes(1),

    //};

    
        var x = await _cache.GetOrCreateAsync(
        City, // Unique key to the cache entry
        async cancel => await GetDataFromTheSourceAsync(City, cancel),
        cancellationToken: ct);
  

    return x;
   


    
});





//app.UseResponseCaching();

//app.UseAuthorization();

//app.MapControllers();

app.Run();




static async Task<string> GetDataFromTheSourceAsync(string name,  CancellationToken token)
{
    Console.WriteLine("RAW data GET");
    string someInfo = $"someinfo-{name}";
    return someInfo;
}
