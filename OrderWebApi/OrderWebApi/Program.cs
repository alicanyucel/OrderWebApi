using Microsoft.EntityFrameworkCore;
using OrderWebApi.Models.Context;
using AutoMapper;
using OrderWebApi.Models.Mapper;
using Serilog;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using OrderAppWebApi.BackgroundService;

var builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetConnectionString("SqlServer");
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<SendMailService>();
Logger log = new LoggerConfiguration().WriteTo.File("logs/log.txt").WriteTo.MSSqlServer(connectionString, "logs").MinimumLevel.Information().CreateLogger();
builder.Host.UseSerilog(log);

builder.Services.AddDbContext<OrderContextDb>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});
builder.Services.AddMemoryCache();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
