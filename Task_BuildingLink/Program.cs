using System.Data;
using Microsoft.Data.Sqlite;
using Task_BuildingLink.Application;
using Task_BuildingLink.Domain.Contracts;
using Task_BuildingLink.Infrastructure.Repositories;
using Serilog;
using Task_BuildingLink.Middlewares;

var builder = WebApplication.CreateBuilder(args);

//register DB connetion

builder.Services.AddSingleton<IDbConnection>(_ => new SqliteConnection("Data Source=drivers.db"));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<IDriverRepository, DriverRepository>();
builder.Services.AddScoped<IDriverService, DriverService>();

//add serilog
Log.Logger = new LoggerConfiguration()
   // .WriteTo.Console()
    .WriteTo.File("logs/log.txt", 
       rollingInterval: RollingInterval.Day,
       outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}",
        retainedFileCountLimit: 7) // Keep logs for the last 7 days
    .CreateLogger();

builder.Host.UseSerilog();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();

