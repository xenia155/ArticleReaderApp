
using System.Runtime.InteropServices.Marshalling;
using webapi;
using Microsoft.Extensions.Logging;
using Pullenti.Ner.Org;

// create a logger factory
var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
});

// create a logger
var logger = loggerFactory.CreateLogger<EntityExtractor>();


using (MyApplicationContext db = new MyApplicationContext())
{
    await db.Database.EnsureDeletedAsync();
    await db.Database.EnsureCreatedAsync();
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<MyApplicationContext>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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