
using Microsoft.Extensions.DependencyInjection;
using OpenWeatherMapLogic;
using System;
using WeatherApi_console.ConsoleOption;



var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddSingleton<IServiceLink,MainOpenW>();
builder.Services.AddHttpClient<IServiceLink,MainOpenW>();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddResponseCaching();

var app = builder.Build();


using(var consoleUI2 = new ConsoleUI(app.Services.GetService<IServiceLink>()!))
{
    if (await consoleUI2.ConsoleStart())
    {
        Console.ReadLine();
        return;
    }

}



//using(var serviceScope = app.Services.CreateScope())
//{
//    var services = serviceScope.ServiceProvider;

//    var myDependency = services.GetRequiredService<IServiceLink>();

//    //  ConsoleUI consoleUi = new ConsoleUI(app.Configuration ,app.Services.GetService<IServiceLink>());
//    ConsoleUI consoleUi = new ConsoleUI(app.Configuration ,myDependency);
//    if (await consoleUi.ConsoleStart())
//    {
//        Console.ReadLine();
//        return;
//    }
   
//}


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
