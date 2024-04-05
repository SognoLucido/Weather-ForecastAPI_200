
using DapperSqlite;
using Microsoft.Extensions.DependencyInjection;
using OpenWeatherMapLogic;
using System;
using WeatherApi_console.ConsoleOption;



var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddSingleton<IServiceLink,MainOpenW>();

builder.Services.AddScoped<IDataServiceLink,DataManager>();
builder.Services.AddHttpClient<IServiceLink,MainOpenW>();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddResponseCaching();

var app = builder.Build();



using (var serviceScope = app.Services.CreateScope())
{

    using (var consoleUi = new ConsoleUI(app.Services.GetRequiredService<IServiceLink>()!, serviceScope.ServiceProvider.GetRequiredService<IDataServiceLink>(), app.Configuration)) //
    { 
        if (await consoleUi.ConsoleStart())
        {
            Console.ReadLine();
            return;
        }
    }
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseResponseCaching();

app.UseAuthorization();

app.MapControllers();

app.Run();
