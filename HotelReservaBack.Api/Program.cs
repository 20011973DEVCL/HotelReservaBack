using HotelReservaBack.Application.Interfaces;
using HotelReservaBack.Application.Services;
using HotelReservaBack.Domain.Interfaces;
using HotelReservaBack.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Servicios del contenedor
builder.Services.AddControllers();

// OpenAPI
builder.Services.AddOpenApi();

// Inyección de dependencias
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IHotelService, HotelService>();

var app = builder.Build();

// Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();