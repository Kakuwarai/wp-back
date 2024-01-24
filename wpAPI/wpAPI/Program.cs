using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using wpAPI.Models;

var builder = WebApplication.CreateBuilder(args);
    
// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionFleetDeclartion = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<WpdbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton(connectionFleetDeclartion);


builder.Services.AddCors(
       o =>
        o.AddPolicy(
            "Policy",
            builder =>
            {

                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }
         )
    );

var app = builder.Build();
app.UseCors("Policy");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();